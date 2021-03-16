using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
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

            // TODO: notify addresse

            await this.deliveryRepository.SaveChangesAsync();
            return new DeliveryArrivedResult(deliveryRecord.Id);
        }


        public async Task HandleDeliveryDisappeared(string deliveryId)
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
