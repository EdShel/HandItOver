using HandItOver.BackEnd.API.Models.Mailbox;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Mailbox;
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
    public class MailboxController : ControllerBase
    {
        private readonly IMailboxService mailboxService;

        public MailboxController(IMailboxService mailboxService)
        {
            this.mailboxService = mailboxService;
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> AuthorizeAsync([FromBody] MailboxAuthModel authModel)
        {
            MailboxAuthRequest request = new MailboxAuthRequest(
                OwnerId: this.User.FindFirst(AuthConstants.Claims.ID)!.Value,
                Size: authModel.Size,
                PhysicalId: authModel.PhysicalId,
                Address: authModel.Address
            );
            MailboxAuthResult result = await this.mailboxService.AuthorizeMailboxAsync(request);
            return new JsonResult(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMailboxesAsync()
        {
            string userId = this.User.FindFirstValue(AuthConstants.Claims.ID);
            var result = await this.mailboxService.GetOwnedMailboxesAsync(userId);
            return new JsonResult(result);
        }

        [HttpGet("{mailboxId}")]
        public async Task<IActionResult> GetMailboxInfoAsync([FromRoute] string mailboxId)
        {
            var result = await this.mailboxService.GetMailboxAsync(mailboxId);
            return new JsonResult(result);
        }


        [HttpPatch("{mailboxId}")]
        [Authorize(Policy = AuthConstants.Policies.MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> EditMailboxAsync(
            [FromRoute] string mailboxId, 
            [FromBody] MailboxEditRequest model)
        {
            var result = await this.mailboxService.EditMailboxAsync(mailboxId, model);
            return new JsonResult(result);
        }

        [HttpGet]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> GetInfoAsync()
        {
            var mailboxId = this.User.FindFirstValue(AuthConstants.Claims.MAILBOX_ID);
            var result = await this.mailboxService.GetMailboxAsync(mailboxId);
            return new JsonResult(result);
        }
    }
}
