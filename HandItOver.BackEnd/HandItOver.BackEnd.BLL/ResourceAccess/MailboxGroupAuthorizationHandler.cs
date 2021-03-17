using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public sealed class MailboxGroupAuthorizationHandler : ResourceAccessAuthorizationHandler<MailboxGroupAuthorizationHandler>
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        public MailboxGroupAuthorizationHandler(MailboxGroupRepository mailboxGroupRepository)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
        }

        protected override async Task<bool> IsOwnerAsync(string userId, string groupId)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.FindByIdOrNullAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");
            return mailboxGroup.OwnerId == userId;
        }
    }

}
