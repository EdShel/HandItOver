using HandItOver.BackEnd.BLL.Models.Auth;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class AuthService
    {
        private readonly UserRepository usersRepository;

        private readonly IRefreshTokenFactory refreshTokenFactory;

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
            var refreshToken = this.refreshTokenFactory.GenerateRefreshToken();
            await this.usersRepository.CreateRefreshTokenAsync(user, refreshToken);

            return new LoginResponseDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenFactory.GenerateTokenForClaims(tokenClaims),
                RefreshToken = refreshToken
            };
        }

        private IEnumerable<Claim> GetTokenClaimsForUser(AppUser user)
        {
            var userClaims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole),
                new Claim(AuthConstants.ID_CLAIM_TYPE, user.Id.ToString())
            };
            return userClaims;
        }

        public async Task RegisterAsync(RegisterDTO registerRequest)
        {
            bool emailIsTaken = await this.usersRepository.IsAnyUserWithEmailAsync(registerRequest.Email);
            if (emailIsTaken)
            {
                throw new BadRequestException("Email is taken!");
            }

            bool nameIsTaken = await this.usersRepository.IsAnyUserWithUserNameAsync(registerRequest.UserName);
            if (nameIsTaken)
            {
                throw new BadRequestException("User name is taken!");
            }

            var user = new AppUser
            {
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                UserRole = registerRequest.Role
            };
            await this.usersRepository.CreateAsync(user, registerRequest.Password);
        }

        public async Task<TokenRefreshResponseDTO> RefreshTokenAsync(TokenRefreshRequestDTO refreshRequest)
        {
            AppUser user = await this.usersRepository.GetUserOrDefaultByUserNameAsync(refreshRequest.UserName);
            if (user == null)
            {
                throw new BadRequestException("Not valid user!");
            }

            bool validRefreshToken = await this.usersRepository.HasRefreshTokenAsync(user, refreshRequest.RefreshToken);
            if (!validRefreshToken)
            {
                throw new BadRequestException("Not valid refresh token!");
            }

            await this.usersRepository.DeleteRefreshTokenAsync(user, refreshRequest.RefreshToken);

            var newRefreshToken = this.refreshTokenFactory.GenerateToken();
            await this.usersRepository.CreateRefreshTokenAsync(user, newRefreshToken);

            var userClaims = GetTokenClaimsForUser(user);
            return new TokenRefreshResponseDTO
            {
                Token = tokenFactory.GenerateTokenForClaims(userClaims),
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeTokenAsync(TokenRevokeDTO revokeRequest)
        {
            var user = await this.usersRepository.GetUserOrDefaultByUserNameAsync(revokeRequest.UserName);
            if (user == null)
            {
                throw new BadRequestException("Not valid user!");
            }

            bool validRefreshToken = await this.usersRepository.HasRefreshTokenAsync(user, revokeRequest.RefreshToken);
            if (!validRefreshToken)
            {
                throw new BadRequestException("Not valid refresh token!");
            }

            await this.usersRepository.DeleteRefreshTokenAsync(user, revokeRequest.RefreshToken);
        }
    }
}
