using HandItOver.BackEnd.BLL.Models.Notification;
using Microsoft.Extensions.Configuration;

namespace HandItOver.BackEnd.BLL.Services.Notification
{
    public class NotificationsMessagesService
    {
        private readonly IConfiguration configuration;

        public NotificationsMessagesService(IConfiguration configuration)
        {
            this.configuration = configuration.GetSection("Notifications");
        }

        public NotificationMessage DeliveryArrived => GetNotificationMessage("deliveryArrived");

        public NotificationMessage DeliveryExpiration => GetNotificationMessage("deliveryExpiration");

        public NotificationMessage DeliveryTheft => GetNotificationMessage("deliveryTheft");

        private NotificationMessage GetNotificationMessage(string notificationName)
        {
            return this.configuration.GetValue<NotificationMessage>(notificationName);
        }
    }
}
