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
    public class AuthService
    {
        private readonly UserRepository usersRepository;

        private readonly IAuthTokenFactory authTokenFactory;

        private readonly IRefreshTokenFactory refreshTokenFactory;

        private readonly AuthSettings authSettings;

        public AuthService(UserRepository usersRepository, IAuthTokenFactory authTokenFactory, IRefreshTokenFactory refreshTokenFactory, AuthSettings authSettings)
        {
            this.usersRepository = usersRepository;
            this.authTokenFactory = authTokenFactory;
            this.refreshTokenFactory = refreshTokenFactory;
            this.authSettings = authSettings;
        }

        public async Task<LoginResult> LoginAsync(LoginRequest loginRequest)
        {
            AppUser? user = await this.usersRepository.FindByEmailOrNullAsync(loginRequest.Email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!await this.usersRepository.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new WrongValueException("Password");
            }

            var tokenClaims = GetTokenClaimsForUser(user);
            var refreshTokenValue = this.refreshTokenFactory.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Value = refreshTokenValue,
                Expires = DateTime.UtcNow.AddMinutes(this.authSettings.RefreshTokenLifetimeMinutes)
            };
            this.usersRepository.CreateRefreshToken(user, refreshToken);
            await this.usersRepository.SaveChangesAsync();

            return new LoginResult(
                Token: this.authTokenFactory.GenerateAuthToken(tokenClaims),
                RefreshToken: user.Email,
                Email: refreshTokenValue
            );
        }

        private static ClaimsIdentity GetTokenClaimsForUser(AppUser user)
        {
            var userClaims = new[]
                {
                    new Claim(AuthConstants.Claims.ID, user.Id),
                    new Claim(AuthConstants.Claims.EMAIL, user.Email),
                }
                .Concat(user.UserRoles.Select(r => new Claim(AuthConstants.Claims.ROLE, r.Role.Name)))
                .ToArray();
            return new ClaimsIdentity(userClaims);
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            AppUser? anotherUser = await this.usersRepository.FindByEmailOrNullAsync(request.Email);
            if (anotherUser != null)
            {
                throw new RecordAlreadyExistsException("User with email");
            }
            var user = new AppUser
            {
                Email = request.Email,
            };

            var addResult = await this.usersRepository.CreateUserAsync(user, request.Password);
            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException("Can't create user.");
            }
            var roleResult = await this.usersRepository.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                await this.usersRepository.DeleteUserAsync(user);
                throw new InvalidOperationException("Can't assign the role.");
            }
        }

        public async Task<RefreshResponse> RefreshTokenAsync(RefreshRequest refreshRequest)
        {
            var userPrincipal = new JwtTokenValidator(authSettings).ExtractPrincipalFromExpiredToken(refreshRequest.AuthToken);
            if (userPrincipal == null)
            {
                throw new WrongValueException("Authorization token");
            }

            string email = userPrincipal.FindFirstValue(AuthConstants.Claims.EMAIL);
            AppUser? user = await this.usersRepository.FindByEmailOrNullAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            RefreshToken? refreshToken = await this.usersRepository.GetRefreshToken(user, refreshRequest.RefreshToken);
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
                Value = newRefreshTokenValue,
                Expires = DateTime.UtcNow.AddMinutes(authSettings.RefreshTokenLifetimeMinutes)
            };
            this.usersRepository.CreateRefreshToken(user, newRefreshToken);
            await this.usersRepository.SaveChangesAsync();

            var userClaims = GetTokenClaimsForUser(user);
            return new RefreshResponse(
                AuthToken: authTokenFactory.GenerateAuthToken(userClaims),
                RefreshToken: newRefreshTokenValue
            );
        }

        public async Task RevokeTokenAsync(RevokeRequest revokeRequest)
        {
            AppUser? user = await this.usersRepository.FindByIdOrNull(revokeRequest.Id);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            RefreshToken? refreshToken = await this.usersRepository.GetRefreshToken(user, revokeRequest.RefreshToken);
            if (refreshToken == null)
            {
                throw new NotFoundException("Refresh token");
            }

            this.usersRepository.DeleteRefreshToken(refreshToken);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
