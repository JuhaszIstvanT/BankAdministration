using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class TransactionViewModel
    {
        [Required(ErrorMessage = "Az összeg megadása kötelező.")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "A célszámla név megadása kötelező.")]
        public String DestinationAccountName { get; set; } = null!;
        [Required(ErrorMessage = "A célszámla szám megadása kötelező.")]
        public String DestinationAccountNumber { get; set; } = null!;
        public Account? Account { get; set; }
    }
}
