namespace personal_accountant.DTOs
{
    public class ConfirmationTokenDTO
    {
        public ConfirmationTokenDTO(int id, int userId, string token, DateTime expireAt, bool isUsed)
        {
            Id = id;
            UserId = userId;
            Token = token;
            ExpireAt = expireAt;
            IsUsed = isUsed;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
