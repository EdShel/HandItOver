using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public sealed class MailboxOwnerAuthorizationHandler : ResourceAccessAuthorizationHandler<MailboxOwnerAuthorizationHandler>
    {
        private readonly MailboxRepository mailboxRepository;

        public MailboxOwnerAuthorizationHandler(MailboxRepository mailboxRepository)
        {
            this.mailboxRepository = mailboxRepository;
        }

        protected override async Task<bool> IsOwnerAsync(string userId, string mailboxId)
        {
            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");
            return mailbox.OwnerId == userId;
        }
    }

}
