using HandItOver.BackEnd.Infrastructure.Services;
using System;
using System.Security.Claims;

namespace HandItOver.BackEnd.BLL.Services
{
    public class JwtTokenGenerator : IAuthTokenGenerator
    {
        public string GenerateJwtToken(ClaimsIdentity claims)
        {
            throw new NotImplementedException();
        }
    }
}
