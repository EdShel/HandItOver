using HandItOver.BackEnd.DAL.Entities;
using System;
using System.Collections.Generic;

namespace HandItOver.BackEnd.BLL.Models.MailboxGroup
{
    public class MailboxGroupRequest
    {
        public string GroupId { get; } = null!;
    }
    public record MailboxGroupViewResult(
        string GroupId,
        string Name,
        bool WhitelistOnly,
        TimeSpan? MaxRentTime,
        IEnumerable<MailboxViewResult> Mailboxes
    );

    public record MailboxViewResult(
        string Id,
        MailboxSize Size,
        string Address
    );

    public record MailboxGroupCreateRequest(
        string OwnerId,
        string Name,
        string FirstMailboxId,
        bool WhitelistOnly,
        TimeSpan? MaxRentTime
    );

    public record MailboxGroupCreatedResult(
        string GroupId
    );

    public record MailboxGroupStats(
        string GroupId,
        string Name,
        IEnumerable<MailboxStats> Mailboxes
    );

    public record MailboxStats(
        string MailboxId,
        MailboxSize Size,
        DeliveryStats? Delivery,
        IEnumerable<RentStats> Rents
    );

    public record DeliveryStats(
        DateTime Arrived,
        DateTime? Taken,
        DateTime? PredictedTakingTime,
        float Weight
    );

    public record RentStats(
        string RenterName,
        DateTime From,
        DateTime Until
    );

    public record MailboxGroupSearchRequest(
        string SearchQuery
    );

    public record MailboxGroupSearchResult(
        string GroupId,
        string Owner,
        string Name,
        IEnumerable<string> Addresses
    );

    public record MailboxGroupEditRequest(
        string GroupId,
        string Name,
        bool WhitelistOnly,
        TimeSpan? MaxRentTime
    );
}
