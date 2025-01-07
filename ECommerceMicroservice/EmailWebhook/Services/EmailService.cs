using ECommercelib.SharedLibrary.DTOs;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

namespace EmailWebhook.Services
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailDTO email)
        {
           var _email = new MimeMessage();
            _email.From.Add(MailboxAddress.Parse("hilda42@ethereal.email"));
            _email.To.Add(MailboxAddress.Parse("hilda42@ethereal.email"));
            _email.Subject = email.Title;
            _email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.Content
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("hilda42@ethereal.email", "95q573TudHfkm2vCaX", CancellationToken.None);
            smtp.Send(_email);
            smtp.Disconnect(true);
            return "Email отправлен";
                
        }
    }
}
