using System;

namespace HandItOver.BackEnd.BLL.Models.Delivery
{
    public record DeliveryArrivedRequest(
        string MailboxId,
        float Weight
    );

    public record DeliveryArrivedResult(
        string DeliveryId
    );

    public record MailboxStatus(
        string MailboxId,
        bool IsOpen,
        string? Renter
    );

    public record ActiveDeliveryResult(
        string Id,
        float Weight,
        string MailboxId,
        DateTime Arrived,
        DateTime PredictedTakingTime,
        DateTime? TerminalTime
    );


    public record DeliveryResult(
        string Id,
        float Weight,
        string MailboxId,
        DateTime Arrived,
        DateTime? Taken,
        DateTime PredictedTakingTime,
        DateTime? TerminalTime
    );
}
