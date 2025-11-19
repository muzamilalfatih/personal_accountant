using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Services.Interfaces
{
    public interface ITransactionServiceInterface 
    {
        public Task<Result<TransactionDTO>> FindAsync(int id);
        public Task<Result<IEnumerable<TransactionDetailDTO>>> GetAllAsync(int? accountId);
        public Task<Result<int>> AddNewAsync(TransactionDTO newTransaction);
        public Task<Result<bool>> UpdateAsync(int id, TransactionDTO updatedTransaction);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
