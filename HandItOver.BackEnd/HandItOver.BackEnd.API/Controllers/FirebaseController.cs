using HandItOver.BackEnd.API.Models.Firebase;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FirebaseController : ControllerBase
    {
        private readonly FirebaseTokenSerivce firebaseTokenSerivce;

        public FirebaseController(FirebaseTokenSerivce firebaseTokenSerivce)
        {
            this.firebaseTokenSerivce = firebaseTokenSerivce;
        }

        [HttpPost("token")]
        public async Task<IActionResult> OnNewTokenGenerated([FromBody] FirebaseTokenModel model)
        {
            string userId = this.User.FindFirstValue(AuthConstants.Claims.ID);
            string tokenValue = model.FirebaseClientToken;
            await this.firebaseTokenSerivce.RegisterFirebaseToken(userId, tokenValue);
            return Ok();
        }

    }
}
