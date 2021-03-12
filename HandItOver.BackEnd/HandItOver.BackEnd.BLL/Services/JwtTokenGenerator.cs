using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HandItOver.BackEnd.BLL.Services
{
    public class JwtTokenGenerator : IAuthTokenFactory
    {
        private readonly AuthSettings authSettings;

        public JwtTokenGenerator(AuthSettings authSettings)
        {
            this.authSettings = authSettings;
        }

        public string GenerateAuthToken(ClaimsIdentity claims)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: this.authSettings.ValidIssuer,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(this.authSettings.TokenLifetimeSeconds)),
                    signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.authSettings.SigningKey)),
                        algorithm: SecurityAlgorithms.HmacSha256
                    )
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
