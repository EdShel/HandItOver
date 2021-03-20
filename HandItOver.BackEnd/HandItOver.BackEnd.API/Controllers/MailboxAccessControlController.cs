using HandItOver.BackEnd.API.Models.MailboxAccessControl;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("mailboxGroup")]
    [Authorize]
    public class MailboxAccessControlController : ControllerBase
    {
        private readonly IMailboxAccessControlService mailboxAccessControlService;

        public MailboxAccessControlController(IMailboxAccessControlService mailboxAccessControlService)
        {
            this.mailboxAccessControlService = mailboxAccessControlService;
        }

        [HttpGet("{groupId}/whitelist")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> GetWhitelistAsync([FromRoute] string groupId)
        {
            WhitelistInfo result = await this.mailboxAccessControlService.GetMailboxWhitelistAsync(groupId);
            return new JsonResult(result);
        }

        [HttpPost("{groupId}/whitelist")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> AddUserToWhitelistAsync(
            [FromRoute] string groupId,
            [FromBody] UserModel user)
        {
            await this.mailboxAccessControlService.AddUserToWhitelistAsync(
                groupId: groupId,
                userEmail: user.UserEmail
            );
            return Ok();
        }

        [HttpDelete("{groupId}/whitelist/{email}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> RemoveUserFromWhitelistAsync(
            [FromRoute] string groupId,
            [FromRoute] string email)
        {
            await this.mailboxAccessControlService.RemoveUserFromWhitelistAsync(
                groupId: groupId,
                userEmail: email
            );
            return NoContent();
        }

        [HttpPost("{groupId}/whitelist/token")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> CreateJoinTokenAsync(
            [FromRoute] string groupId)
        {
            var token = await this.mailboxAccessControlService.CreateWhitelistJoinTokenAsync(groupId);
            return new JsonResult(token);
        }

        [HttpGet("{groupId}/whitelist/token")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> ViewJoinTokensAsync(
            [FromRoute] string groupId)
        {
            var tokens = await this.mailboxAccessControlService.GetAllTokensAsync(groupId);
            return new JsonResult(tokens);
        }

        [HttpDelete("{groupId}/whitelist/token/{tokenId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> DeleteJoinTokenAsync(
            [FromRoute] string groupId,
            [FromRoute] string tokenId)
        {
            await this.mailboxAccessControlService.DeleteTokenAsync(groupId, tokenId);
            return NoContent();
        }

        [HttpPost("{groupId}/whitelist/join")]
        [Authorize]
        public async Task<IActionResult> ViewJoinTokensAsync(
            [FromRoute] string groupId,
            [FromQuery] string joinToken)
        {
            await this.mailboxAccessControlService.JoinWhitelistByTokenAsync(
                groupId,
                joinToken,
                this.User.FindFirstValue(AuthConstants.Claims.EMAIL)
            );
            return Ok();
        }

    }
}
