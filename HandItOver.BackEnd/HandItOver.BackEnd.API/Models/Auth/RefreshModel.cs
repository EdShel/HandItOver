using System.ComponentModel.DataAnnotations;

namespace HandItOver.BackEnd.API.Models.Auth
{
    public class RefreshModel
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }

    public record RevokeModel(string RefreshToken);
}
