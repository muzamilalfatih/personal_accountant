using personal_accountant.Utilities;

namespace personal_accountant.Services.Interfaces
{
    public interface IEmailSenderService
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
