using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UsersService usersService;

        public UserController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("me"), Authorize]
        public async Task<IActionResult> GetInfoAboutCurrentUserAsync()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)?.Value ?? string.Empty;
            UserInfoResult user = await this.usersService.GetInfoByIdAsync(userId);
            return new JsonResult(user);
        }

        [HttpGet("user"), Authorize]
        public async Task<IActionResult> GetInfoAboutAnotherUserAsync(string email)
        {
            UserInfoResult user = await this.usersService.GetInfoByEmailAsync(email);
            return new JsonResult(user);
        }
    }


    [ApiController]
    [Route("[controller]")]
    public class MailboxController : ControllerBase
    {
        [HttpPost("auth")]
        public async Task<IActionResult> AuthorizeMailbox()
        {
            return new JsonResult(new
            {
                Token = "",
                RefreshToken = ""
            });
        }
    }
}
