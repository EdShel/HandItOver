using AutoMapper;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Models.DeliveryTimePredictor;
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
    public class DeliveryService : IDeliveryService
    {
        private readonly MailboxRepository mailboxRepository;

        private readonly DeliveryRepository deliveryRepository;

        private readonly RentRepository rentRepository;

        private readonly UserRepository userRepository;

        private readonly NotificationsMessagesService notificationsMessagesService;

        private readonly INotificationService firebaseNotificationService;

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

        public async Task<DeliveryArrivedResult> HandleDeliveryArrivalAsync(DeliveryArrivedRequest delivery)
        {
            if (delivery.Weight <= 0)
            {
                throw new WrongValueException("Delivery weight");
            }
            Mailbox mailbox = await this.mailboxRepository.FindByIdWithGroupOrNullAsync(delivery.MailboxId)
                ?? throw new WrongValueException("Mailbox");

            var alreadyExistingDelivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(delivery.MailboxId);
            if (alreadyExistingDelivery != null)
            {
                throw new OperationException("Mailbox already contains a delivery.");
            }

            (DateTime? terminalTime, string addressee) = await FindTerminalTimeAndAddressee(mailbox);
            DateTime predictedTime = await PredictTimeAsync(delivery);

            Delivery deliveryRecord = new Delivery
            {
                AddresseeId = addressee,
                MailboxId = delivery.MailboxId,
                Weight = delivery.Weight,
                Arrived = DateTime.UtcNow,
                TerminalTime = terminalTime,
                PredictedTakingTime = predictedTime,
                Taken = null
            };
            this.deliveryRepository.AddDelivery(deliveryRecord);

            mailbox.IsOpen = false;
            this.mailboxRepository.UpdateMailbox(mailbox);

            await this.deliveryRepository.SaveChangesAsync();

            await SendArrivalNotificationAsync(addressee);

            return new DeliveryArrivedResult(deliveryRecord.Id);
        }

        private async Task<(DateTime?, string)> FindTerminalTimeAndAddressee(Mailbox mailbox)
        {
            string addressee;
            DateTime? terminalTime;

            if (mailbox.MailboxGroup == null)
            {
                addressee = mailbox.OwnerId;
                terminalTime = null;
            }
            else
            {
                MailboxRent? rent = await this.rentRepository.FindForTimeOrNull(mailbox.Id, DateTime.UtcNow);
                addressee = rent == null ? mailbox.OwnerId : rent.RenterId;
                terminalTime = mailbox.MailboxGroup.MaxRentTime == null
                    ? null
                    : DateTime.UtcNow.Add(mailbox.MailboxGroup.MaxRentTime.Value);
            }
            return (terminalTime, addressee);
        }

        private async Task<DateTime> PredictTimeAsync(DeliveryArrivedRequest delivery)
        {
            var deliveriesToUseInPrediction = await this.deliveryRepository.GetAllTakenAsync();
            if (!deliveriesToUseInPrediction.Any())
            {
                const int basicPredictionDays = 2;
                return DateTime.UtcNow.AddDays(basicPredictionDays);
            }
            DeliveryTimePredictor predictor = new DeliveryTimePredictor(
                deliveriesToUseInPrediction.Select(
                    d => new DeliveryPredictionData(
                        DayOfWeek: (int)d.Arrived.DayOfWeek,
                        Hour: d.Arrived.Hour,
                        Season: (d.Arrived.Month - 1) / 4,
                        Weight: d.Weight,
                        Duration: d.Taken!.Value - d.Arrived
                    )
                )
            );
            var now = DateTime.UtcNow;
            TimeSpan predictedTime = predictor.Predict(
                new DeliveryPredictionData(
                    (int)now.DayOfWeek,
                    now.Hour,
                    (now.Month - 1) / 4,
                    delivery.Weight,
                    TimeSpan.Zero
                )
            );
            return now.Add(predictedTime);
        }

        private async Task SendArrivalNotificationAsync(string addresse)
        {
            try
            {
                var message = this.notificationsMessagesService.DeliveryArrived(addresse);
                await this.firebaseNotificationService.SendAsync(message);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogInformation(ex, "Can't send push notification.");
            }
        }

        public async Task HandleDeliveryDisappearedAsync(string mailboxId)
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
            this.deliveryRepository.UpdateDelivery(delivery);
            await this.deliveryRepository.SaveChangesAsync();
        }


        public async Task RequestOpeningDeliveryAsync(string deliveryId)
        {
            Delivery currentDelivery = await this.deliveryRepository.FindByIdWithMailboxOrNullAsync(deliveryId)
                ?? throw new NotFoundException("Delivery");

            currentDelivery.Mailbox.IsOpen = true;
            this.mailboxRepository.UpdateMailbox(currentDelivery.Mailbox);

            currentDelivery.Taken = DateTime.UtcNow;
            this.deliveryRepository.UpdateDelivery(currentDelivery);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task<MailboxStatus> GetMailboxStatusAsync(string mailboxId)
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

        public async Task GiveAwayDeliveryRightAsync(string deliveryId, string newAddresseeId)
        {
            Delivery delivery = await this.deliveryRepository.FindByIdOrNull(deliveryId)
                ?? throw new NotFoundException("Delivery");
            AppUser addresse = await this.userRepository.FindByIdOrNullAsync(newAddresseeId)
                ?? throw new NotFoundException("User");
            delivery.AddresseeId = addresse.Id;
            this.deliveryRepository.UpdateDelivery(delivery);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActiveDeliveryResult>> GetActiveDeliveriesAsync(string userId)
        {
            IEnumerable<Delivery> deliveries = await this.deliveryRepository.GetActiveDeliveriesOfUserAsync(userId);
            return this.mapper.Map<IEnumerable<ActiveDeliveryResult>>(deliveries);
        }

        public async Task<IEnumerable<DeliveryResult>> GetRecentDeliveriesAsync(string mailboxId, int count)
        {
            const int maxNumberOfDeliveriesPerRequest = 20;
            count = Math.Clamp(count, 1, maxNumberOfDeliveriesPerRequest);
            var deliveries = await this.deliveryRepository.GetRecentDeliveriesOfMailboxAsync(mailboxId, count);
            return this.mapper.Map<IEnumerable<DeliveryResult>>(deliveries);
        }

        public async Task<DeliveryResult> GetDeliveryByIdAsync(string deliveryId)
        {
            Delivery delivery = await this.deliveryRepository.FindByIdOrNull(deliveryId)
                ?? throw new NotFoundException("Delivery");
            return this.mapper.Map<DeliveryResult>(delivery);
        }
    }
}
