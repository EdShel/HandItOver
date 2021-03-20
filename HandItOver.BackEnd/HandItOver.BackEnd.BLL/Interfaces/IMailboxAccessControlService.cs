using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxAccessControlService
    {
        Task AddUserToWhitelistAsync(string groupId, string userEmail);
        Task<JoinTokenModel> CreateWhitelistJoinTokenAsync(string groupId);
        Task DeleteTokenAsync(string groupId, string tokenId);
        Task<IEnumerable<JoinTokenModel>> GetAllTokensAsync(string groupId);
        Task<WhitelistInfo> GetMailboxWhitelistAsync(string groupId);
        Task JoinWhitelistByTokenAsync(string groupId, string tokenValue, string userEmail);
        Task RemoveUserFromWhitelistAsync(string groupId, string userEmail);
    }
}