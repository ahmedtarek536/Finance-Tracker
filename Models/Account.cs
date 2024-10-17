using System.ComponentModel.DataAnnotations;

namespace Finance_Tracker.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = default!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
