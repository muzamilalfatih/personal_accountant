using Microsoft.Data.SqlClient;
using personal_accountant.DTOs;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Services
{
    public class ConfirmationTokenService(IConfirmationTokenRepositoryInterface repo) : IConfirmationTokenServiceInterface
    {
        public async Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO)
        {
            return await repo.AddNewAsync(passwordResetTokenDTO);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token)
        {
           return await repo.GetByTokenAsync(token); 
        }

        public async Task<Result<bool>> MarkAsUsedAsync(string token, SqlConnection conn, SqlTransaction tran)
        {
            return await repo.MarkAsUsedAsync(token, conn, tran);
        }
    }
}
