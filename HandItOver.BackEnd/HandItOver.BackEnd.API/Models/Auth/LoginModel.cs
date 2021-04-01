using System.ComponentModel.DataAnnotations;

namespace HandItOver.BackEnd.API.Models.Auth
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
