using System.ComponentModel.DataAnnotations;

namespace HandItOver.BackEnd.API.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FullName{ get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password{ get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;
    }
}
