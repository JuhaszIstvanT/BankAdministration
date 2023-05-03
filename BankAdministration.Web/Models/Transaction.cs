using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; } = null!;
        public string SourceAccountNumber { get; set; } = null!;
        public string DestinationAccountNumber { get; set; } = null!;
        public string DestinationAccountName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Amount { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
