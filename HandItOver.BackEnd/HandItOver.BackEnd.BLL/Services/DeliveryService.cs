using HandItOver.BackEnd.BLL.Models.MailboxMessages;
using HandItOver.BackEnd.DAL.Entities;
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

        public async Task HandleDeliveryArrival(DeliveryArrivedRequest delivery)
        {
            Mailbox mailbox = await this.mailboxRepository.FindByIdWithGroupOrNullAsync(delivery.MailboxId)
                ?? throw new WrongValueException("Mailbox");
            string addresse;
            if (mailbox.MailboxGroup == null)
            {
                addresse = mailbox.OwnerId;
            }
            else
            {
                MailboxRent? rent = await this.rentRepository.FindForTimeOrNull(mailbox.Id, DateTime.UtcNow);
                // TODO: ?? rentRepository.NearestToTheTimePeriod()
                addresse = rent == null ? mailbox.OwnerId : rent.RenterId;
            }

            Delivery deliveryRecord = new Delivery
            {
                AddresseeId = addresse,
                MailboxId = delivery.MailboxId,
                Weight = delivery.Weight,
                Arrived = DateTime.UtcNow,
                Taken = null,
            };
            this.deliveryRepository.AddDelivery(deliveryRecord);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task RequestOpening(string mailboxId)
        {
            Delivery currentDelivery = await this.deliveryRepository.GetCurrentDeliveryAsync(mailboxId)
                ?? throw new NotFoundException("Delivery");
            currentDelivery.Taken = DateTime.UtcNow;
            this.deliveryRepository.UpdateDelivery(currentDelivery);
            this.deliveryRepository.UpdateDelivery(currentDelivery);
        }

        public async Task<bool> IsRequestedOpening(string mailboxId)
        {
            return (await this.deliveryRepository.GetCurrentDeliveryAsync(mailboxId)) == null
                || ((await this.rentRepository.FindForTimeOrNull(mailboxId, DateTime.UtcNow)) != null);
        }

        public async Task HandleDeliveryDisappeared()
        {
            // TODO: Call FBI
        }
    }
}
