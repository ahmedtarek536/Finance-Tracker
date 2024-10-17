using System.ComponentModel.DataAnnotations;

namespace Finance_Tracker.Models
{

    public class User
    {
        public int Id { get; set; }

        [Required, MinLength(3)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }

}
