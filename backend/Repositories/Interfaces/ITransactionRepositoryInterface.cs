using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Repositories.Interfaces
{
    public interface ITransactionRepositoryInterface 
    {
        public Task<Result<TransactionDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<TransactionDetailDTO>>> GetAllAsync(int? accountId);
        public Task<Result<int>> AddNewAsync(TransactionDTO newTransaction);
        public Task<Result<bool>> UpdateAsync(int id, TransactionDTO updatedTransaction);
        public Task<Result<bool>> DeleteAsync(int id);
        
    }
}
