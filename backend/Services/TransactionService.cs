using personal_accountant.DTOs;
using personal_accountant.Repositories;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Services
{
    public class TransactionService : ITransactionServiceInterface
    {
        private readonly ITransactionRepositoryInterface _repo;
        public TransactionService(ITransactionRepositoryInterface repo)
        {
            _repo = repo;
        }

        public async Task<Result<int>> AddNewAsync(TransactionDTO newTransaction)
        {
           return await _repo.AddNewAsync(newTransaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<TransactionDetailDTO>>> GetAllAsync(int? accountId)
        {
            
            return await _repo.GetAllAsync(accountId);
        }

        public async Task<Result<TransactionDTO>> FindAsync(int id)
        {
           return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, TransactionDTO updatedTransaction)
        {
            return await _repo.UpdateAsync(id, updatedTransaction);
        }
    }
}
