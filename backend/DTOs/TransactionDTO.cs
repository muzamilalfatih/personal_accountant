namespace personal_accountant.DTOs
{
    public class TransactionDTO
    {
        public TransactionDTO(int id,  decimal amount, DateTime date,string description, int accountId)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Description = description;
            AccountId = accountId;
        }

        public int Id { get; set; }
        public decimal Amount { get; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; }
        public int AccountId { get; set; }
    }
}
