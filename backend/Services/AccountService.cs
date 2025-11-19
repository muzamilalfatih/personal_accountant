using personal_accountant.DTOs;
using personal_accountant.Repositories;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;
using System.Security.Principal;

namespace personal_accountant.Services
{
    public class AccountService : IAccountServiceInterface
    {
        private readonly IAccountRepositoryInterface _repo;

        public AccountService(IAccountRepositoryInterface repo)
        {
            _repo = repo;
        }
        public async Task<Result<int>> AddNewAsync(AccountDTO newAccount)
        {
            Result<bool> existenceResult = await _repo.IsExistAsync(newAccount.Name, newAccount.UserId);
            if (!existenceResult.Success)
                return new Result<int>(false, existenceResult.Message, -1, existenceResult.ErrorCode);
            if (existenceResult.Data)
                return new Result<int>(false, "This name is already exist!", -1, 409);
            return await _repo.AddNewAsync(newAccount);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<AccountDetailDTO>>> GetAllAsync(int? userId)
        {
            return await _repo.GetAllAsync(userId);
        }

        public async Task<Result<AccountDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, AccountDTO updatedAccount)
        {
             Result<AccountDTO> existingAccount = await _repo.GetByNameAsync(updatedAccount.Name);

            if (!existingAccount.Success && existingAccount.ErrorCode != 404)
                return new Result<bool>(false, existingAccount.Message, false, existingAccount.ErrorCode);

            if (existingAccount.Success && existingAccount.Data.Id != id)
                return new Result<bool>(false, "This name is already exist!!", false, 409);

          
            return await _repo.UpdateAsync(id, updatedAccount);
        }

        public Task<Result<int>> GetUserIdAsync(int id)
        {
            return _repo.GetUserIdAsync(id);
        }
    }
}
