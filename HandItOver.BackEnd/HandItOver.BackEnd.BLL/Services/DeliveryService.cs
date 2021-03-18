using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Services.Notification;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
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

        private readonly FirebaseRepository firebaseRepository;

        public DeliveryService(
            MailboxRepository mailboxRepository,
            DeliveryRepository deliveryRepository,
            RentRepository rentRepository,
            UserRepository userRepository,
            NotificationsMessagesService notificationsMessagesService,
            FirebaseRepository firebaseRepository)
        {
            this.mailboxRepository = mailboxRepository;
            this.deliveryRepository = deliveryRepository;
            this.rentRepository = rentRepository;
            this.userRepository = userRepository;
            this.notificationsMessagesService = notificationsMessagesService;
            this.firebaseRepository = firebaseRepository;
        }

        public async Task<DeliveryArrivedResult> HandleDeliveryArrival(DeliveryArrivedRequest delivery)
        {
            Mailbox mailbox = await this.mailboxRepository.FindByIdWithGroupOrNullAsync(delivery.MailboxId)
                ?? throw new WrongValueException("Mailbox");
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

            Delivery deliveryRecord = new Delivery
            {
                AddresseeId = addresse,
                MailboxId = delivery.MailboxId,
                Weight = delivery.Weight,
                Arrived = DateTime.UtcNow,
                TerminalTime = terminalTime,
                PredictedTakingTime = DateTime.UtcNow.AddDays(7), // TODO: add prediction here
                Taken = null
            };
            this.deliveryRepository.AddDelivery(deliveryRecord);

            mailbox.IsOpen = false;
            this.mailboxRepository.UpdateMailbox(mailbox);

            (string title, string message) = this.notificationsMessagesService.DeliveryArrived;
            await this.firebaseRepository.SendMessageAsync(addresse, title, message);

            await this.deliveryRepository.SaveChangesAsync();
            return new DeliveryArrivedResult(deliveryRecord.Id);
        }


        public async Task HandleDeliveryDisappeared(string mailboxId)
        {
            var delivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Delivery");
            (string title, string message) = this.notificationsMessagesService.DeliveryTheft;
            await this.firebaseRepository.SendMessageAsync(delivery.AddresseeId, title, message);
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

                    (string title, string message) = this.notificationsMessagesService.DeliveryExpiration;
                    await this.firebaseRepository.SendMessageAsync(currentDelivery.AddresseeId, title, message);
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

        // TODO: replace with DTO
        public async Task<IEnumerable<Delivery>> GetActiveDeliveries(string userId)
        {
            var deliveries = await this.deliveryRepository.GetActiveDeliveriesOfUserAsync(userId);
            return deliveries;
        }
    }
}
