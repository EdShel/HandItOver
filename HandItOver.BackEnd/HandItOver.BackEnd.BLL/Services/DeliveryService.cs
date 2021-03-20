using AutoMapper;
using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Services.Notification;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class DeliveryService
    {
        private readonly MailboxRepository mailboxRepository;

        private readonly DeliveryRepository deliveryRepository;

        private readonly RentRepository rentRepository;

        private readonly UserRepository userRepository;

        private readonly NotificationsMessagesService notificationsMessagesService;

        private readonly FirebaseNotificationService firebaseNotificationService;

        private readonly ILogger<DeliveryService> logger;

        private readonly IMapper mapper;

        public DeliveryService(
            MailboxRepository mailboxRepository,
            DeliveryRepository deliveryRepository,
            RentRepository rentRepository,
            UserRepository userRepository,
            NotificationsMessagesService notificationsMessagesService,
            FirebaseNotificationService firebaseNotificationService,
            ILogger<DeliveryService> logger,
            IMapper mapper)
        {
            this.mailboxRepository = mailboxRepository;
            this.deliveryRepository = deliveryRepository;
            this.rentRepository = rentRepository;
            this.userRepository = userRepository;
            this.notificationsMessagesService = notificationsMessagesService;
            this.firebaseNotificationService = firebaseNotificationService;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<DeliveryArrivedResult> HandleDeliveryArrival(DeliveryArrivedRequest delivery)
        {
            if (delivery.Weight <= 0)
            {
                throw new WrongValueException("Delivery weight");
            }
            Mailbox mailbox = await this.mailboxRepository.FindByIdWithGroupOrNullAsync(delivery.MailboxId)
                ?? throw new WrongValueException("Mailbox");
            if (!mailbox.IsOpen)
            {
                throw new OperationException("Mailbox already contains a delivery.");
            }

            string addresse;
            DateTime? terminalTime;
            if (mailbox.MailboxGroup == null)
            {
                addresse = mailbox.OwnerId;
                terminalTime = null;
            }
            else
            {
                MailboxRent? rent = await this.rentRepository.FindForTimeOrNull(mailbox.Id, DateTime.UtcNow);
                // TODO: ?? rentRepository.NearestToTheTimePeriod()
                addresse = rent == null ? mailbox.OwnerId : rent.RenterId;
                terminalTime = mailbox.MailboxGroup.MaxRentTime == null
                    ? null
                    : DateTime.UtcNow.Add(mailbox.MailboxGroup.MaxRentTime.Value);
            }

            DateTime predictedTime;
            var deliveriesToUseInPrediction = await this.deliveryRepository.GetAllTaken();
            if (!deliveriesToUseInPrediction.Any())
            {
                const int basicPredictionDays = 2;
                predictedTime = DateTime.UtcNow.AddDays(basicPredictionDays);
            }
            else
            {
                var now = DateTime.UtcNow;
                predictedTime = now.Add(new DeliveryTimePredictor(
                    deliveriesToUseInPrediction.Select(
                        d => new DeliveryPredictionData((int)d.Arrived.DayOfWeek, d.Arrived.Hour, (d.Arrived.Month - 1) / 4, d.Weight, d.Taken!.Value - d.Arrived)
                    )
                ).Predict(new DeliveryPredictionData(
                    (int)now.DayOfWeek,
                    now.Hour,
                    (now.Month - 1) / 4,
                    delivery.Weight,
                    TimeSpan.Zero
                ))
                );
            }
            Delivery deliveryRecord = new Delivery
            {
                AddresseeId = addresse,
                MailboxId = delivery.MailboxId,
                Weight = delivery.Weight,
                Arrived = DateTime.UtcNow,
                TerminalTime = terminalTime,
                PredictedTakingTime = predictedTime, // TODO: add prediction here
                Taken = null
            };
            this.deliveryRepository.AddDelivery(deliveryRecord);

            mailbox.IsOpen = false;
            this.mailboxRepository.UpdateMailbox(mailbox);

            await this.deliveryRepository.SaveChangesAsync();

            try
            {
                var message = this.notificationsMessagesService.DeliveryArrived(addresse);
                await this.firebaseNotificationService.SendAsync(message);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogInformation(ex, "Can't send push notification.");
            }

            return new DeliveryArrivedResult(deliveryRecord.Id);
        }


        public async Task HandleDeliveryDisappeared(string mailboxId)
        {
            var delivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Delivery");
            try
            {
                var message = this.notificationsMessagesService.DeliveryTheft(delivery.AddresseeId);
                await this.firebaseNotificationService.SendAsync(message);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogInformation(ex, "Can't send push notification.");
            }
            delivery.Taken = DateTime.UtcNow;
            deliveryRepository.UpdateDelivery(delivery);
            await deliveryRepository.SaveChangesAsync();
        }


        public async Task RequestOpeningDelivery(string deliveryId)
        {
            Delivery currentDelivery = await this.deliveryRepository.FindByIdWithMailboxOrNullAsync(deliveryId)
                ?? throw new NotFoundException("Delivery");

            currentDelivery.Mailbox.IsOpen = true;
            this.mailboxRepository.UpdateMailbox(currentDelivery.Mailbox);

            currentDelivery.Taken = DateTime.UtcNow;
            this.deliveryRepository.UpdateDelivery(currentDelivery);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task<MailboxStatus> GetMailboxStatus(string mailboxId)
        {
            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");
            Delivery? currentDelivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId);
            string? renter = null;
            if (!mailbox.IsOpen)
            {
                if (currentDelivery != null
                    && currentDelivery.TerminalTime != null
                    && DateTime.UtcNow >= currentDelivery.TerminalTime)
                {
                    mailbox.IsOpen = true;
                    this.mailboxRepository.UpdateMailbox(mailbox);
                    await this.mailboxRepository.SaveChangesAsync();

                    try
                    {
                        var message = this.notificationsMessagesService.DeliveryExpiration(currentDelivery.AddresseeId);
                        await this.firebaseNotificationService.SendAsync(message);
                    }
                    catch (NotFoundException ex)
                    {
                        this.logger.LogInformation(ex, "Can't send push notification.");
                    }
                }
            }
            else
            {
                var currentRent = await this.rentRepository.FindForTimeWithRenterOrNull(mailboxId, DateTime.UtcNow);
                if (currentRent != null)
                {
                    renter = currentRent.Renter.FullName;
                }
            }
            return new MailboxStatus(
                MailboxId: mailbox.Id,
                IsOpen: mailbox.IsOpen,
                Renter: renter
            );
        }

        public async Task GiveAwayDeliveryRight(string deliveryId, string newAddresseeId)
        {
            Delivery delivery = await this.deliveryRepository.FindByIdOrNull(deliveryId)
                ?? throw new NotFoundException("Delivery");
            AppUser addresse = await this.userRepository.FindByIdOrNullAsync(newAddresseeId)
                ?? throw new NotFoundException("User");
            delivery.AddresseeId = addresse.Id;
            this.deliveryRepository.UpdateDelivery(delivery);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActiveDeliveryResult>> GetActiveDeliveries(string userId)
        {
            IEnumerable<Delivery> deliveries = await this.deliveryRepository.GetActiveDeliveriesOfUserAsync(userId);
            return this.mapper.Map<IEnumerable<ActiveDeliveryResult>>(deliveries);
        }
    }
}
