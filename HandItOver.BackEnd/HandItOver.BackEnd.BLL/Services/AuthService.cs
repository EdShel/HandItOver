using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Auth;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository usersRepository;

        private readonly ITokenService tokenService;

        private readonly IRefreshTokenFactory refreshTokenFactory;

        public AuthService(UserRepository usersRepository, ITokenService tokenService, IRefreshTokenFactory refreshTokenFactory)
        {
            this.usersRepository = usersRepository;
            this.tokenService = tokenService;
            this.refreshTokenFactory = refreshTokenFactory;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request.Role == AuthConstants.Roles.ADMIN)
            {
                bool isAdminMyself = request.Registerer.IsInRole(AuthConstants.Roles.ADMIN);
                Task<bool> adminsExist = this.usersRepository.IsAnyoneOfRoleRegisteredAsync(AuthConstants.Roles.ADMIN);
                bool canRegisterAdmin = isAdminMyself || !await adminsExist;
                if (!canRegisterAdmin)
                {
                    throw new NoRightsException("register admin");
                }
            }

            AppUser? anotherUser = await this.usersRepository.FindByEmailOrNullAsync(request.Email);
            if (anotherUser != null)
            {
                throw new RecordAlreadyExistsException("User with email");
            }
            var user = new AppUser
            {
                Email = request.Email,
                FullName = request.FullName,
                UserName = request.Email
            };

            var addResult = await this.usersRepository.CreateUserAsync(user, request.Password);
            if (!addResult.Succeeded)
            {
                throw new OperationException("Can't create user.");
            }
            var roleResult = await this.usersRepository.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                await this.usersRepository.DeleteUserAsync(user);
                throw new OperationException("Can't assign the role.");
            }
        }

        public async Task<LoginResult> LoginAsync(LoginRequest loginRequest)
        {
            AppUser? user = await this.usersRepository.FindByEmailOrNullAsync(loginRequest.Email);
            if (user == null)
            {
                throw new WrongValueException("User");
            }

            if (!await this.usersRepository.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new WrongValueException("Password");
            }

            var tokenClaims = await GetTokenClaimsForUserAsync(user);
            var newRefreshToken = this.refreshTokenFactory.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Value = newRefreshToken.Value,
                Expires = newRefreshToken.Expires
            };
            this.usersRepository.CreateRefreshToken(user, refreshToken);
            await this.usersRepository.SaveChangesAsync();

            return new LoginResult(
                Token: this.tokenService.GenerateAuthToken(tokenClaims),
                Email: user.Email,
                RefreshToken: newRefreshToken.Value,
                FullName: user.FullName,
                Role: (await this.usersRepository.GetUserRolesAsync(user)).First()
            );
        }

        private async Task<ClaimsIdentity> GetTokenClaimsForUserAsync(AppUser user)
        {
            var roles = await this.usersRepository.GetUserRolesAsync(user);
            var userClaims = new[]
            {
                new Claim(AuthConstants.Claims.ID, user.Id),
                new Claim(AuthConstants.Claims.EMAIL, user.Email),
                new Claim(AuthConstants.Claims.ROLE, roles.First())
            };
            return new ClaimsIdentity(userClaims);
        }


        public async Task<RefreshResult> RefreshTokenAsync(RefreshRequest refreshRequest)
        {
            string authHeaderValue = refreshRequest.AuthHeaderValue;
            var userPrincipal = this.tokenService.ExtractPrincipalFromExpiredAuthHeader(authHeaderValue);
            if (userPrincipal == null)
            {
                throw new WrongValueException("Authorization token");
            }

            string id = userPrincipal.FindFirstValue(AuthConstants.Claims.ID);
            AppUser? user = await this.usersRepository.FindByIdOrNullAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            RefreshToken? refreshToken = await this.usersRepository.GetRefreshTokenAsync(user, refreshRequest.RefreshToken);
            if (refreshToken == null)
            {
                throw new WrongValueException("Refresh token");
            }

            if (refreshToken.IsExpired)
            {
                throw new ExpiredException("Refresh token");
            }

            this.usersRepository.DeleteRefreshToken(refreshToken);
            var newRefreshTokenValue = this.refreshTokenFactory.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken
            {
                Value = newRefreshTokenValue.Value,
                Expires = newRefreshTokenValue.Expires
            };
            this.usersRepository.CreateRefreshToken(user, newRefreshToken);
            await this.usersRepository.SaveChangesAsync();

            return new RefreshResult(
                AuthToken: this.tokenService.GenerateAuthToken(userPrincipal.Identities.First()),
                RefreshToken: newRefreshTokenValue.Value
            );
        }

        public async Task RevokeTokenAsync(RevokeRequest revokeRequest)
        {
            AppUser? user = await this.usersRepository.FindByIdOrNullAsync(revokeRequest.Id);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            RefreshToken? refreshToken = await this.usersRepository.GetRefreshTokenAsync(user, revokeRequest.RefreshToken);
            if (refreshToken == null)
            {
                throw new NotFoundException("Refresh token");
            }

            this.usersRepository.DeleteRefreshToken(refreshToken);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
