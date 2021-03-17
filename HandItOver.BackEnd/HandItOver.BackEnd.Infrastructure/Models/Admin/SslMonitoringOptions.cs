namespace HandItOver.BackEnd.Infrastructure.Models.Admin
{
    public class SslMonitoringOptions
    {
        public bool Enabled { get; set; }

        public int PeriodMinutes { get; set; }

        public int MinutesUntilExpiration { get; set; }

        public string Host { get; set; } = null!;

        public int NotificationDelayMinutes { get; set; }

        public string EmailForNotifications { get; set; } = null!;
    }
}
