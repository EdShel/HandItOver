using HandItOver.BackEnd.BLL.Models.Delivery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<ActiveDeliveryResult>> GetActiveDeliveriesAsync(string userId);
        Task<MailboxStatus> GetMailboxStatusAsync(string mailboxId);
        Task GiveAwayDeliveryRightAsync(string deliveryId, string newAddresseeId);
        Task<DeliveryArrivedResult> HandleDeliveryArrivalAsync(DeliveryArrivedRequest delivery);
        Task HandleDeliveryDisappearedAsync(string mailboxId);
        Task RequestOpeningDeliveryAsync(string deliveryId);
    }
}