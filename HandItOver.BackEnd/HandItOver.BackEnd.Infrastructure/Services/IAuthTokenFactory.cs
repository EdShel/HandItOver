using System.Security.Claims;

namespace HandItOver.BackEnd.Infrastructure.Services
{
    public interface IAuthTokenFactory
    {
        public string GenerateAuthToken(ClaimsIdentity claims);
    }
}
