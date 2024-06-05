using reservation_system_be.Helper;

namespace reservation_system_be.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
