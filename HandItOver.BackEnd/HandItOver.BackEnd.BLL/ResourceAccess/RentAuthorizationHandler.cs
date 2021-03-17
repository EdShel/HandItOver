using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public sealed class RentAuthorizationHandler : ResourceAccessAuthorizationHandler<RentAuthorizationHandler>
    {
        private readonly RentRepository rentRepository;

        public RentAuthorizationHandler(RentRepository rentRepository)
        {
            this.rentRepository = rentRepository;
        }

        protected override async Task<bool> IsOwnerAsync(string userId, string rentId)
        {
            MailboxRent rent = await this.rentRepository.FindByIdWithMailboxOrNull(rentId)
                ?? throw new NotFoundException("Mailbox group");
            return rent.RenterId == userId || rent.Mailbox.OwnerId == userId;
        }
    }
}
