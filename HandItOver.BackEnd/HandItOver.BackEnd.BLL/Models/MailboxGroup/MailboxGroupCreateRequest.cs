using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.MailboxGroup
{
    public interface IMailboxGroupRequest
    {
        string GroupId { get; }
    }

    public record MailboxGroupCreateRequest(
        string OwnerId,
        string Name,
        string FirstMailboxId,
        bool WhitelistOnly
    );

    public record MailboxGroupCreatedResult(
        string GroupId
    );
}
