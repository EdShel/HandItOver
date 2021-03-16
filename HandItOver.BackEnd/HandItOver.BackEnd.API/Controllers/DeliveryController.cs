using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        // TODO: replace with model
        [HttpPost("arrived")]
        public async Task<IActionResult> DeliveryArrived([FromBody] DeliveryArrivedRequest model)
        {
            await this.deliveryService.HandleDeliveryArrival(model);
            return Ok();
        }

        [HttpPost("{id}/stolen")]
        public async Task<IActionResult> DeliveryStolen([FromRoute] string id)
        {
            await this.deliveryService.HandleDeliveryDisappeared(id);
            return Ok();
        }

        [HttpPost("{id}/giveAway")]
        public async Task<IActionResult> GiveAwayRight(
            [FromRoute] string id,
            [FromBody] DeliveryGiveAwayModel model)
        {
            await this.deliveryService.GiveAwayDeliveryRight(id, model.To);
            return Ok();
        }

        public record DeliveryGiveAwayModel(string To);
    }
}
