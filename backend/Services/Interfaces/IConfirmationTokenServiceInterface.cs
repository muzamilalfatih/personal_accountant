using Microsoft.Data.SqlClient;
using personal_accountant.DTOs;
using personal_accountant.Utilities;

namespace personal_accountant.Services.Interfaces
{
    public interface IConfirmationTokenServiceInterface
    {
        public Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO);
        public Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token);
        public Task<Result<bool>> MarkAsUsedAsync(string token, SqlConnection conn, SqlTransaction tran);
    }
}
