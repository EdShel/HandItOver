using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost("{id}/open")]
        public async Task<IActionResult> OpenMailbox([FromRoute] string id)
        {
            await this.mailboxService.RequestOpening(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatus([FromRoute] string id)
        {
            var result = await this.mailboxService.GetMailboxStatus(id);
            return new JsonResult(result);
        }
    }
}
