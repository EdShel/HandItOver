namespace HandItOver.BackEnd.BLL.Models.Auth
{
    public record LoginResult(string Token, string RefreshToken, string Email);

    public class LoginRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
