using System.Collections.Generic;

namespace HandItOver.BackEnd.BLL.Models.Notification
{
    public record NotificationMessage(
        string ReceiverAddress,
        string MessageKey,
        IDictionary<string, string>? Data = null
    );
}
