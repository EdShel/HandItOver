using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.BLL.Models.MailboxGroup;
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

    public class MailboxGroupService
    {
        private readonly UserRepository userRepository;

        private readonly MailboxRepository mailboxRepository;

        private readonly MailboxGroupRepository mailboxGroupRepository;

        public MailboxGroupService(
            UserRepository userRepository,
            MailboxRepository mailboxRepository,
            MailboxGroupRepository mailboxGroupRepository)
        {
            this.userRepository = userRepository;
            this.mailboxRepository = mailboxRepository;
            this.mailboxGroupRepository = mailboxGroupRepository;
        }

        public async Task<MailboxGroupCreatedResult> CreateMailboxGroupAsync(MailboxGroupCreateRequest request)
        {
            AppUser owner = await this.userRepository.FindByIdOrNullAsync(request.OwnerId)
                ?? throw new NotFoundException("Mailbox group owner");

            MailboxGroup? existantGroup = await this.mailboxGroupRepository.FindByNameOrNullAsync(request.Name);
            if (existantGroup != null)
            {
                throw new RecordAlreadyExistsException("Mailbox group");
            }

            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(request.FirstMailboxId)
                ?? throw new NotFoundException("First mailbox");
            if (mailbox.GroupId != null)
            {
                throw new WrongValueException("First mailbox's group");
            }

            MailboxGroup newMailboxGroup = new MailboxGroup
            {
                Name = request.Name,
                OnwerId = request.OwnerId,
                WhitelistOnly = request.WhitelistOnly
            };
            this.mailboxGroupRepository.CreateMailboxGroup(newMailboxGroup);
            newMailboxGroup.Mailboxes.Add(mailbox);
            await this.mailboxGroupRepository.SaveChangesAsync();

            return new MailboxGroupCreatedResult(
                GroupId: newMailboxGroup.GroupId
            );
        }

        public async Task<MailboxGroup> GetMailboxGroupById(string groupId)
        {
            return await this.mailboxGroupRepository.FindByIdOrNullAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");
        }

        public async Task<MailboxGroup> GetMailboxGroupByName(string name)
        {
            return await this.mailboxGroupRepository.FindByNameOrNullAsync(name)
                ?? throw new NotFoundException("Mailbox group");
        }

        public async Task DeleteMailboxGroupAsync(string mailboxGroupId)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.FindByIdOrNullAsync(mailboxGroupId)
                ?? throw new NotFoundException("Mailbox group");
            this.mailboxGroupRepository.DeleteMailboxGroup(mailboxGroup);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task AddMailboxToGroupAsync(string mailboxId, string groupId)
        {
            MailboxGroup group = await this.mailboxGroupRepository.FindByIdOrNullAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");
            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");

            if (group.Mailboxes.Contains(mailbox))
            {
                throw new RecordAlreadyExistsException("Mailbox in group");
            }

            group.Mailboxes.Add(mailbox);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task RemoveMailboxFromGroupAsync(string mailboxId, string groupId)
        {
            MailboxGroup group = await this.mailboxGroupRepository.FindByIdOrNullAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");
            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");

            if (!group.Mailboxes.Contains(mailbox))
            {
                throw new WrongValueException("Mailbox group");
            }

            group.Mailboxes.Remove(mailbox);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }
    }
}
