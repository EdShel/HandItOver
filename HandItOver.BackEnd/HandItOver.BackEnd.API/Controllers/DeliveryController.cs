using HandItOver.BackEnd.API.Models.Delivery;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        [HttpPost("arrived")]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> DeliveryArrivedAsync([FromBody] DeliveryArrivedModel model)
        {
            string mailboxId = this.User.FindFirstValue(AuthConstants.Claims.MAILBOX_ID);
            var request = new DeliveryArrivedRequest(mailboxId, model.Weight);
            await this.deliveryService.HandleDeliveryArrivalAsync(request);
            return Ok();
        }

        [HttpPost("{deliveryId}/open")]
        [Authorize(AuthConstants.Policies.DELIVERY_ADDRESSEE_OR_MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> OpenMailboxAsync([FromRoute] string deliveryId)
        {
            await this.deliveryService.RequestOpeningDeliveryAsync(deliveryId);
            return Ok();
        }

        [HttpGet("{deliveryId}")]
        [Authorize(AuthConstants.Policies.DELIVERY_ADDRESSEE_OR_MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> GetInfoAboutDelivery([FromRoute] string deliveryId)
        {
            var delivery = await this.deliveryService.GetDeliveryByIdAsync(deliveryId);
            return Ok(delivery);
        }

        [HttpGet("mailboxStatus")]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> GetStatusAsync()
        {
            string mailboxId = this.User.FindFirstValue(AuthConstants.Claims.MAILBOX_ID);
            var result = await this.deliveryService.GetMailboxStatusAsync(mailboxId);
            return new JsonResult(result);
        }

        [HttpPost("stolen")]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> DeliveryStolenAsync()
        {
            string mailboxId = this.User.FindFirstValue(AuthConstants.Claims.MAILBOX_ID);
            await this.deliveryService.HandleDeliveryDisappearedAsync(mailboxId);
            return Ok();
        }

        [HttpPost("{deliveryId}/giveAway")]
        [Authorize(AuthConstants.Policies.DELIVERY_ADDRESSEE_OR_MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> GiveAwayRightAsync(
            [FromRoute] string deliveryId,
            [FromBody] DeliveryGiveAwayModel model)
        {
            await this.deliveryService.GiveAwayDeliveryRightAsync(deliveryId, model.NewAddresseeId);
            return Ok();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveDeliveriesAsync()
        {
            string userId = this.User.FindFirstValue(AuthConstants.Claims.ID);
            var result = await this.deliveryService.GetActiveDeliveriesAsync(userId);
            return new JsonResult(result);
        }

        [HttpGet("active/{userId}")]
        [Authorize(Roles = AuthConstants.Roles.ADMIN)]
        public async Task<IActionResult> GetActiveDeliveriesOfUserAsync([FromRoute] string userId)
        {
            var result = await this.deliveryService.GetActiveDeliveriesAsync(userId);
            return new JsonResult(result);
        }

        [HttpGet("recent/{mailboxId}")]
        [Authorize(AuthConstants.Policies.MAILBOX_OWNER_ONLY)]
        public async Task<IActionResult> GetRecentDeliveriesAsync(
            [FromRoute] string mailboxId,
            [FromQuery] int count)
        {
            var result = await this.deliveryService.GetRecentDeliveriesAsync(mailboxId, count);
            return new JsonResult(result);
        }
    }
}
