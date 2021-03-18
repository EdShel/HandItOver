using AutoMapper;
using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class UsersService
    {
        private readonly UserRepository userRepository;

        private readonly IMapper mapper;

        public UsersService(UserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
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

        public async Task<UsersPaginatedResult> GetUsersPaginated(UsersPaginatedRequest request)
        {
            if (request.PageIndex < 0)
            {
                throw new WrongValueException(nameof(request.PageIndex));
            }
            if (request.PageSize < 1)
            {
                throw new WrongValueException(nameof(request.PageSize));
            }
            const int maxPageSize = 100;
            request = request with { PageSize = Math.Max(request.PageSize, maxPageSize) };

            UserRepository.UserSearchResult usersPaginated = await this.userRepository.SearchUsersPaginatedAsync(
                query: request.SearchQuery,
                pageIndex: request.PageIndex,
                pageSize: request.PageSize
            );

            return new UsersPaginatedResult(
                TotalPages: usersPaginated.TotalPages,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                Users: usersPaginated.Users.Select(u => this.mapper.Map<UserPublicInfoResult>(u))
            );
        }
    }
}
