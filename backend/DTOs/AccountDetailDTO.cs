namespace personal_accountant.DTOs
{
    public class AccountDetailDTO
    {
        public AccountDetailDTO(int id, string name, DateTime date, decimal totalBalance)
        {
            Id = id;
            Name = name;
            Date = date;
            TotalBalance = totalBalance;
        }

        public int Id {  get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
