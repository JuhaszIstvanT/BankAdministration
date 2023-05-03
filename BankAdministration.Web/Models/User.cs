using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BankAdministration.Web.Models
{
    public class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
        }
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Pin { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
