namespace personal_accountant.DTOs
{
    public class ChangePasswordDTO
    {
        public ChangePasswordDTO(string currentPassword, string newPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
