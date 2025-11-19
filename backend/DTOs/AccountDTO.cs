using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace personal_accountant.DTOs
{
    public class AccountDTO
    {
        public AccountDTO(int id, string name, int userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public decimal TotalBalance { get; set; } = 0;
    }
}
