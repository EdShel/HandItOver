namespace HandItOver.BackEnd.Infrastructure.Models.Auth
{
    public class JwtTokenSettings
    {
        public string ValidIssuer { get; set; } = null!;

        public string SigningKey { get; set; } = null!;

        public bool ValidateLifetime { get; set; } 

        public int TokenLifetimeSeconds { get; set; }
    }
}
