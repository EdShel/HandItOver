using System.ComponentModel.DataAnnotations;

namespace HandItOver.BackEnd.API.Models.Auth
{
    public class RefreshModel
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }

    public record RegisterModel(
        string Email,
        string FullName,
        string Password,
        string Role
    );

    public record LoginModel(
        string Email,
        string Password
    );

    public record RevokeModel(string RefreshToken);
}
