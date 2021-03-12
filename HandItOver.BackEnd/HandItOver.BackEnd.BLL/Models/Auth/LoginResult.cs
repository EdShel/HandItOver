using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.Auth
{
    public class LoginResult
    {
        public string Token { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
