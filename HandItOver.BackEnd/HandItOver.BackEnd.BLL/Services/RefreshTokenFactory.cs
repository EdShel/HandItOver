using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using System;
using System.Security.Cryptography;

namespace HandItOver.BackEnd.BLL.Services
{
    public class RefreshTokenFactory : IRefreshTokenFactory
    {
        private readonly int durationMinutes;

        public RefreshTokenFactory(RefreshTokenSettings settings)
        {
            this.durationMinutes = settings.RefreshTokenLifetimeMinutes;
        }

        public IRefreshToken GenerateRefreshToken()
        {
            return new RefreshToken(
                Value: GenerateRefreshTokenValue(),
                Expires: DateTime.UtcNow.AddMinutes(this.durationMinutes)
            );
        }

        private static string GenerateRefreshTokenValue()
        {
            const int tokenLengthInBytes = 32;
            var randomNumber = new byte[tokenLengthInBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private record RefreshToken(
            string Value,
            DateTime Expires
        ) : IRefreshToken;
    }
}
