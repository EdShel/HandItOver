using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.BLL.ResourceAccess;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
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


        [HttpPost("{mailboxId}/open")]
        public async Task<IActionResult> OpenMailbox([FromRoute] MailboxRequest model)
        {
            await this.mailboxService.RequestOpening(model.MailboxId);
            return Ok();
        }

        [HttpGet("{mailboxId}")]
        [Authorize(Policy = AuthConstants.Policies.MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> GetStatus([FromRoute] MailboxRequest model)
        {
            var result = await this.mailboxService.GetMailboxStatus(model.MailboxId);
            return new JsonResult(result);
        }


        public class MailboxRequest
        {
            [Required]
            [FromRoute]
            public string MailboxId { get; set; } = null!;
        }

        [HttpGet]
        public async Task<IActionResult> GetMailboxes()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)!.Value;
            IEnumerable<Mailbox> result = await this.mailboxService.GetOwnedMailboxes(userId);
            return new JsonResult(result);
        }
    }
}
