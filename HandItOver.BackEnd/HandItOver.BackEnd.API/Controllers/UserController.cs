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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("me") ]
        public async Task<IActionResult> GetInfoAboutCurrentUserAsync()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)?.Value ?? string.Empty;
            UserAccountInfoResult user = await this.usersService.GetInfoByIdAsync(userId);
            return new JsonResult(user);
        }

        [HttpGet("byId/{userId}")]
        public async Task<IActionResult> GetInfoAboutUserByIdAsync([FromRoute] string userId)
        {
            UserAccountInfoResult user = await this.usersService.GetInfoByIdAsync(userId);
            return new JsonResult(user);
        }

        [HttpGet("byEmail")]
        public async Task<IActionResult> GetInfoAboutAnotherUserAsync([FromQuery] string email)
        {
            UserAccountInfoResult user = await this.usersService.GetInfoByEmailAsync(email);
            return new JsonResult(user);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery, Required] string search)
        {
            var result = await this.usersService.FindByNameOrEmailAsync(search);
            return new JsonResult(result);
        }


        [HttpGet("paginated")]
        public async Task<IActionResult> SearchPaginatedAsync(
            [FromQuery, Required] int pageIndex,
            [FromQuery, Required] int pageSize,
            [FromQuery] string? search
        )
        {
            var request = new UsersPaginatedRequest(
                SearchQuery: search,
                PageIndex: pageIndex,
                PageSize: pageSize
            );
            var result = await this.usersService.GetUsersPaginatedAsync(request);
            return new JsonResult(result);
        }
    }
}
