using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Models
{
    public class BankAdministrationContext : DbContext
    {
        public BankAdministrationContext(DbContextOptions<BankAdministrationContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
