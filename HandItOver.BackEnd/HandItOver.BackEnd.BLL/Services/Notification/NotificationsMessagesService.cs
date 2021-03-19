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

        public NotificationMessage DeliveryArrived(string userId) => GetNotificationMessage(userId, "deliveryArrived");

        public NotificationMessage DeliveryExpiration(string userId) => GetNotificationMessage(userId, "deliveryExpiration");

        public NotificationMessage DeliveryTheft(string userId) => GetNotificationMessage(userId, "deliveryTheft");

        private NotificationMessage GetNotificationMessage(string userId, string notificationName)
        {
            var message = this.configuration.GetSection(notificationName).Get<Message>();
            return new NotificationMessage(userId, message.Title, message.Body);
        }

        private class Message
        {
            public string Title { get; set; } = null!;
            public string Body { get; set; } = null!;
        }

    }
}
