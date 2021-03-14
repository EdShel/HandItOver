﻿using System;

namespace HandItOver.BackEnd.DAL.Entities
{
    public enum MailboxSize
    {
        Small,
        Medium,
        Big
    }

    public class Mailbox
    {
        public string Id { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        public MailboxSize Size { get; set; }

        public string GroupId { get; set; } = null!;

        public string PhysicalId { get; set; } = null!;
    }

    public class MailboxGroup
    {
        public string ClusterId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool WhitelistOnly { get; set; }
    }

    public class MailboxWhitelist
    {
        public string MailBoxId { get; set; } = null!;

        public string AppUserId { get; set; } = null!;
    }

    public class MailboxRent
    {
        public string RentId { get; set; } = null!;

        public string MailboxId { get; set; } = null!;

        public string RenterId { get; set; } = null!;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }

    public class Delivery
    {
        public string Id { get; set; } = null!;

        public float Weight { get; set; }

        public string AddresseeId { get; set; } = null!;

        public string MailboxId { get; set; } = null!;

        public DateTime Arrived { get; set; }

        public DateTime Taken { get; set; }
    }
}
