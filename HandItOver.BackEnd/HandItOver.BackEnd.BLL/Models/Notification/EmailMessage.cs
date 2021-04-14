namespace HandItOver.BackEnd.BLL.Models.Notification
{
    public record EmailMessage(string ReceiverAddress, string Title, string Body);
}
