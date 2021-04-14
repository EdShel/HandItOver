using HandItOver.BackEnd.BLL.Models.Notification;
using HandItOver.BackEnd.Infrastructure.Models;

namespace HandItOver.BackEnd.BLL.Services.Notification
{
    public class NotificationsMessagesService
    {
        public NotificationMessage DeliveryArrived(string userId) => 
            new NotificationMessage(userId, NotificationKeys.DELIVERY_ARRIVED);

        public NotificationMessage DeliveryExpiration(string userId) => 
            new NotificationMessage(userId, NotificationKeys.DELIVERY_EXPIRED);

        public NotificationMessage DeliveryTheft(string userId) => 
            new NotificationMessage(userId, NotificationKeys.DELIVERY_STOLEN);
    }
}
