using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public abstract class ResourceAccessAuthorizationHandler<THandler>
        : AuthorizationHandler<ResourceOwnerRequirement<THandler>>
    {
        public static ResourceOwnerRequirement<THandler> GetRequirement(string idRouteParameterName)
        {
            return new ResourceOwnerRequirement<THandler>(idRouteParameterName);
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ResourceOwnerRequirement<THandler> requirement)
        {
            if (context.User.IsInRole(AuthConstants.Roles.ADMIN))
            {
                context.Succeed(requirement);
            }

            string? userId = context.User.FindFirst(AuthConstants.Claims.ID)?.Value;
            RouteValueDictionary routeValues = (context.Resource as DefaultHttpContext)!.Request.RouteValues;
            string routeIdPropertyName = requirement.IdRouteParameterName;
            if (!routeValues.TryGetValue(routeIdPropertyName, out object? id))
            {
                throw new InvalidOperationException($"Request doesn't have route value '{routeIdPropertyName}'.");
            }
            if (userId != null && await IsOwnerAsync(userId, (id as string)!))
            {
                context.Succeed(requirement);
            }
        }

        protected abstract Task<bool> IsOwnerAsync(string userId, string resourceId);
    }
}
