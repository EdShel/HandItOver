namespace HandItOver.BackEnd.BLL.Models.Auth
{
    public record LoginResult(string Token, string RefreshToken, string Email);

    public record LoginRequest(string Email, string Password);
}
