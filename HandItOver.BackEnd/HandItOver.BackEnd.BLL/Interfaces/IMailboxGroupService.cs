using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxGroupService
    {
        Task AddMailboxToGroupAsync(string groupId, string mailboxId);
        Task<MailboxGroupCreatedResult> CreateMailboxGroupAsync(MailboxGroupCreateRequest request);
        Task DeleteMailboxGroupAsync(string mailboxGroupId);
        Task EditMailboxGroup(MailboxGroupEditRequest request);
        Task<IEnumerable<MailboxGroupSearchResult>> FindMailboxes(MailboxGroupSearchRequest request);
        Task<MailboxGroupViewResult> GetMailboxGroupById(string groupId);
        Task<MailboxGroup> GetMailboxGroupByName(string name);
        Task<MailboxGroupStats> GetStats(string groupId);
        Task RemoveMailboxFromGroupAsync(string groupId, string mailboxId);
    }
}