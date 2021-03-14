﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Models.MailboxAccessControl
{
    public record WhitelistInfo(
        string MailboxGroupId,
        IEnumerable<WhitelistEntry> Entries
    );
    public record WhitelistEntry(
        string Email,
        string Id
    );
}