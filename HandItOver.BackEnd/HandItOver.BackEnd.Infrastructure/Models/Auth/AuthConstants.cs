namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public static class AuthConstants
    {
        public static class Claims
        {
            public const string ROLE = "role";

            public const string EMAIL = "email";

            public const string ID = "id";

            public const string OWNER_ID = "onwerId";
        }

        public static class Roles
        {
            public const string USER = "user";

            public const string ADMIN = "admin";

            public const string MAILBOX = "mailbox";
        }

        public static class Policies
        {
            public const string MAILBOX_OWNER_ONLY = "mailbox";

            public const string MAILBOX_GROUP_OWNER_ONLY = "mailboxGroup";
        }
    }
}
