using MailKit.Security;
using MimeKit;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;
using MailKit.Net.Smtp;
namespace personal_accountant.Services
{
    public class EmailSenderService(IConfiguration configuration) : IEmailSenderService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {

            var email = configuration["EMAIL_CONFIGURATION:EMAIL"];
            var password = configuration["EMAIL_CONFIGURATION:PASSWORD"];
            var host = configuration["EMAIL_CONFIGURATION:HOST"];
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(email, password);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", email));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
    }
}
