using AutoMapper;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class MailboxRentService : IMailboxRentService
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        private readonly RentRepository rentRepository;

        private readonly DeliveryRepository deliveryRepository;

        private readonly IMapper mapper;

        public MailboxRentService(
            MailboxGroupRepository mailboxGroupRepository,
            RentRepository rentRepository,
            IMapper mapper, DeliveryRepository deliveryRepository)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
            this.rentRepository = rentRepository;
            this.mapper = mapper;
            this.deliveryRepository = deliveryRepository;
        }

        public async Task<RentResult> RentMailboxAsync(RentRequest request)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository
                .FindWithWhitelistById(request.GroupId)
                ?? throw new NotFoundException("Mailbox group");
            if (!IsUserAllowedToRentGroup(request.RenterId, mailboxGroup))
            {
                throw new NoRightsException("rent the mailbox group");
            }
            if (request.RentFrom >= request.RentUntil)
            {
                throw new WrongValueException("Rent period");
            }
            if (mailboxGroup.MaxRentTime != null
                && (request.RentUntil - request.RentFrom).TotalHours >= mailboxGroup.MaxRentTime.Value.TotalHours)
            {
                throw new NoRightsException($"rent more than for {mailboxGroup.MaxRentTime.Value.TotalHours} hours.");
            }

            IList<Mailbox> possibleToRent = await FindMailboxesToRent(request);
            Mailbox toRent = possibleToRent.OrderBy(m => m.Size).First();
            MailboxRent rentRecord = new MailboxRent
            {
                MailboxId = toRent.Id,
                RenterId = request.RenterId,
                From = request.RentFrom,
                Until = request.RentUntil,
                Mailbox = toRent
            };
            this.rentRepository.CreateRent(rentRecord);
            await this.rentRepository.SaveChangesAsync();

            return this.mapper.Map<RentResult>(rentRecord);
        }

        private async Task<IList<Mailbox>> FindMailboxesToRent(RentRequest request)
        {
            var mailboxesOfSuitableSize = await this.mailboxGroupRepository
                .GetMailboxesOfSizeOrBiggerAsync(request.GroupId, request.PackageSize);
            if (!mailboxesOfSuitableSize.Any())
            {
                throw new OperationException("No mailboxes of suitable size.");
            }

            var vacantIntervals = (await Task.WhenAll(
                mailboxesOfSuitableSize.Select(mb => GetVacantTimeIntervalsForMailboxAsync(mb))
            )).Zip(mailboxesOfSuitableSize);

            var vacantRightNow = vacantIntervals.Where(
                mb => mb.First.Any(
                    interval => interval.Begin <= request.RentUntil 
                        && (interval.End == null || request.RentFrom <= interval.End.Value)
                )
            ).Select(mb => mb.Second)
             .ToList();

            if (!vacantRightNow.Any())
            {
                throw new OperationException("No available mailboxes for the period.");
            }

            return vacantRightNow;
        }

        private static bool IsUserAllowedToRentGroup(string userId, MailboxGroup mailboxGroup)
        {
            return !mailboxGroup.WhitelistOnly
                || userId == mailboxGroup.OwnerId
                || mailboxGroup.Whitelisted.Any(u => u.Id == userId);
        }

        public async Task<IList<TimeInterval>> FindVacantIntervalsToRent(RentTimeCheckRequest request)
        {
            MailboxGroup group = await this.mailboxGroupRepository.FindByIdWithMailboxesOrNullAsync(request.GroupId)
                ?? throw new NotFoundException("Mailbox group");
            var freeIntervals = new List<TimeInterval>();
            foreach (var mailbox in group.Mailboxes)
            {
                if (mailbox.Size < request.PackageSize)
                {
                    continue;
                }

                var vacantIntervalsOfThisMailbox = await GetVacantTimeIntervalsForMailboxAsync(mailbox);

                vacantIntervalsOfThisMailbox.RemoveAll(
                    interval => freeIntervals.Any(addedInterval => addedInterval.DoesFullyContain(interval))
                );

                freeIntervals.RemoveAll(
                    addedInterval => vacantIntervalsOfThisMailbox.Any(interval => interval.DoesFullyContain(addedInterval))
                );

                freeIntervals.AddRange(vacantIntervalsOfThisMailbox);
            }
            return freeIntervals.OrderBy(interval => interval.Begin).ToList();
        }

        private async Task<List<TimeInterval>> GetVacantTimeIntervalsForMailboxAsync(Mailbox mailbox)
        {
            var intervals = (await this.rentRepository.FindRentsIntervalsAsync(mailbox.Id)).ToList();

            var delivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailbox.Id);
            if (delivery != null)
            {
                intervals.Insert(0, new TimeInterval(delivery.Arrived, delivery.TerminalTime));
            }

            var firstInterval = intervals.FirstOrDefault();
            if (firstInterval == null)
            {
                return new List<TimeInterval>(1) { new TimeInterval(DateTime.UtcNow, null) };
            }

            var vacantIntervalsOfThisMailbox = new List<TimeInterval>();
            DateTime lastVacantTime = DateTime.UtcNow;
            foreach (var currentRentInterval in intervals)
            {
                bool isIntervalInPast = currentRentInterval.End.HasValue && currentRentInterval.End < lastVacantTime;
                if (isIntervalInPast)
                {
                    continue;
                }

                if (!currentRentInterval.DoesContain(lastVacantTime))
                {
                    vacantIntervalsOfThisMailbox.Add(new TimeInterval(lastVacantTime, currentRentInterval.Begin));
                }
                bool isThisRentLimitless = currentRentInterval.End == null;
                if (!isThisRentLimitless)
                {
                    lastVacantTime = currentRentInterval.End!.Value;
                }
                else
                {
                    break;
                }
            }
            vacantIntervalsOfThisMailbox.Add(new TimeInterval(lastVacantTime, null));
            RemoveShortIntervals(vacantIntervalsOfThisMailbox);

            return vacantIntervalsOfThisMailbox;
        }

        private static void RemoveShortIntervals(List<TimeInterval> vacantIntervalsOfThisMailbox)
        {
            const int minRentTimeMinutes = 15;
            vacantIntervalsOfThisMailbox.RemoveAll(
                interval => interval.GetDuration() != null
                    && interval.GetDuration()!.Value.TotalMinutes < minRentTimeMinutes
            );
        }

        public async Task CancelRentAsync(string rentId)
        {
            MailboxRent rent = await this.rentRepository.FindByIdOrNullAsync(rentId)
                ?? throw new NotFoundException("Rent record");
            this.rentRepository.DeleteRent(rent);
            await this.rentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<RentResult>> GetRentsForMailboxGroupAsync(string groupId)
        {
            IEnumerable<MailboxRent> rents = await this.rentRepository.GetByMailboxGroupAsync(groupId);
            return this.mapper.Map<IEnumerable<RentResult>>(rents);
        }

        public async Task<RentResult> GetRentAsync(string rentId)
        {
            MailboxRent rent = await this.rentRepository.FindByIdOrNullAsync(rentId)
                ?? throw new NotFoundException("Rent record");

            return this.mapper.Map<RentResult>(rent);
        }

        public async Task<IEnumerable<RentResult>> GetRentsOfUserAsync(string userId)
        {
            IEnumerable<MailboxRent> rents = await this.rentRepository.GetByRenterAsync(userId);
            return this.mapper.Map<IEnumerable<RentResult>>(rents);
        }
    }
}
