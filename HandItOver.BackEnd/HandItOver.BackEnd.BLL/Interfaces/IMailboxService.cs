using HandItOver.BackEnd.BLL.Models.Mailbox;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxService
    {
        Task<MailboxAuthResult> AuthorizeMailboxAsync(MailboxAuthRequest request);
        Task<MailboxViewResult> EditMailboxAsync(string mailboxId, MailboxEditRequest changes);
        Task<MailboxViewResult> GetMailboxAsync(string mailboxId);
        Task<IEnumerable<MailboxViewResult>> GetOwnedMailboxesAsync(string userId);
    }
}