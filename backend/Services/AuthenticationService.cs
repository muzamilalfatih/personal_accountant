using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Services;
using personal_accountant.DTOs;
using personal_accountant.DTOs.Mapper;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Services
{
    public class AuthenticationService : IAuthenticationServiceInterface
    {
        private readonly IUserServiceInterface _userService;
        private readonly IPasswordServiceInterface _passwordService;
        private readonly ITokenServiceInterface _tokenService;
        private readonly IConfirmationTokenServiceInterface _resetPasswordTokenService;
        private readonly IEmailSenderService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _connectionString;

        public AuthenticationService(IUserServiceInterface userService, IPasswordServiceInterface passwordService,
            ITokenServiceInterface tokenService, IConfirmationTokenServiceInterface resetPasswordTokenService, IOptions<DatabaseSettings> options,
            IEmailSenderService emailService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _resetPasswordTokenService = resetPasswordTokenService;
            _emailService = emailService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _connectionString = options.Value.DefautConnection;
        }
        public async Task<Result<LoggedUserDTO>> LogInAsync(LoggingDTO request)
        {
            Result<UserDTO> userResult = await _userService.FindAsync(request.Email);
            if (!userResult.Success)
            {
                if (userResult.ErrorCode == 404)
                    return new Result<LoggedUserDTO>(false, "Invalid username/password", null, 401);
                return new Result<LoggedUserDTO>(false, userResult.Message, null, userResult.ErrorCode);
            }
            if (!userResult.Data.IsConfirmed)
            {
                return new Result<LoggedUserDTO>(false, "Pending to confirm", null, 401);
            }
            if (!_passwordService.VerifyPassword(userResult.Data, request.Password))
            {
                return new Result<LoggedUserDTO>(false, "Invalid username/password", null, 401);
            }
            

            string token = _tokenService.GenerateToken(userResult.Data);

            LoggedUserDTO loggedUser = new LoggedUserDTO(token, userResult.Data.ToUserPublicDTO());

            return new Result<LoggedUserDTO>(true, "Successfully logged in!", loggedUser);
        }
        public async Task<Result<bool>> ForgetPasswordAsync([FromBody] string email)
        {
            Result<UserDTO> userResult = await _userService.FindAsync(email);
            if (!userResult.Success)
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);

            string token = _tokenService.GenerateResetToken();

            ConfirmationTokenDTO resetPasswordDTO = new ConfirmationTokenDTO(0, userResult.Data.Id, token, DateTime.Now.AddMinutes(15), false);

            Result<int> saveTokenResult = await _resetPasswordTokenService.AddNewAsync(resetPasswordDTO);
            if (!saveTokenResult.Success)
                return new Result<bool>(false, "An expected error on the server ", false, 500);
            var context = _httpContextAccessor.HttpContext;
            string frontendDomain =  context.Request.Headers["Origin"].ToString();

            
            string resetUrl = $"{frontendDomain}/reset-password/{token}";
            string body = $@"
<html>
<head>
  <meta charset='UTF-8' />
</head>

<body style='background:#f4f4f4; padding:40px; font-family:Arial, sans-serif; direction:rtl; text-align:right;'>

  <div style='max-width:480px; margin:auto; background:#ffffff; padding:30px; border-radius:12px; box-shadow:0 3px 10px rgba(0,0,0,0.1);'>

    <h2 style='color:#111827; margin-bottom:20px; font-size:22px;'>
      إعادة تعيين كلمة المرور
    </h2>

    <p style='color:#374151; font-size:16px; line-height:1.6;'>
      مرحباً،
    </p>

    <p style='color:#374151; font-size:16px; line-height:1.6;'>
      اضغط على الزر أدناه لإعادة تعيين كلمة المرور الخاصة بك.
    </p>

    <div style='text-align:center; margin:30px 0;'>
      <a href='{resetUrl}'
         style='background:#4f46e5; color:#ffffff; padding:12px 24px; border-radius:8px;
                text-decoration:none; font-size:16px; display:inline-block;'>
        إعادة تعيين كلمة المرور
      </a>
    </div>

    <p style='color:#6b7280; font-size:14px; line-height:1.6;'>
      إذا لم تطلب إعادة تعيين كلمة المرور، تجاهل هذه الرسالة بسهولة.
    </p>

  </div>

</body>
</html>";

            try
            {
                await _emailService.SendEmailAsync("muzamilalfatih123@gmail.com", "RESET PASSWORD", body);
            }catch (Exception ex)
            {

            }
            return new Result<bool>(true, "Email with link for reset password was sent!", true);
        }

        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {

            Result<ConfirmationTokenDTO> getResetTokenResult = await _resetPasswordTokenService.GetByTokenAsync(resetPasswordDTO.Token);
            if (!getResetTokenResult.Success || (getResetTokenResult.Success && (getResetTokenResult.Data.ExpireAt < DateTime.Now || getResetTokenResult.Data.IsUsed)))
                return new Result<bool>(false, "Invalid or expired token", false, 401);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<bool> resetPasswordResult = await _userService.ResetPasswordAsync(getResetTokenResult.Data.UserId, resetPasswordDTO.NewPassword, connection, transaction);
                    if (!resetPasswordResult.Success)
                    {
                        transaction.Rollback();
                        return new Result<bool>(false, resetPasswordResult.Message, false, resetPasswordResult.ErrorCode);
                    }
                    Result<bool> markAsUsed = await _resetPasswordTokenService.MarkAsUsedAsync(resetPasswordDTO.Token, connection, transaction);
                    if (!markAsUsed.Success)
                    {
                        transaction.Rollback();
                        return new Result<bool>(false, markAsUsed.Message, false, markAsUsed.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return resetPasswordResult;
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
