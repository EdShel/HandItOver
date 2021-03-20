using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IMailboxAccessControlService
    {
        Task AddUserToWhitelistAsync(string groupId, string userEmail);
        Task<JoinTokenModel> CreateWhitelistJoinTokenAsync(string groupId);
        Task DeleteToken(string groupId, string tokenId);
        Task<IEnumerable<JoinTokenModel>> GetAllTokensAsync(string groupId);
        Task<WhitelistInfo> GetMailboxWhitelist(string groupId);
        Task JoinWhitelistByToken(string groupId, string tokenValue, string userEmail);
        Task RemoveUserFromWhitelistAsync(string groupId, string userEmail);
    }
}