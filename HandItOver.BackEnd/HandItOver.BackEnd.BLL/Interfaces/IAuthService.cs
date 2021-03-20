using HandItOver.BackEnd.BLL.Models.Auth;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginRequest loginRequest);
        Task<RefreshResult> RefreshTokenAsync(RefreshRequest refreshRequest);
        Task RegisterAsync(RegisterRequest request);
        Task RevokeTokenAsync(RevokeRequest revokeRequest);
    }
}