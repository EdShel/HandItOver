using HandItOver.BackEnd.BLL.Models.MailboxMessages;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Entities
{
    public class DeliveryService
    {
        private readonly MailboxRepository mailboxRepository;

        private readonly DeliveryRepository deliveryRepository;

        private readonly RentRepository rentRepository;

        private readonly UserRepository userRepository;

        public DeliveryService(
            MailboxRepository mailboxRepository,
            DeliveryRepository deliveryRepository,
            RentRepository rentRepository,
            UserRepository userRepository)
        {
            this.mailboxRepository = mailboxRepository;
            this.deliveryRepository = deliveryRepository;
            this.rentRepository = rentRepository;
            this.userRepository = userRepository;
        }

        public async Task HandleDeliveryArrival(DeliveryArrivedRequest delivery)
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

            // TODO: notify addresse

            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task RequestOpening(string mailboxId)
        {
            Delivery currentDelivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId)
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
            if (!mailbox.IsOpen)
            {
                if (currentDelivery == null)
                {
                    MailboxRent? currentRent = await this.rentRepository.FindForTimeOrNull(mailboxId, DateTime.UtcNow);
                    if (currentRent != null)
                    {
                        mailbox.IsOpen = true;
                        this.mailboxRepository.UpdateMailbox(mailbox);
                        await this.mailboxRepository.SaveChangesAsync();
                    }
                }
                else if (currentDelivery.TerminalTime != null
                        && DateTime.UtcNow >= currentDelivery.TerminalTime)
                {
                    mailbox.IsOpen = true;
                    this.mailboxRepository.UpdateMailbox(mailbox);
                    await this.mailboxRepository.SaveChangesAsync();
                }
            }
            return new MailboxStatus(
                MailboxId: mailbox.Id,
                IsOpen: mailbox.IsOpen,
                Info: mailbox.Info
            );
        }

        public async Task HandleDeliveryDisappeared()
        {
            // TODO: Call FBI
            await Task.CompletedTask;
        }

        public async Task GiveAwayDeliveryRight(string id, string to)
        {
            Delivery delivery = await this.deliveryRepository.FindByIdOrNull(id)
                ?? throw new NotFoundException("Delivery");
            AppUser addresse = await this.userRepository.FindByIdOrNullAsync(id)
                ?? throw new NotFoundException("User");
            delivery.AddresseeId = id;
            this.deliveryRepository.UpdateDelivery(delivery);
            await this.deliveryRepository.SaveChangesAsync();
        }
    }
}
