using HandItOver.BackEnd.BLL.Services.Notification;
using HandItOver.BackEnd.Infrastructure.Models.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public class CertExpirationNotifyService : HostedService
    {
        private readonly SslMonitoringOptions options;

        private readonly EmailService emailService;

        public CertExpirationNotifyService(
            SslMonitoringOptions options,
            EmailService emailService)
        {
            this.options = options;
            this.emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!this.options.Enabled)
            {
                return;
            }
            do
            {
                DateTime? expires = await new CertExpirationChecker().GetCertExpirationDateAsync(this.options.Host);
                bool expired = expires.HasValue
                    && (expires.Value - DateTime.UtcNow).TotalMinutes <= this.options.MinutesUntilExpiration;
                if (expired)
                {
                    await this.emailService.SendAsync(
                        this.options.EmailForNotifications,
                        "Hand It Over SSL expiration",
                        $"The certificate will expire at {expires.Value}");

                    await Task.Delay(this.options.NotificationDelayMinutes * 60 * 1000, cancellationToken);
                }
                else
                {
                    await Task.Delay(this.options.PeriodMinutes * 60 * 1000, cancellationToken);
                }
            }
            while (!cancellationToken.IsCancellationRequested);
        }

    }
}
