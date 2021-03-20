using HandItOver.BackEnd.BLL.Models.Delivery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<ActiveDeliveryResult>> GetActiveDeliveries(string userId);
        Task<MailboxStatus> GetMailboxStatus(string mailboxId);
        Task GiveAwayDeliveryRight(string deliveryId, string newAddresseeId);
        Task<DeliveryArrivedResult> HandleDeliveryArrival(DeliveryArrivedRequest delivery);
        Task HandleDeliveryDisappeared(string mailboxId);
        Task RequestOpeningDelivery(string deliveryId);
    }
}