using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using personal_accountant.DTOs;
using personal_accountant.DTOs.Mapper;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Repositories
{
    public class AccountRepository : IAccountRepositoryInterface
    {
        private readonly string _connectionString;

        public AccountRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefautConnection;
        }
        public async Task<Result<int>> AddNewAsync(AccountDTO newAccount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

INSERT INTO Accounts
           (name
           ,user_id)
     VALUES
           (@name
           ,@userId)
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newAccount.Name);
                    command.Parameters.AddWithValue("@userId", newAccount.UserId);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Account added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add account.", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"
DELETE FROM Accounts
      WHERE id = @id
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);


                    try
                    {
                        await connection.OpenAsync();
                        int rowAffected = await command.ExecuteNonQueryAsync();

                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "account deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete account.", false, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<IEnumerable<AccountDetailDTO>>> GetAllAsync(int? userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
    a.id,
    a.name,
    a.date,
    COALESCE((SELECT SUM(t.amount) 
              FROM Transactions t 
              WHERE t.account_id = a.id), 0) AS total_balance
FROM Accounts a
WHERE a.user_id = @userId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", (object?) userId ?? DBNull.Value);

                    List<AccountDetailDTO> accounts = new List<AccountDetailDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {


                                accounts.Add(new AccountDetailDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("name")),
                                 reader.GetDateTime(reader.GetOrdinal("date")),
                                 reader.GetDecimal(reader.GetOrdinal("total_balance"))
                               ));
                            }
                            if (accounts.Count < 1)
                                return new Result<IEnumerable<AccountDetailDTO>>(false, "No account found.", null, 404);
                            return new Result<IEnumerable<AccountDetailDTO>>(true, "roles retrieved successfully.", accounts);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<AccountDetailDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<AccountDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Accounts WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                AccountDTO accountDTO = new AccountDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("name")),
                                 reader.GetInt32(reader.GetOrdinal("user_id"))
                               );
                                return new Result<AccountDTO>(true, "Account found successfully", accountDTO);
                            }
                            else
                            {
                                return new Result<AccountDTO>(false, "Account not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AccountDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<AccountDTO>> GetByNameAsync(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Accounts WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                AccountDTO accountDTO = new AccountDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("name")),
                                 reader.GetInt32(reader.GetOrdinal("user_id"))
                               );
                                return new Result<AccountDTO>(true, "Account found successfully", accountDTO);
                            }
                            else
                            {
                                return new Result<AccountDTO>(false, "Account not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AccountDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<int>> GetUserIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Top 1 UserId FROM Accounts 
WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int userId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "User id retrieved successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Account not found.", -1, 404);
                        }

                    }
                    catch (Exception ex)
                    {
                        return new Result<int>(false, "An unexpected error occurred on the server.", -1, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> IsExistAsync(string name, int  userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Accounts WHERE name = @name and user_id = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@userId", userId);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "Account existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, AccountDTO updatedAccount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Accounts
SET 
    name = @name, 
    user_id = @userId

WHERE id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", updatedAccount.Id);
                    command.Parameters.AddWithValue("@name", updatedAccount.Name);
                    command.Parameters.AddWithValue("@userId", updatedAccount.UserId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "account updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update account.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

    }
}
