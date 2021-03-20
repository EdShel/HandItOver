using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MailboxGroupController : ControllerBase
    {
        private readonly MailboxGroupService mailboxGroupService;

        private readonly MailboxRentService mailboxRentService;

        public MailboxGroupController(
            MailboxGroupService mailboxGroupService,
            MailboxRentService mailboxRentService)
        {
            this.mailboxGroupService = mailboxGroupService;
            this.mailboxRentService = mailboxRentService;
        }

        // TODO: replace model
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] MailboxGroupCreateModel model)
        {
            var request = new MailboxGroupCreateRequest(
                this.User.FindFirst(AuthConstants.Claims.ID)!.Value,
                model.Name,
                model.FirstMailboxId,
                model.WhitelistOnly,
                model.MaxRentTime
            );
            var result = await this.mailboxGroupService.CreateMailboxGroupAsync(request);
            return new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }

        public record MailboxGroupCreateModel(
            string Name,
            string FirstMailboxId,
            bool WhitelistOnly,
            TimeSpan? MaxRentTime
        );

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await this.mailboxGroupService.GetMailboxGroupById(id);
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> FindBySearchRequest([FromQuery] string search)
        {
            var request = new MailboxGroupSearchRequest(search);
            var result = await this.mailboxGroupService.FindMailboxes(request);
            return new JsonResult(result);
        }

        [HttpDelete("{groupId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> DeleteGroup([FromRoute] string groupId)
        {
            await this.mailboxGroupService.DeleteMailboxGroupAsync(groupId);
            return NoContent();
        }

        [HttpPut("{groupId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> EditGroup(
            [FromRoute] string groupId,
            [FromBody] MailboxGroupEditModel model)
        {
            var request = new MailboxGroupEditRequest(
                groupId,
                model.Name,
                model.WhitelistOnly,
                model.MaxRentTime
            );
            await this.mailboxGroupService.EditMailboxGroup(request);
            return Ok();
        }

        public record MailboxGroupEditModel(
            string Name,
            bool WhitelistOnly,
            TimeSpan? MaxRentTime
        );

        [HttpPost("{groupId}/mailboxes")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> AddMailbox(
            [FromRoute] string groupId,
            [FromBody] MailboxAddModel model)
        {
            await this.mailboxGroupService.AddMailboxToGroupAsync(
                groupId: groupId,
                mailboxId: model.MailboxId);
            return Ok();
        }

        public record MailboxAddModel(string MailboxId);

        [HttpDelete("{groupId}/mailboxes/{mailboxId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> RemoveMailboxFromGroup(
            [FromRoute] string groupId,
            [FromRoute] string mailboxId)
        {
            await this.mailboxGroupService.RemoveMailboxFromGroupAsync(groupId, mailboxId);
            return NoContent();
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> ViewStats([FromRoute] string id)
        {
            var result = await this.mailboxGroupService.GetStats(id);
            return new JsonResult(result);
        }

        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentMailbox(
            [FromRoute] string id,
            [FromBody] RentModel model)
        {
            var request = new RentRequest(
                GroupId: id,
                RenterId: this.User.FindFirst(AuthConstants.Claims.ID)!.Value,
                PackageSize: model.PackageSize,
                RentFrom: model.RentFrom,
                RentUntil: model.RentUntil
            );
            var result = await this.mailboxRentService.RentMailbox(request);
            return new JsonResult(result);
        }

        public record RentModel(
            MailboxSize PackageSize,
            DateTime RentFrom,
            DateTime RentUntil
        );

        [HttpDelete("rent/{rentId}")]
        [Authorize(AuthConstants.Policies.RENTER_OR_OWNER_ONLY)]
        public async Task<IActionResult> CancelRent([FromRoute] string rentId)
        {
            await this.mailboxRentService.CancelRent(rentId);
            return NoContent();
        }

        [HttpGet("rent/{rentId}")]
        [Authorize(AuthConstants.Policies.RENTER_OR_OWNER_ONLY)]
        public async Task<IActionResult> ViewRent([FromRoute] string rentId)
        {
            var result = await this.mailboxRentService.GetRent(rentId);
            return new JsonResult(result);
        }

        [HttpGet("rent")]
        public async Task<IActionResult> ViewRents()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)!.Value;
            var result = await this.mailboxRentService.GetRentsOfUserAsync(userId);
            return new JsonResult(result);
        }
    }
}
