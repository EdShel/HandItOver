using HandItOver.BackEnd.API.Models.Auth;
using HandItOver.BackEnd.BLL.Models.Auth;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterModel request)
        {
            var registerDTO = new RegisterRequest
            (
                Email: request.Email,
                Password: request.Password,
                Role: request.Role,
                Registerer: this.User
            );
            await this.authService.RegisterAsync(registerDTO);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var loginDTO = new LoginRequest
            (
                Email: request.Email,
                Password: request.Password
            );
            LoginResult response = await this.authService.LoginAsync(loginDTO);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshTokenAsync([FromBody] RefreshModel request)
        {
            bool hasAuthHeader = this.Request.Headers.TryGetValue("Authorization", out StringValues tokens);
            if (!hasAuthHeader || tokens.Count > 1)
            {
                return BadRequest("Provide single authentication header.");
            }

            var requestDTO = new RefreshRequest
            (
                AuthToken: tokens.First(),
                RefreshToken: request.RefreshToken
            );

            RefreshResult response = await this.authService.RefreshTokenAsync(requestDTO);

            return Ok(response);
        }

        [HttpPost("revoke"), Authorize]
        public async Task<ActionResult> RevokeTokenAsync([FromBody] RevokeModel request)
        {
            var requestDTO = new RevokeRequest(
                Id: this.User.FindFirst(AuthConstants.Claims.ID)?.Value ?? string.Empty,
                RefreshToken: request.RefreshToken
            );
            await this.authService.RevokeTokenAsync(requestDTO);

            return Ok();
        }
    }
}
