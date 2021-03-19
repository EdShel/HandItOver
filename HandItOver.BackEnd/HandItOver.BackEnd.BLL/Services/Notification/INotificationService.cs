using HandItOver.BackEnd.BLL.Models.Notification;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Notification
{
    public interface INotificationService
    {
        public Task SendAsync(NotificationMessage message);
    }
}
