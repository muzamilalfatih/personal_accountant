namespace personal_accountant.DTOs
{
    public class LoggingDTO
    {
        public LoggingDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email {  get; set; }
        public string Password { get; set; }
    }
}
