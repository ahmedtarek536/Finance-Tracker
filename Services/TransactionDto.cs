namespace Finance_Tracker.Services
{
    public class TransactionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Payee { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
