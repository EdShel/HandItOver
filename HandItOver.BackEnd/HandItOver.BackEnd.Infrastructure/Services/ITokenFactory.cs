using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;

namespace HandItOver.BackEnd.Infrastructure.Services
{
    public interface ITokenService
    {
        ClaimsPrincipal? ExtractPrincipalFromExpiredAuthHeader(string headerValue);

        TokenValidationParameters ValidationParameters { get; }

        string GenerateAuthToken(ClaimsIdentity claims);
    }
}
