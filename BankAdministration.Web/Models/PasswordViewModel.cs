using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
