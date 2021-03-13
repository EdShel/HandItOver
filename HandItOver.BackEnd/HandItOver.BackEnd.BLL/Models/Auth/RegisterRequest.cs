using System.Security.Principal;

namespace HandItOver.BackEnd.BLL.Models.Auth
{
    public record RegisterRequest(
        string Email,
        string Password,
        string Role,
        IPrincipal Registerer
    );

    public record RefreshResult(
        string AuthToken,
        string RefreshToken
    );

    public record RefreshRequest(
        string AuthHeaderValue,
        string RefreshToken
    );

    public record RevokeRequest(
        string Id,
        string RefreshToken    
    );
}
