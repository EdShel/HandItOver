using HandItOver.BackEnd.DAL.Entities.Auth;
using System;
using System.Collections.Generic;

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

        public string? GroupId { get; set; }

        public string PhysicalId { get; set; } = null!;

        public bool IsOpen { get; set; }

        public MailboxGroup? MailboxGroup { get; set; } = null!;

        public ICollection<Delivery> Deliveries { get; set; } = null!;

        public ICollection<MailboxRent> Rents { get; set; } = null!;
    }

    public class MailboxGroup
    {
        public string GroupId { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool WhitelistOnly { get; set; }

        public ICollection<Mailbox> Mailboxes { get; set; } = null!;

        public ICollection<AppUser> Whitelisted { get; set; } = null!;
    }

    public class MailboxRent
    {
        public string RentId { get; set; } = null!;

        public string MailboxId { get; set; } = null!;

        public string RenterId { get; set; } = null!;

        public DateTime From { get; set; }

        public DateTime Until { get; set; }

        public Mailbox Mailbox { get; set; } = null!;
    }

    public class Delivery
    {
        public string Id { get; set; } = null!;

        public float Weight { get; set; }

        public string AddresseeId { get; set; } = null!;

        public string MailboxId { get; set; } = null!;

        public DateTime Arrived { get; set; }

        public DateTime? Taken { get; set; }

        public Mailbox Mailbox { get; set; } = null!;
    }
}
