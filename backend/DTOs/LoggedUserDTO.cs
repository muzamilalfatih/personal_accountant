namespace personal_accountant.DTOs
{
    public class LoggedUserDTO
    {
        public LoggedUserDTO(string accessToken, UserPublicDTO user)
        {
            AccessToken = accessToken;
            User = user;
        }

        public string AccessToken { get; set; }
        public UserPublicDTO User { get; set; }
    }
}
