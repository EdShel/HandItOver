namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public static class AuthConstants
    {
        public static class Claims
        {
            public const string ROLE = "role";

            public const string EMAIL = "email";

            public const string ID = "id";

            public const string MAILBOX_ID = "mailboxId";
        }

        public static class Roles
        {
            public const string USER = "user";

            public const string ADMIN = "admin";

            public const string MAILBOX = "mailbox";
        }

        public static class Policies
        {
            public const string MAILBOX_OWNER_ONLY = "mailboxOwner";

            public const string MAILBOX_GROUP_OWNER_ONLY = "mailboxGroup";

            public const string RENTER_OR_OWNER_ONLY = "renterOrOwner";

            public const string DELIVERY_ADDRESSEE_ONLY = "addressee";
        }
    }
}
