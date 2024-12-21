using ECommercelib.SharedLibrary.DTOs;

namespace EmailWebhook.Services
{
    public interface IEmailService
    {
        string SendEmail(EmailDTO email);
    }
}
