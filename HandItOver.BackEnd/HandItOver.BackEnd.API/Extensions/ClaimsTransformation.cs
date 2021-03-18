using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Extensions
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult((ClaimsPrincipal)new CustomClaimsPrincipal(principal));
        }

        private sealed class CustomClaimsPrincipal : ClaimsPrincipal
        {
            public CustomClaimsPrincipal(ClaimsPrincipal principal)
            {
                AddIdentities(principal.Identities);
            }

            public override bool IsInRole(string role)
            {
                var userRole = this.FindFirstValue(AuthConstants.Claims.ROLE);
                return userRole == role;
            }
        }
    }
}
