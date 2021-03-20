using HandItOver.BackEnd.DAL.Entities;

namespace HandItOver.BackEnd.API.Models.Mailbox
{
    public record MailboxAuthModel(
        string PhysicalId,
        MailboxSize Size,
        string Address
    );
}
