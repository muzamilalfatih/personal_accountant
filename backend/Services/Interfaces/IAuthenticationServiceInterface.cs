using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Services.Interfaces
{
    public interface IAuthenticationServiceInterface
    {
        public Task<Result<LoggedUserDTO>> LogInAsync(LoggingDTO request);
        public  Task<Result<bool>> ForgetPasswordAsync(string email);
        public Task<Result<bool>> ResetPasswordAsync( ResetPasswordDTO resetPasswordDTO);
    }
}
