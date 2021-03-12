using HandItOver.BackEnd.Infrastructure.Services;
using System;
using System.Security.Cryptography;

namespace HandItOver.BackEnd.BLL.Services
{
    public class RefreshTokenFactory : IRefreshTokenFactory
    {
        public string GenerateRefreshToken()
        {
            const int tokenLengthInBytes = 32;
            var randomNumber = new byte[tokenLengthInBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
