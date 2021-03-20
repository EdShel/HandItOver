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
        Task EditMailboxGroupAsync(MailboxGroupEditRequest request);
        Task<IEnumerable<MailboxGroupSearchResult>> FindMailboxesAsync(MailboxGroupSearchRequest request);
        Task<MailboxGroupViewResult> GetMailboxGroupById(string groupId);
        Task<MailboxGroup> GetMailboxGroupByNameAsync(string name);
        Task<MailboxGroupStats> GetStatsAsync(string groupId);
        Task RemoveMailboxFromGroupAsync(string groupId, string mailboxId);
    }
}