using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
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

        public async Task<UserInfoResult> GetInfoByIdAsync(string id)
        {
            AppUser? user = await this.userRepository.FindByIdOrNullAsync(id);
            return MakeUserInfo(user);
        }

        public async Task<UserInfoResult> GetInfoByEmailAsync(string email)
        {
            AppUser? user = await this.userRepository.FindByEmailOrNullAsync(email);
            return MakeUserInfo(user);
        }

        private UserInfoResult MakeUserInfo(AppUser? user)
        {
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            return new UserInfoResult(
                Id: user.Id,
                Email: user.Email,
                Role: user.UserRoles.First().Role.Name
            );
        }
    }
}
