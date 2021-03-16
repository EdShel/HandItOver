using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class UsersService
    {
        private readonly UserRepository userRepository;

        public UsersService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserAccountInfoResult> GetInfoByIdAsync(string id)
        {
            AppUser? user = await this.userRepository.FindByIdOrNullAsync(id);
            return await MakeUserInfo(user);
        }

        public async Task<UserAccountInfoResult> GetInfoByEmailAsync(string email)
        {
            AppUser? user = await this.userRepository.FindByEmailOrNullAsync(email);
            return await MakeUserInfo(user);
        }

        public async Task<IEnumerable<UserPublicInfoResult>> FindByNameOrEmail(string seachQuery)
        {
            IEnumerable<AppUser> users = await this.userRepository.FindByNameOrEmailAsync(seachQuery);
            return users.Select(user => new UserPublicInfoResult(
                Id: user.Id,
                Email: user.Email,
                FullName: user.FullName
            ));
        }

        private async Task<UserAccountInfoResult> MakeUserInfo(AppUser? user)
        {
            if (user == null)
            {
                throw new NotFoundException("User");
            }
            var roles = await this.userRepository.GetUserRolesAsync(user);
            return new UserAccountInfoResult(
                Id: user.Id,
                Email: user.Email,
                FullName: user.FullName,
                Role: roles.First()
            );
        }
    }
}
