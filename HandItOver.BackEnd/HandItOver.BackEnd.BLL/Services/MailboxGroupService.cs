using AutoMapper;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class MailboxGroupService : IMailboxGroupService
    {
        private readonly UserRepository userRepository;

        private readonly MailboxRepository mailboxRepository;

        private readonly MailboxGroupRepository mailboxGroupRepository;

        private readonly IMapper mapper;

        public MailboxGroupService(
            UserRepository userRepository,
            MailboxRepository mailboxRepository,
            MailboxGroupRepository mailboxGroupRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mailboxRepository = mailboxRepository;
            this.mailboxGroupRepository = mailboxGroupRepository;
            this.mapper = mapper;
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
                OwnerId = request.OwnerId,
                WhitelistOnly = request.WhitelistOnly,
                MaxRentTime = request.MaxRentTime,
                Mailboxes = new List<Mailbox>()
            };
            this.mailboxGroupRepository.CreateMailboxGroup(newMailboxGroup);
            newMailboxGroup.Mailboxes.Add(mailbox);
            await this.mailboxGroupRepository.SaveChangesAsync();

            return new MailboxGroupCreatedResult(
                GroupId: newMailboxGroup.GroupId
            );
        }

        public async Task<MailboxGroupViewResult> GetMailboxGroupById(string groupId)
        {
            var group = await this.mailboxGroupRepository.FindByIdOrNullAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");
            return this.mapper.Map<MailboxGroupViewResult>(group);
        }

        public async Task<MailboxGroup> GetMailboxGroupByNameAsync(string name)
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

        public async Task EditMailboxGroupAsync(MailboxGroupEditRequest request)
        {
            var mailboxGroup = this.mapper.Map<MailboxGroup>(request);
            this.mailboxGroupRepository.ReplaceMailboxGroup(mailboxGroup);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MailboxGroupSearchResult>> FindMailboxesAsync(MailboxGroupSearchRequest request)
        {
            MailboxGroup[] groups = await this.mailboxGroupRepository.FindByNameOrAddressOrOwnerAsync(request.SearchQuery);
            return groups.Select(group => new MailboxGroupSearchResult(
                Id: group.GroupId,
                Owner: group.Owner.FullName,
                Name: group.Name
            ));
        }

        public async Task AddMailboxToGroupAsync(string groupId, string mailboxId)
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

        public async Task RemoveMailboxFromGroupAsync(string groupId, string mailboxId)
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

        public async Task<MailboxGroupStats> GetStatsAsync(string groupId)
        {
            MailboxGroup group = await this.mailboxGroupRepository.FindByIdFullInfoAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            return new MailboxGroupStats(
                group.GroupId,
                group.Name,
                group.Mailboxes.Select(
                    mb => new MailboxStats(
                        mb.Id,
                        mb.Size,
                        mb.Deliveries.Select(
                            d => new DeliveryStats(
                                Arrived: d.Arrived,
                                Taken: d.Taken,
                                PredictedTakingTime: d.PredictedTakingTime,
                                Weight: d.Weight
                            )
                        ).FirstOrDefault(),
                        mb.Rents.Select(
                            r => new RentStats(
                                RenterName: r.Renter.Email,
                                From: r.From,
                                Until: r.Until
                            )
                        )
                    )
                )
            );
        }
    }
}
