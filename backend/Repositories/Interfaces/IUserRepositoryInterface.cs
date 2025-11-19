using Microsoft.Data.SqlClient;
using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Repositories.Interfaces
{
    public interface IUserRepositoryInterface 
    {
        public Task<Result<UserDTO>> GetByIdAsync(int id);
        public Task<Result<UserDTO>> GetByEmailAsync(string email);
        public Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int? currentUserId);
        public Task<Result<int>> AddNewAsync(UserDTO newUser);
        public Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<bool>> IsExistAsync(string email);
        public Task<Result<bool>> ToggleRoleAsync(int id, string role);
        public Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction tran);
        public Task<Result<bool>> ConfirmEmail(int id, SqlConnection conn, SqlTransaction tran);
        public Task<Result<bool>> ResetPasswordAsync(int id, string newPassword);
    }
}
