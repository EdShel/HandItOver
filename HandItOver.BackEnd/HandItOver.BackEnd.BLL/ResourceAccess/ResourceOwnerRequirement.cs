using Microsoft.AspNetCore.Authorization;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public class ResourceOwnerRequirement<T> : IAuthorizationRequirement
    {
        public ResourceOwnerRequirement(string idRouteParameterName)
        {
            this.IdRouteParameterName = idRouteParameterName;
        }

        public string IdRouteParameterName { get; private set; }
    }

}
