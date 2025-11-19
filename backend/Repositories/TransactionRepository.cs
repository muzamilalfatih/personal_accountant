using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using personal_accountant;
using personal_accountant.DTOs;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Utilities;
using System.Transactions;

namespace personal_accountant.Repositories
{
    public class TransactionRepository : ITransactionRepositoryInterface
    {
        private readonly string _connectionString;

        public TransactionRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefautConnection;
        }
        public async Task<Result<int>> AddNewAsync(TransactionDTO newTransaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

INSERT INTO Transactions
           (amount,
            description,
            account_id)
     VALUES
           (
            @amount,
            @description,
            @accountId)
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@amount", newTransaction.Amount);
                    command.Parameters.AddWithValue("@accountId", newTransaction.AccountId);
                    command.Parameters.AddWithValue("@description", newTransaction.Description);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "Transaction added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add transaction.", -1);
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
DELETE FROM Transactions
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
                            return new Result<bool>(true, "transaction deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete transaction.", false, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<IEnumerable<TransactionDetailDTO>>> GetAllAsync(int? accountId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
    id, 
    amount, 
    date, 
	description,
    account_id,
    SUM(amount) OVER() AS total_amount
FROM Transactions
WHERE (@accountId is null or account_id = @accountId);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@accountId", (object?) accountId ?? DBNull.Value);

                    List<TransactionDetailDTO> transactions = new List<TransactionDetailDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new TransactionDetailDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetDecimal(reader.GetOrdinal("amount")),
                                 reader.GetDateTime(reader.GetOrdinal("date")),
                                 reader.GetString(reader.GetOrdinal("description")),
                                 reader.GetInt32(reader.GetOrdinal("account_id")),
                                 reader.GetDecimal(reader.GetOrdinal("total_amount"))
                               ));
                            }
                            if (transactions.Count < 1)
                                return new Result<IEnumerable<TransactionDetailDTO>>(false, "No transaction found.", null, 404);
                            return new Result<IEnumerable<TransactionDetailDTO>>(true, "transactions retrieved successfully.", transactions);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<TransactionDetailDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }
        
        public async Task<Result<TransactionDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Transaction WHERE id = @id";
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
                                
                                TransactionDTO transactionDTO = new TransactionDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetDecimal(reader.GetOrdinal("amount")),
                                    reader.GetDateTime(reader.GetOrdinal("date")),
                                    reader.GetString(reader.GetOrdinal("description")),
                                    reader.GetInt32(reader.GetOrdinal("account_id"))
                               );
                                return new Result<TransactionDTO>(true, "Transaction found successfully", transactionDTO);
                            }
                            else
                            {
                                return new Result<TransactionDTO>(false, "Transaction not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<TransactionDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, TransactionDTO updatedTransaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Transactions
SET 
    amount = @amount, 
    date = @date,
    account_id = @accountId,
    description = @description

WHERE id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@id", updatedTransaction.Id);
                    command.Parameters.AddWithValue("@amount", updatedTransaction.Amount);
                    command.Parameters.AddWithValue("@date", updatedTransaction.Date);
                    command.Parameters.AddWithValue("@description", updatedTransaction.Description);
                    command.Parameters.AddWithValue("@accountId", updatedTransaction.AccountId);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "transaction updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update transaction.", false);
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
