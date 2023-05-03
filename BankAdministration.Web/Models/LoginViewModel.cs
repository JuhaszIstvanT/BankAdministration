using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "A számlaszám megadása kötelező.")]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = "A Pin kód megadása kötelező.")]
        public string Pin { get; set; } = null!;
    }
}
