using HandItOver.BackEnd.BLL.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserPublicInfoResult>> FindByNameOrEmail(string seachQuery);
        Task<UserAccountInfoResult> GetInfoByEmailAsync(string email);
        Task<UserAccountInfoResult> GetInfoByIdAsync(string id);
        Task<UsersPaginatedResult> GetUsersPaginated(UsersPaginatedRequest request);
    }
}