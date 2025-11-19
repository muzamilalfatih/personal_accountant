using Microsoft.AspNetCore.Identity;
using personal_accountant.DTOs;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Services
{
    public class PasswordService : IPasswordServiceInterface
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        public string HashPassword(UserDTO user)
        {
            if (user.Password == "") return "";
            return _hasher.HashPassword(user, user.Password);
        }

        public bool VerifyPassword(UserDTO user, string enteredPassword)
        {
            PasswordVerificationResult result =  _hasher.VerifyHashedPassword(user, user.Password, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
