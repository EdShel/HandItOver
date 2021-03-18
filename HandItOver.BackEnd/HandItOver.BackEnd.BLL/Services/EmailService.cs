using HandItOver.BackEnd.Infrastructure.Models.Auth;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class EmailService
    {
        private readonly EmailOptions options;

        public EmailService(EmailOptions options)
        {
            this.options = options;
        }

        public async Task SendAsync(string receiver, string subject, string bodyHtml)
        {
            if (!this.options.Enabled)
            {
                return;
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.options.Name, this.options.Email));
            emailMessage.To.Add(new MailboxAddress(string.Empty, receiver));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = bodyHtml
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
