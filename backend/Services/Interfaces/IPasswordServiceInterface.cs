using personal_accountant.DTOs;

namespace personal_accountant.Services.Interfaces
{
    public interface IPasswordServiceInterface
    {
        public string HashPassword(UserDTO user);
        public bool VerifyPassword(UserDTO user, string enteredPassword);
    }
}
