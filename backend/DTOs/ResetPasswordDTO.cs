namespace personal_accountant.DTOs
{
    public class ResetPasswordDTO
    {
        public ResetPasswordDTO( string token,  string newPassword)
        {
            Token = token;
            NewPassword = newPassword;
        }

        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
