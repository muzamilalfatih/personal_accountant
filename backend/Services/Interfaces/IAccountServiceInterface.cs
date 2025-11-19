using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Services.Interfaces
{
    public interface IAccountServiceInterface 
    {
 
        public Task<Result<AccountDTO>> FindAsync(int id);
        public Task<Result<IEnumerable<AccountDetailDTO>>> GetAllAsync(int? userId);
        public Task<Result<int>> AddNewAsync(AccountDTO newAccount);
        public Task<Result<bool>> UpdateAsync(int id, AccountDTO updatedAccount);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<int>> GetUserIdAsync(int id);
    }
}
