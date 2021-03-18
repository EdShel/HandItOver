using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.BLL.Models.MailboxRent;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateGroup([FromBody] MailboxGroupCreateRequest model)
        {
            var request = model with
            {
                OwnerId = this.User.FindFirst(AuthConstants.Claims.ID)!.Value
            };
            var result = await this.mailboxGroupService.CreateMailboxGroupAsync(request);
            return new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] string id)
        {
            await this.mailboxGroupService.DeleteMailboxGroupAsync(id);
            return NoContent();
        }

        // TODO: replace model with dto
        // which has no id and nav properties
        [HttpPut("{id}")]
        public async Task<IActionResult> EditGroup(
            [FromRoute] string id,
            [FromBody] MailboxGroup model)
        {
            model.GroupId = id;
            await this.mailboxGroupService.EditMailboxGroup(model);
            return NoContent();
        }

        [HttpPost("{id}/mailboxes")]
        public async Task<IActionResult> AddMailbox(
            [FromRoute] string id,
            [FromBody] MailboxAddModel model)
        {
            await this.mailboxGroupService.AddMailboxToGroupAsync(
                groupId: id,
                mailboxId: model.MailboxId);
            return Ok();
        }

        public record MailboxAddModel(string MailboxId);

        [HttpDelete("{groupId}/mailboxes/{mailboxId}")]
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

        // TODO: replace with model
        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentMailbox(
            [FromRoute] string id,
            [FromBody] RentRequest model)
        {
            var request = model with
            {
                GroupId = id,
                RenterId = this.User.FindFirst(AuthConstants.Claims.ID)!.Value
            };
            var result = await this.mailboxRentService.RentMailbox(request);
            return new JsonResult(result);
        }

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
