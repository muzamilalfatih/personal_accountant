using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using personal_accountant.DTOs;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Utilities;
using System.Data;

namespace personal_accountant.Repositories
{
    public class ConfirmationTokenRepository : IConfirmationTokenRepositoryInterface
    {
        private readonly string _connectionString;
        public ConfirmationTokenRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefautConnection;
        }

        public async Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

INSERT INTO ConfirmationTokens
           (user_id
           ,token
           ,expires_at
           ,is_used)
     VALUES
           (@user_id
           ,@token
           ,@expires_at
           ,@is_used)
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", passwordResetTokenDTO.UserId);
                    command.Parameters.AddWithValue("@token", passwordResetTokenDTO.Token);
                    command.Parameters.AddWithValue("@expires_at", passwordResetTokenDTO.ExpireAt);
                    command.Parameters.AddWithValue("@is_used", passwordResetTokenDTO.IsUsed);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Token added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add token.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM ConfirmationTokens WHERE token = @token";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", token);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ConfirmationTokenDTO resetTokenDTO = new ConfirmationTokenDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                 reader.GetString(reader.GetOrdinal("token")),
                                 reader.GetDateTime(reader.GetOrdinal("expires_at")),
                                 reader.GetBoolean(reader.GetOrdinal("is_used"))
                                 );
                                return new Result<ConfirmationTokenDTO>(true, "Token found successfully", resetTokenDTO);
                            }
                            else
                            {
                                return new Result<ConfirmationTokenDTO>(false, "Token not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<ConfirmationTokenDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> MarkAsUsedAsync(string token, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
UPDATE ConfirmationTokens
SET 
    is_used = 1
WHERE token = @token;
select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@token", token);
                object result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Token marked as used successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to mark token as used.", false);
                }


            }
        }
    }
}
