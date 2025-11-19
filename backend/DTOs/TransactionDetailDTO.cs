namespace personal_accountant.DTOs
{
    public class TransactionDetailDTO
    {
        public TransactionDetailDTO(int id, decimal amount, DateTime date,string description, int accountId, decimal totalAmount)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Description = description;
            AccountId = accountId;
            TotalAmount = totalAmount;
        }

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; }
        public int AccountId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
