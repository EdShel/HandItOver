using HandItOver.BackEnd.DAL.Entities;
using System;

namespace HandItOver.BackEnd.API.Models.MailboxGroup
{
    public record MailboxGroupCreateModel(
        string Name,
        string FirstMailboxId,
        bool WhitelistOnly,
        TimeSpan? MaxRentTime
    );

    public record MailboxGroupEditModel(
        string Name,
        bool WhitelistOnly,
        TimeSpan? MaxRentTime
    );

    public record MailboxAddModel(string MailboxId);

    public record RentModel(
        MailboxSize PackageSize,
        DateTime RentFrom,
        DateTime RentUntil
    );
}
