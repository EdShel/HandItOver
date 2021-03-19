using HandItOver.BackEnd.BLL.Models.Notification;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Notification
{
    public class EmailService : INotificationService
    {
        private readonly EmailOptions options;

        public EmailService(EmailOptions options)
        {
            this.options = options;
        }

        public async Task SendAsync(NotificationMessage message)
        {
            if (!this.options.Enabled)
            {
                return;
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.options.Name, this.options.Email));
            emailMessage.To.Add(new MailboxAddress(string.Empty, message.ReceiverAddress));
            emailMessage.Subject = message.Title;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(this.options.Server, this.options.Port, this.options.UseSsl);
                await client.AuthenticateAsync(this.options.Email, this.options.Password);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

    }
}
