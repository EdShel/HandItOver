using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HandItOver.BackEnd.BLL.Services
{
    public partial class JwtTokenService : ITokenService
    {
        private const string SECURITY_ALGORITHM = SecurityAlgorithms.HmacSha256;

        private readonly JwtTokenSettings jwtSettings;

        public JwtTokenService(JwtTokenSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            this.ValidationParameters = NormalValidation();
        }

        public TokenValidationParameters ValidationParameters { get; }

        private TokenValidationParameters NormalValidation()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.SigningKey)),
                ValidIssuer = this.jwtSettings.ValidIssuer,
                ValidateIssuer = true,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        }

        private TokenValidationParameters ExpiredValidation()
        {
            var normal = NormalValidation();
            normal.ValidateLifetime = false;
            return normal;
        }

        public string GenerateAuthToken(ClaimsIdentity claims)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: this.jwtSettings.ValidIssuer,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(this.jwtSettings.TokenLifetimeSeconds)),
                    signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.SigningKey)),
                        algorithm: SECURITY_ALGORITHM
                    )
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal? ExtractPrincipalFromExpiredAuthHeader(string headerValue)
        {
            var tokenValidationParameters = ExpiredValidation();

            var tokenHandler = new JwtSecurityTokenHandler
            {
                // Set it to false, becuase it will mangle the claims types
                // kinda email to http://some.really/strange/url/email.
                MapInboundClaims = false
            };
            string token = headerValue.Substring("Bearer ".Length);
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtToken
                || !jwtToken.Header.Alg.Equals(SECURITY_ALGORITHM, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
