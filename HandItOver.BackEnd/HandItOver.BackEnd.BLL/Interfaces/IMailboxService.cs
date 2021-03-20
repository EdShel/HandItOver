using HandItOver.BackEnd.BLL.Models.Mailbox;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxService
    {
        Task<MailboxAuthResult> AuthorizeMailboxAsync(MailboxAuthRequest request);
        Task<MailboxViewResult> GetMailbox(string mailboxId);
        Task<IEnumerable<MailboxViewResult>> GetOwnedMailboxes(string userId);
    }
}