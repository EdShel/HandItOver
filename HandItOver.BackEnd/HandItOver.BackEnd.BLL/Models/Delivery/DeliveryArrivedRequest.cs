namespace HandItOver.BackEnd.BLL.Models.Delivery
{
    public record DeliveryArrivedRequest(
        string MailboxId,
        float Weight
    );

    public record MailboxStatus(
        string MailboxId,
        bool IsOpen
    );
}
