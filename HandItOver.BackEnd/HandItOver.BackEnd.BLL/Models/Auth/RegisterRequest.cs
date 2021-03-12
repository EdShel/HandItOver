namespace HandItOver.BackEnd.BLL.Models.Auth
{
    public record RegisterRequest(
        string Email,
        string Password,
        string Role
    );

    public record RefreshResponse(
        string AuthToken,
        string RefreshToken
    );

    public record RefreshRequest(
        string AuthToken,
        string RefreshToken
    );

    public record RevokeRequest(
        string Id,
        string RefreshToken    
    );
}
