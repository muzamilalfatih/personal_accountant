using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using personal_accountant.DTOs;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Utilities;
using System.Data;

namespace personal_accountant.Repositories
{
    public class UserRepository : IUserRepositoryInterface
    {
        private readonly string _connectionString;
        public UserRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefautConnection;
        }
        public async Task<Result<int>> AddNewAsync(UserDTO newUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

INSERT INTO Users
           (first_name
           ,last_name
           ,email
           ,password)
     VALUES
           (@first_name
           ,@last_name
           ,@email
           ,@password)
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@first_name", newUser.FirstName);
                    command.Parameters.AddWithValue("@last_name", newUser.LastName);
                    command.Parameters.AddWithValue("@email", newUser.Email);
                    command.Parameters.AddWithValue("@password", newUser.Password);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "User added successfully.", id);
                        }
                        else
                        {
                            return new Result<int>(false, "Failed to add user.", -1);
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
DELETE FROM Users
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
                            return new Result<bool>(true, "user deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete user.", false, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int? currentUserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
FROM Users
WHERE (@currentUserId IS NULL OR id <> @currentUserId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@currentUserId", currentUserId == null ? DBNull.Value : currentUserId);
                    List<UserPublicDTO> users = new List<UserPublicDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {


                                users.Add(new UserPublicDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("first_name")),
                                 reader.GetString(reader.GetOrdinal("Last_name")),
                                 reader.GetString(reader.GetOrdinal("email")),
                                 reader.GetString(reader.GetOrdinal("role"))
                               ));
                            }
                            if (users.Count < 1)
                                return new Result<IEnumerable<UserPublicDTO>>(false, "No user found.", null, 404);
                            return new Result<IEnumerable<UserPublicDTO>>(true, "roles retrieved successfully.", users);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<UserPublicDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }
        public async Task<Result<UserDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Users WHERE id = @id";
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
                                UserDTO userDTO = new UserDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("first_name")),
                                 reader.GetString(reader.GetOrdinal("Last_name")),
                                 reader.GetString(reader.GetOrdinal("email")),
                                 reader.GetString(reader.GetOrdinal("password")),
                                 reader.GetString(reader.GetOrdinal("role")),
                                 reader.GetBoolean(reader.GetOrdinal("is_confirmed"))
                               );
                                return new Result<UserDTO>(true, "User found successfully", userDTO);
                            }
                            else
                            {
                                return new Result<UserDTO>(false, "User not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<UserDTO>> GetByEmailAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Users WHERE email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                UserDTO userDTO = new UserDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                 reader.GetString(reader.GetOrdinal("first_name")),
                                 reader.GetString(reader.GetOrdinal("Last_name")),
                                 reader.GetString(reader.GetOrdinal("email")),
                                 reader.GetString(reader.GetOrdinal("password")),
                                 reader.GetString(reader.GetOrdinal("role")),
                                 reader.GetBoolean(reader.GetOrdinal("is_confirmed"))
                                 );
                                return new Result<UserDTO>(true, "User found successfully", userDTO);
                            }
                            else
                            {
                                return new Result<UserDTO>(false, "User not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Users
SET 
    first_name = @firstName, 
    last_name = @lastName,
    password = CASE WHEN @password <> '' THEN @password ELSE password END
WHERE id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@firstName", updatedUser.FirstName);
                    command.Parameters.AddWithValue("@lastName", updatedUser.LastName);
                    command.Parameters.AddWithValue("@password", updatedUser.Password);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "user updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update user.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> IsExistAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Users WHERE Email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "User existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> ToggleRoleAsync(int id, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Users
SET 
    role = @role
WHERE id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@role", role);


                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Role toggled successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to toggle user.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
UPDATE Users
SET 
    password = @newPassword
WHERE id = @id;
select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newPassword", newPassword);
                object result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Password changed successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to  change password.", false);
                }



            }
        }
        public async Task<Result<bool>> ResetPasswordAsync(int id, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Users
SET 
    password = @newPassword
WHERE id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@newPassword", newPassword);
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Password changed successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to  change password.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> ConfirmEmail(int id,  SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
UPDATE Users
SET 
    is_confirmed = 1
WHERE id = @id;
select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@id", id);
                object result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Email confirmed successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to  confirm email .", false);
                }



            }
        }
    }
}
