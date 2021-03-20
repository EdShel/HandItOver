using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Users;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("me"), Authorize]
        public async Task<IActionResult> GetInfoAboutCurrentUserAsync()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)?.Value ?? string.Empty;
            UserAccountInfoResult user = await this.usersService.GetInfoByIdAsync(userId);
            return new JsonResult(user);
        }

        [HttpGet("byEmail"), Authorize]
        public async Task<IActionResult> GetInfoAboutAnotherUserAsync([FromQuery] string email)
        {
            UserAccountInfoResult user = await this.usersService.GetInfoByEmailAsync(email);
            return new JsonResult(user);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> SearchAsync([FromQuery, Required] string search)
        {
            var result = await this.usersService.FindByNameOrEmailAsync(search);
            return new JsonResult(result);
        }
    }
}
