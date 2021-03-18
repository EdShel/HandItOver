using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.BLL.Services;
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
        private readonly MailboxAccessControlService mailboxAccessControlService;

        public MailboxAccessControlController(MailboxAccessControlService mailboxAccessControlService)
        {
            this.mailboxAccessControlService = mailboxAccessControlService;
        }

        [HttpGet("{groupId}/whitelist")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> GetWhitelist([FromRoute] string groupId)
        {
            WhitelistInfo result = await this.mailboxAccessControlService.GetMailboxWhitelist(groupId);
            return new JsonResult(result);
        }

        [HttpPost("{groupId}/whitelist")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> AddUserToWhitelist(
            [FromRoute] string groupId,
            [FromBody] UserModel user)
        {
            await this.mailboxAccessControlService.AddUserToWhitelistAsync(
                groupId: groupId,
                userEmail: user.UserEmail
            );
            return Ok();
        }

        [HttpDelete("{groupId}/whitelist/{user}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> RemoveUserFromWhitelist(
            [FromRoute] string groupId,
            [FromRoute] UserModel user)
        {
            await this.mailboxAccessControlService.RemoveUserFromWhitelistAsync(
                groupId: groupId,
                userEmail: user.UserEmail
            );
            return NoContent();
        }

        public record UserModel(string UserEmail);

        [HttpPost("{groupId}/whitelist/token")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> CreateJoinToken(
            [FromRoute] string groupId)
        {
            var token = await this.mailboxAccessControlService.CreateWhitelistJoinTokenAsync(groupId);
            return new JsonResult(token);
        }

        [HttpGet("{groupId}/whitelist/token")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> ViewJoinTokens(
            [FromRoute] string groupId)
        {
            var tokens = await this.mailboxAccessControlService.GetAllTokensAsync(groupId);
            return new JsonResult(tokens);
        }

        [HttpDelete("{groupId}/whitelist/token/{tokenId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> DeleteJoinToken(
            [FromRoute] string groupId,
            [FromRoute] string tokenId)
        {
            await this.mailboxAccessControlService.DeleteToken(tokenId);
            return NoContent();
        }

        [HttpGet("{groupId}/whitelist/join/{joinToken}")]
        [Authorize]
        public async Task<IActionResult> ViewJoinTokens(
            [FromRoute] string groupId,
            [FromRoute] string joinToken)
        {
            await this.mailboxAccessControlService.JoinWhitelistByToken(
                groupId,
                joinToken,
                this.User.FindFirstValue(AuthConstants.Claims.EMAIL));
            return Ok();
        }

    }
}
