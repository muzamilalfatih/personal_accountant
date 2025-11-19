namespace personal_accountant.DTOs
{
    public class UserDTO
    {
        public UserDTO(int id, string firstName, string lastName, string email, string password, string role = "User", bool isConfirmed = false)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            IsConfirmed = isConfirmed;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } =string.Empty;
        public bool IsConfirmed { get; set; }
    }
}
