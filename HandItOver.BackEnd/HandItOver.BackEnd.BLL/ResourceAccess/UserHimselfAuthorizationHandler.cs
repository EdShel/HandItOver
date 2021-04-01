using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public sealed class UserHimselfAuthorizationHandler : ResourceAccessAuthorizationHandler<UserHimselfAuthorizationHandler>
    {
        protected override Task<bool> IsOwnerAsync(string userId, string ownerId)
        {
            return Task.FromResult(userId == ownerId);
        }
    }
}
