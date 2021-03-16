﻿using HandItOver.BackEnd.DAL.Entities;

namespace HandItOver.BackEnd.BLL.Models.Mailbox
{
    public record MailboxAuthRequest(
        string OwnerId,
        MailboxSize Size,
        string PhysicalId,
        string Address
    );

    public record MailboxAuthResult(
        string MailboxId,
        string AuthToken,
        string RefreshToken
    );
}
