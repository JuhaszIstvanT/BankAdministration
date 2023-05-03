using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Models
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            BankAdministrationContext context = serviceProvider.GetRequiredService<BankAdministrationContext>();

            context.Database.EnsureCreated();

            if (context.Transactions.Any())
            {
                return;
            }

            
            var users = new User[]
{
                new User {
                    FullName = "Gáspár Győzi",
                    UserName = "Győzike",
                    Password = "000",
                    Pin = "0000"
                },
                new User {
                    FullName = "Majoros Péter",
                    UserName = "majka",
                    Password= "111",
                    Pin = "1111"
                }
};
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
            
            var accounts = new Account[]
            {
                new Account
                {
                    UserId = 1,
                    Name = "gaspar",
                    AccountNumber = "1414141",
                    Balance = 50000,
                    StartDate = DateTime.Now.AddDays(3)
                },
                new Account
                {
                    UserId = 1,
                    Name = "gyozo",
                    AccountNumber = "1414242",
                    Balance = 70000,
                    StartDate = DateTime.Now.AddDays(4)
                },
                new Account
                {
                    UserId = 2,
                    Name = "peti",
                    AccountNumber = "142424433",
                    Balance = 10000,
                    StartDate = DateTime.Now.AddDays(1)
                }
            };
            foreach (Account a in accounts)
            {
                context.Accounts.Add(a);
            }
            context.SaveChanges();

            var transactions = new Transaction[]
            {
                new Transaction
                {
                    AccountId = 1,
                    Type = "inland",
                    SourceAccountNumber = "5525252",
                    DestinationAccountNumber = "4151666",
                    DestinationAccountName = "maja",
                    Date = DateTime.Now.AddDays(6),
                    Amount = 10000
                },
                new Transaction
                {
                    AccountId = 1,
                    Type = "inland",
                    SourceAccountNumber = "552553",
                    DestinationAccountNumber = "2351666",
                    DestinationAccountName = "diszno",
                    Date = DateTime.Now.AddDays(10),
                    Amount = 20000
                }
            };
            foreach (Transaction t in transactions)
            {
                context.Transactions.Add(t);
            }
            context.SaveChanges();
        }
    }
}
