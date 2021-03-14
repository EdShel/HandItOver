using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Entities
{
    public class MailboxAccessControlService
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        private readonly UserRepository userRepository;

        public MailboxAccessControlService(
            MailboxGroupRepository mailboxGroupRepository,
            UserRepository userRepository)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
            this.userRepository = userRepository;
        }

        public async Task<WhitelistInfo> GetMailboxWhitelist(string groupId)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            return new WhitelistInfo(
                MailboxGroupId: mailboxGroup.GroupId,
                Entries: mailboxGroup.Whitelisted.Select(
                    u => new WhitelistEntry(
                        Email: u.Email,
                        Id: u.Id
                    )
                )
            );
        }

        public async Task AddUserToWhitelistAsync(string groupId, string userEmail)
        {
            AppUser user = await this.userRepository.FindByEmailOrNullAsync(userEmail)
                ?? throw new NotFoundException("User");
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            if (mailboxGroup.Whitelisted.Contains(user))
            {
                throw new RecordAlreadyExistsException("User in whitelist");
            }

            mailboxGroup.Whitelisted.Add(user);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task RemoveUserFromWhitelistAsync(string groupId, string userEmail)
        {
            AppUser user = await this.userRepository.FindByEmailOrNullAsync(userEmail)
                ?? throw new NotFoundException("User");
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            if (!mailboxGroup.Whitelisted.Contains(user))
            {
                throw new WrongValueException("Mailbox group whitelist");
            }

            mailboxGroup.Whitelisted.Remove(user);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }
    }
}
