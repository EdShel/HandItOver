using HandItOver.BackEnd.DAL.Entities;
using System;

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
        RentedMailboxResult Mailbox,
        DateTime From,
        DateTime Until,
        RenterResult Renter
    );

    public record RentedMailboxResult(
        string Id,
        string Address,
        MailboxSize Size
    );

    public record RenterResult(
        string Id,
        string FullName,
        string Email
    );

    public record RentTimeCheckRequest(
        string GroupId,
        string RenterId,
        MailboxSize PackageSize
    );
}
