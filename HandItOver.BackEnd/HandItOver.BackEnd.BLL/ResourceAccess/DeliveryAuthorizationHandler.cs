using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public sealed class DeliveryAuthorizationHandler : ResourceAccessAuthorizationHandler<DeliveryAuthorizationHandler>
    {
        private readonly DeliveryRepository deliveryRepository;

        public DeliveryAuthorizationHandler(DeliveryRepository deliveryRepository)
        {
            this.deliveryRepository = deliveryRepository;
        }

        protected override async Task<bool> IsOwnerAsync(string userId, string deliveryId)
        {
            Delivery delivery = await this.deliveryRepository.FindByIdWithMailboxOrNullAsync(deliveryId)
                ?? throw new NotFoundException("Delivery record");
            return delivery.AddresseeId == userId || delivery.Mailbox.OwnerId == userId;
        }
    }
}
