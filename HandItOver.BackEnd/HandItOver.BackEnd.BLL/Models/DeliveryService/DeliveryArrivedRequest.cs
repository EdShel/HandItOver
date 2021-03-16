using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.MailboxMessages
{
    public record DeliveryArrivedRequest(
        string MailboxId,
        float Weight
    );

    public record MailboxStatus(
        string MailboxId,
        bool IsOpen,
        string Info
    );
}
