namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public static class AuthConstants
    {
        public static class Claims
        {
            public const string ROLE = "role";

            public const string EMAIL = "email";

            public const string ID = "id";
        }

        public static class Roles
        {
            public const string DEFAULT = "user";

            public const string ADMIN = "admin";
        }
    }
}
