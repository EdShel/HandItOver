using HandItOver.BackEnd.API.Models.Firebase;
using HandItOver.BackEnd.BLL.Interfaces;
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
        private readonly IFirebaseTokenSerivce firebaseTokenSerivce;

        public FirebaseController(IFirebaseTokenSerivce firebaseTokenSerivce)
        {
            this.firebaseTokenSerivce = firebaseTokenSerivce;
        }

        [HttpPost("token")]
        public async Task<IActionResult> OnNewTokenGeneratedAsync([FromBody] FirebaseTokenModel model)
        {
            string userId = this.User.FindFirstValue(AuthConstants.Claims.ID);
            string tokenValue = model.FirebaseClientToken;
            await this.firebaseTokenSerivce.RegisterFirebaseTokenAsync(userId, tokenValue);
            return Ok();
        }

    }
}
