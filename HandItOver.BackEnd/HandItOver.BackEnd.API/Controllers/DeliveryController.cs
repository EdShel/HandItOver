using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Services;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        // TODO: move this and all other records inside controllers to the Models folder
        [HttpPost("arrived")]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> DeliveryArrived([FromBody] DeliveryArrivedModel model)
        {
            string mailboxId = this.User.FindFirst(AuthConstants.Claims.MAILBOX_ID)!.Value;
            var request = new DeliveryArrivedRequest(mailboxId, model.Weight);
            await this.deliveryService.HandleDeliveryArrival(request);
            return Ok();
        }

        public record DeliveryArrivedModel(float Weight);

        [HttpPost("{deliveryId}/open")]
        [Authorize(AuthConstants.Policies.DELIVERY_ADDRESSEE_ONLY)]
        public async Task<IActionResult> OpenMailbox([FromRoute] string deliveryId)
        {
            await this.deliveryService.RequestOpeningDelivery(deliveryId);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> GetStatus([FromRoute] string mailboxId)
        {
            var result = await this.deliveryService.GetMailboxStatus(mailboxId);
            return new JsonResult(result);
        }

        [HttpPost("stolen")]
        [Authorize(Roles = AuthConstants.Roles.MAILBOX)]
        public async Task<IActionResult> DeliveryStolen()
        {
            string mailboxId = this.User.FindFirst(AuthConstants.Claims.MAILBOX_ID)!.Value;
            await this.deliveryService.HandleDeliveryDisappeared(mailboxId);
            return Ok();
        }

        [HttpPost("{deliveryId}/giveAway")]
        [Authorize(AuthConstants.Policies.DELIVERY_ADDRESSEE_ONLY)]
        public async Task<IActionResult> GiveAwayRight(
            [FromRoute] string deliveryId,
            [FromBody] DeliveryGiveAwayModel model)
        {
            await this.deliveryService.GiveAwayDeliveryRight(deliveryId, model.To);
            return Ok();
        }

        public record DeliveryGiveAwayModel(string To);
    }
}
