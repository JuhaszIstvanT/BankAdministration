using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public int Balance { get; set; }
        public DateTime StartDate { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
