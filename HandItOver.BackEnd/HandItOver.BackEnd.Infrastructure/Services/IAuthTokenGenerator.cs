using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HandItOver.BackEnd.Infrastructure.Services
{
    public interface IAuthTokenGenerator
    {
        public string GenerateJwtToken(ClaimsIdentity claims);
    }

    public interface IRefreshTokenFactory
    {
        public string GenerateRefreshToken();
    }
}
