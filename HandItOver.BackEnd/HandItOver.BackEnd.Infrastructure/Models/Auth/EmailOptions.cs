namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public class EmailOptions
    {
        public bool Enabled { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Server { get; set; } = null!;

        public int Port { get; set; }

        public bool UseSsl { get; set; }
    }
}
