using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("mailboxGroup")]
    [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
    public class MailboxAccessControlController : ControllerBase
    {
        private readonly MailboxAccessControlService mailboxAccessControlService;

        public MailboxAccessControlController(MailboxAccessControlService mailboxAccessControlService)
        {
            this.mailboxAccessControlService = mailboxAccessControlService;
        }

        [HttpGet("{groupId}/whitelist")]
        public async Task<IActionResult> GetWhitelist([FromRoute] MailboxGroupModel groupModel)
        {
            WhitelistInfo result = await this.mailboxAccessControlService.GetMailboxWhitelist(groupModel.GroupId);
            return new JsonResult(result);
        }

        [HttpPost("{groupId}/whitelist")]
        public async Task<IActionResult> AddUserToWhitelist(
            [FromRoute] MailboxGroupModel groupModel,
            [FromBody] UserModel user)
        {
            await this.mailboxAccessControlService.AddUserToWhitelistAsync(
                groupModel.GroupId,
                user.UserEmail);
            return Ok();
        }

        [HttpDelete("{groupId}/whitelist/{user}")]
        public async Task<IActionResult> RemoveUserFromWhitelist(
            [FromRoute] MailboxGroupModel groupModel,
            [FromRoute] UserModel user)
        {
            await this.mailboxAccessControlService.RemoveUserFromWhitelistAsync(
                groupModel.GroupId,
                user.UserEmail);
            return NoContent();
        }

        public record MailboxGroupModel(string GroupId);

        public record UserModel(string UserEmail);
    }
}
