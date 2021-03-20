using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MailboxController : ControllerBase
    {
        private readonly MailboxService mailboxService;

        public MailboxController(MailboxService mailboxService)
        {
            this.mailboxService = mailboxService;
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] MailboxAuthModel authModel)
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

        public record MailboxAuthModel(
            string PhysicalId,
            MailboxSize Size,
            string Address
        );

        [HttpGet("my")]
        public async Task<IActionResult> GetMailboxes()
        {
            string userId = this.User.FindFirstValue(AuthConstants.Claims.ID);
            var result = await this.mailboxService.GetOwnedMailboxes(userId);
            return new JsonResult(result);
        }

        [HttpGet("{mailboxId}")]
        public async Task<IActionResult> GetMailboxInfo([FromRoute] string mailboxId)
        {
            var result = await this.mailboxService.GetMailbox(mailboxId);
            return new JsonResult(result);
        }

        [HttpGet]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> GetInfo()
        {
            var mailboxId = this.User.FindFirstValue(AuthConstants.Claims.MAILBOX_ID);
            var result = await this.mailboxService.GetMailbox(mailboxId);
            return new JsonResult(result);
        }
    }
}
