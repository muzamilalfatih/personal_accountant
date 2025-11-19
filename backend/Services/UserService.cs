using Azure.Core;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using personal_accountant.DTOs;
using personal_accountant.DTOs.Mapper;
using personal_accountant.Repositories;
using personal_accountant.Repositories.Interfaces;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Services
{
    public class UserService : IUserServiceInterface
    {
        private readonly IUserRepositoryInterface _repo;
        private readonly IPasswordServiceInterface _passwordService;
        private readonly ITokenServiceInterface _tokenService;
        private readonly IConfirmationTokenServiceInterface _resetPasswordTokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailService;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly string _connectionString;

        public UserService(IUserRepositoryInterface repo, IPasswordServiceInterface passwordService, ITokenServiceInterface tokenService,
            IConfirmationTokenServiceInterface resetPasswordTokenService, IConfiguration configuration, IEmailSenderService emailService,
            IOptions<DatabaseSettings> options, HttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _resetPasswordTokenService = resetPasswordTokenService;
            _configuration = configuration;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _connectionString = options.Value.DefautConnection;
        }

        
        async Task<Result<bool>> _sendConfirmationEmail(int id)
        {
            string token = _tokenService.GenerateResetToken();

            var resetPasswordDTO = new ConfirmationTokenDTO(
                0,
                id,
                token,
                DateTime.Now.AddMinutes(15),
                false
            );

            Result<int> saveTokenResult = await _resetPasswordTokenService.AddNewAsync(resetPasswordDTO);
            if (!saveTokenResult.Success)
                return new Result<bool>(false, "An unexpected server error occurred.", false, 500);


            var context = _httpContextAccessor.HttpContext;
            string frontendDomain = context.Request.Headers["Origin"].ToString();
            string resetUrl = $"{frontendDomain}/confirm-email/{token}";

            string body = $@"
<html>
<head>
  <meta charset='UTF-8' />
</head>

<body style='background:#f4f4f4; padding:40px; font-family:Arial, sans-serif; direction:rtl; text-align:right;'>

  <div style='max-width:480px; margin:auto; background:#ffffff; padding:30px; border-radius:12px; box-shadow:0 3px 10px rgba(0,0,0,0.1);'>

    <h2 style='color:#111827; margin-bottom:20px; font-size:22px;'>
      تأكيد البريد الإلكتروني
    </h2>

    <p style='color:#374151; font-size:16px; line-height:1.6;'>
      مرحباً،
    </p>

    <p style='color:#374151; font-size:16px; line-height:1.6;'>
      اضغط على الزر أدناه لتأكيد بريدك الإلكتروني وتفعيل حسابك.
    </p>

    <div style='text-align:center; margin:30px 0;'>
      <a href='{resetUrl}'
         style='background:#4f46e5; color:#ffffff; padding:12px 24px; border-radius:8px;
                text-decoration:none; font-size:16px; display:inline-block;'>
        تأكيد البريد الإلكتروني
      </a>
    </div>

    <p style='color:#6b7280; font-size:14px; line-height:1.6;'>
      إذا لم تقم بإنشاء حساب أو لم تطلب هذا الإجراء، يمكنك تجاهل هذه الرسالة بأمان.
    </p>

  </div>

</body>
</html>";


            // 8. Send email
            try
            {
                await _emailService.SendEmailAsync("muzamilalfatih123@gmail.com", "Confirm Email", body);
                return new Result<bool>(true, "Confirmation email sent successfully");
            }
            catch (Exception ex)
            {
                return new Result<bool>(false, "failed to send confirmation email", false, 500);
            }
            
        }
        public async Task<Result<int>> AddNewAsync(UserDTO newUser)
        {
            int id = 0;

            Result<UserDTO> userResult = await _repo.GetByEmailAsync(newUser.Email);
            if (!userResult.Success && userResult.ErrorCode is not 404)
                return new Result<int>(false, userResult.Message, -1, userResult.ErrorCode);
            if (userResult.Success && userResult.Data.IsConfirmed)
                return new Result<int>(false, "This email is already registered!", -1, 409);
            newUser.Password = _passwordService.HashPassword(newUser);

            if (userResult.Success && !userResult.Data.IsConfirmed)
            {
                Result<bool> updateResult =  await _repo.UpdateAsync(userResult.Data.Id, newUser);
                if (!updateResult.Success)                
                   return new Result<int>(false, updateResult.Message, -1, userResult.ErrorCode);
                id = userResult.Data.Id;
            }
            else
            {
                Result<int> addUserResult = await _repo.AddNewAsync(newUser);
                if (!addUserResult.Success)
                    return addUserResult;
                id = addUserResult.Data;
            }

            
            Result<bool> confirmationResult = await _sendConfirmationEmail(id);
            if (!confirmationResult.Success)
                return new Result<int>(false, "Failed to register user", -1, confirmationResult.ErrorCode);
            return new Result<int>(true, "User resgister successfully", id);

        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<UserDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<UserDTO>> FindAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }

        public  async Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int? currentUserId)
        {
            return await _repo.GetAllAsync(currentUserId);
        }

        public async Task<Result<bool>> IsExistAsync(string email)
        {
            return await _repo.IsExistAsync(email);
        }
        public Task<Result<bool>> ToggleRoleAsync(int id, string role)
        {
            return _repo.ToggleRoleAsync(id, role);
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser)
        {


            Result<UserDTO> existingUser = await _repo.GetByEmailAsync(updatedUser.Email);
            if (!existingUser.Success && existingUser.ErrorCode != 404)
                return new Result<bool>(false, existingUser.Message, false, existingUser.ErrorCode);

            if (existingUser.Success &&  existingUser.Data.Id != id)
                return new Result<bool>(false, "This email is already register!", false, 409);

            updatedUser.Password = _passwordService.HashPassword(updatedUser);
            return await _repo.UpdateAsync(id, updatedUser);
        }

        public async Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction tran)
        {
            Result<UserDTO> userResult = await _repo.GetByIdAsync(id);
            if (!userResult.Success)
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);


            userResult.Data.Password = newPassword;

            return await _repo.ResetPasswordAsync(id, _passwordService.HashPassword(userResult.Data), conn, tran);
        }

        public async Task<Result<bool>> ChangedPassword(int id, ChangePasswordDTO changePasswordDTO)
        {
            Result<UserDTO> userResult = await _repo.GetByIdAsync(id);
            if (!userResult.Success)
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);

            if (!_passwordService.VerifyPassword(userResult.Data, changePasswordDTO.CurrentPassword))
            {
                return new Result<bool>(false, "Invalid password", false, 400);
            }

            userResult.Data.Password = changePasswordDTO.NewPassword;

            return await _repo.ResetPasswordAsync(id, _passwordService.HashPassword(userResult.Data));
        }

        public async Task<Result<bool>> ConfirmEmail(string token)
        {
            Result<ConfirmationTokenDTO> getResetTokenResult = await _resetPasswordTokenService.GetByTokenAsync(token);
            if (!getResetTokenResult.Success || (getResetTokenResult.Success && (getResetTokenResult.Data.ExpireAt < DateTime.Now || getResetTokenResult.Data.IsUsed)))
                return new Result<bool>(false, "Invalid or expired token", false, 401);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<bool> confirmEmailResult = await _repo.ConfirmEmail(getResetTokenResult.Data.UserId, connection, transaction);
                    if (!confirmEmailResult.Success)
                    {
                        transaction.Rollback();
                        return confirmEmailResult;
                    }
                    Result<bool> markAsUsed = await _resetPasswordTokenService.MarkAsUsedAsync(token, connection, transaction);
                    if (!markAsUsed.Success)
                    {
                        transaction.Rollback();
                        return new Result<bool>(false, markAsUsed.Message, false, markAsUsed.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return confirmEmailResult ;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }
            }
        }
    }
}
