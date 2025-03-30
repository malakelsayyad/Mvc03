using Company.G02.PL.Services.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.G02.PL.Helpers
{
    public class EmailSettingsWorkshop : IMailSettingsWorkshop
    {
        private MailSettingsWorkshop _options;

        public EmailSettingsWorkshop(IOptions<MailSettingsWorkshop> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(MailboxAddress.Parse(_options.Email));

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = email.Body;

            mail.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);

            smtp.Disconnect(true);

        }
    }
}