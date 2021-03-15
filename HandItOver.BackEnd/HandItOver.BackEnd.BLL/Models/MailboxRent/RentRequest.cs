using HandItOver.BackEnd.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.MailboxRent
{
    public record RentRequest(
        string GroupId,
        string RenterId,
        MailboxSize PackageSize,
        DateTime RentFrom,
        DateTime RentUntil
    );

    public record RentResult(
        string RentId,
        string MailboxId,
        MailboxSize MailboxSize,
        DateTime From,
        DateTime Until
    );
}
