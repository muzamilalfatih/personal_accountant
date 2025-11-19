using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Repositories.Interfaces
{
    public interface IAccountRepositoryInterface 
    {
        public Task<Result<AccountDTO>> GetByIdAsync(int id);
        public Task<Result<AccountDTO>> GetByNameAsync(string name);
        public Task<Result<IEnumerable<AccountDetailDTO>>> GetAllAsync(int? userId);
        public Task<Result<int>> AddNewAsync(AccountDTO newAccount);
        public Task<Result<bool>> UpdateAsync(int id, AccountDTO updatedAccount);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<bool>> IsExistAsync(string name, int userId);
        public Task<Result<int>> GetUserIdAsync(int id);
    }
}
