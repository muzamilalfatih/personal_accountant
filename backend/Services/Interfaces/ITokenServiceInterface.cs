using personal_accountant.DTOs;

namespace personal_accountant.Services.Interfaces
{
    public interface ITokenServiceInterface
    {
        string GenerateToken(UserDTO user);
        public  string GenerateResetToken(int length = 32);
    }
}
