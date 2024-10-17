using System.ComponentModel.DataAnnotations;

namespace Finance_Tracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Payee { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; set; }

        [Required]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = default!;

        [Required]
        public Account Account { get; set; } = default!;

        [Required]
        public Category Category { get; set; } = default!;
    }
}
