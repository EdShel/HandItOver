using HandItOver.BackEnd.API.Models.MailboxGroup;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MailboxGroupController : ControllerBase
    {
        private readonly IMailboxGroupService mailboxGroupService;

        private readonly IMailboxRentService mailboxRentService;

        public MailboxGroupController(
            IMailboxGroupService mailboxGroupService,
            IMailboxRentService mailboxRentService)
        {
            this.mailboxGroupService = mailboxGroupService;
            this.mailboxRentService = mailboxRentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupAsync([FromBody] MailboxGroupCreateModel model)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var result = await this.mailboxGroupService.GetMailboxGroupByIdAsync(id);
            return new JsonResult(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetAllMyGroupsAsync()
        {
            string id = this.User.FindFirstValue(AuthConstants.Claims.ID);
            var result = await this.mailboxGroupService.GetMailboxGroupByOwnerAsync(id);
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> FindBySearchRequestAsync([FromQuery] string search)
        {
            var request = new MailboxGroupSearchRequest(search);
            var result = await this.mailboxGroupService.FindMailboxesAsync(request);
            return new JsonResult(result);
        }

        [HttpDelete("{groupId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> DeleteGroupAsync([FromRoute] string groupId)
        {
            await this.mailboxGroupService.DeleteMailboxGroupAsync(groupId);
            return NoContent();
        }

        [HttpPut("{groupId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> EditGroupAsync(
            [FromRoute] string groupId,
            [FromBody] MailboxGroupEditModel model)
        {
            var request = new MailboxGroupEditRequest(
                groupId,
                model.Name,
                model.WhitelistOnly,
                model.MaxRentTime
            );
            await this.mailboxGroupService.EditMailboxGroupAsync(request);
            return Ok();
        }

        [HttpPost("{groupId}/mailboxes")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> AddMailboxAsync(
            [FromRoute] string groupId,
            [FromBody] MailboxAddModel model)
        {
            await this.mailboxGroupService.AddMailboxToGroupAsync(
                groupId: groupId,
                mailboxId: model.MailboxId
            );
            return Ok();
        }

        [HttpDelete("{groupId}/mailboxes/{mailboxId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> RemoveMailboxFromGroupAsync(
            [FromRoute] string groupId,
            [FromRoute] string mailboxId)
        {
            await this.mailboxGroupService.RemoveMailboxFromGroupAsync(groupId, mailboxId);
            return NoContent();
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> ViewStatsAsync([FromRoute] string id)
        {
            var result = await this.mailboxGroupService.GetStatsAsync(id);
            return new JsonResult(result);
        }

        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentMailboxAsync(
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
            var result = await this.mailboxRentService.RentMailboxAsync(request);
            return new JsonResult(result);
        }

        [HttpGet("{id}/rentTime")]
        public async Task<IActionResult> GetVacantRentTimeAsync(
            [FromRoute] string id,
            [FromQuery] RentCheckModel model)
        {
            var request = new RentTimeCheckRequest(
                GroupId: id,
                RenterId: this.User.FindFirstValue(AuthConstants.Claims.ID),
                PackageSize: model.PackageSize
            );
            var result = await this.mailboxRentService.FindVacantIntervalsToRent(request);
            return new JsonResult(result);
        }

        [HttpDelete("rent/{rentId}")]
        [Authorize(AuthConstants.Policies.RENTER_OR_OWNER_ONLY)]
        public async Task<IActionResult> CancelRentAsync([FromRoute] string rentId)
        {
            await this.mailboxRentService.CancelRentAsync(rentId);
            return NoContent();
        }

        [HttpGet("rent/{rentId}")]
        [Authorize(AuthConstants.Policies.RENTER_OR_OWNER_ONLY)]
        public async Task<IActionResult> ViewRentAsync([FromRoute] string rentId)
        {
            var result = await this.mailboxRentService.GetRentAsync(rentId);
            return new JsonResult(result);
        }

        [HttpGet("{groupId}/rent")]
        [Authorize(AuthConstants.Policies.MAILBOX_GROUP_OWNER_ONLY)]
        public async Task<IActionResult> ViewRentsAsync([FromRoute] string groupId)
        {
            var result = await this.mailboxRentService.GetRentsForMailboxGroupAsync(groupId);
            return new JsonResult(result);
        }

        [HttpGet("rent/user/my")]
        public async Task<IActionResult> ViewRentsAsync()
        {
            string userId = this.User.FindFirst(AuthConstants.Claims.ID)!.Value;
            var result = await this.mailboxRentService.GetRentsOfUserAsync(userId);
            return new JsonResult(result);
        }

        [HttpGet("rent/user/{userId}")]
        [Authorize(AuthConstants.Policies.USER_HIMSELF_ONLY)]
        public async Task<IActionResult> ViewRentsOfUserAsync([FromRoute] string userId)
        {
            var result = await this.mailboxRentService.GetRentsOfUserAsync(userId);
            return new JsonResult(result);
        }
    }
}
