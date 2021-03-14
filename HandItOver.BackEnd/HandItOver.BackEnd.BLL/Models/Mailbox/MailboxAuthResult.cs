using HandItOver.BackEnd.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.Mailbox
{
    public record MailboxAuthRequest(
        string OwnerId,
        MailboxSize Size,
        string PhysicalId
    );

    public record MailboxAuthResult(
        string AuthToken,
        string RefreshToken
    );
}
