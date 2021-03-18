﻿using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
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
        public async Task<IActionResult> GetWhitelist([FromRoute] string groupId)
        {
            WhitelistInfo result = await this.mailboxAccessControlService.GetMailboxWhitelist(groupId);
            return new JsonResult(result);
        }

        [HttpPost("{groupId}/whitelist")]
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
    }
}
