using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Entities
{
    public class MailboxRentService
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        private readonly RentRepository rentRepository;

        public MailboxRentService(MailboxGroupRepository mailboxGroupRepository, RentRepository rentRepository)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
            this.rentRepository = rentRepository;
        }

        public async Task<RentResult> RentMailbox(RentRequest request)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository
                .GetWhitelistByIdAsync(request.GroupId)
                ?? throw new NotFoundException("Mailbox group");
            bool allowedToRent = !mailboxGroup.WhitelistOnly
                || request.RenterId == mailboxGroup.OwnerId
                || mailboxGroup.Whitelisted.Any(u => u.Id == request.RenterId);
            if (!allowedToRent)
            {
                throw new NoRightsException("rent the mailbox group.");
            }

            var notOccupiedRightNow = await this.mailboxGroupRepository.MailboxesWithoutDelivery(mailboxGroup.GroupId);
            var notRentedForPeriod = await this.mailboxGroupRepository.MailboxesWithoutRent(mailboxGroup.GroupId, request.RentFrom, request.RentUntil);
            var willBeFreeToRent = notOccupiedRightNow.Union(notRentedForPeriod).ToArray();
            if (willBeFreeToRent.Length == 0)
            {
                throw new InvalidOperationException("No available mailboxes for the period.");
            }
            var possibleToRent = willBeFreeToRent.Where(m => request.PackageSize <= m.Size).ToArray();
            if (possibleToRent.Length == 0)
            {
                throw new InvalidOperationException("No mailboxes of suitable size.");
            }
            Mailbox toRent = possibleToRent.OrderBy(m => m.Size).First();
            MailboxRent rentRecord = new MailboxRent
            {
                MailboxId = toRent.Id,
                RenterId = request.RenterId,
                From = request.RentFrom,
                Until = request.RentUntil,
            };
            this.rentRepository.CreateRent(rentRecord);
            await this.rentRepository.SaveChangesAsync();

            return new RentResult(
                RentId: rentRecord.RentId,
                MailboxId: rentRecord.MailboxId,
                MailboxSize: toRent.Size,
                From: rentRecord.From,
                Until: rentRecord.Until
            );
        }

        public async Task CancelRent(string rentId)
        {
            MailboxRent rent = await this.rentRepository.FindByIdOrNull(rentId)
                ?? throw new NotFoundException("Rent record");
            this.rentRepository.DeleteRent(rent);
            await this.rentRepository.SaveChangesAsync();
        }

        public async Task<RentResult> GetRent(string rentId)
        {
            MailboxRent rent = await this.rentRepository.FindByIdOrNull(rentId)
                ?? throw new NotFoundException("Rent record");

            return new RentResult(
                RentId: rent.RentId,
                MailboxId: rent.MailboxId,
                MailboxSize: rent.Mailbox.Size,
                From: rent.From,
                Until: rent.Until
            );
        }
    }
}
