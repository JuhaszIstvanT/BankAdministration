using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class BankService
    {
        private readonly BankAdministrationContext _context;

        public BankService(BankAdministrationContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Users => _context.Users;
        public IEnumerable<Account> Accounts => _context.Accounts.Include(a => a.User);

        public bool Login(LoginViewModel user)
        {
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            User? u = _context.Users.Include(u => u.Accounts).FirstOrDefault(c => c.UserName == user.UserName);

            if (u == null)
                return false;

            string accNum = u.Accounts.OrderBy(a => a.StartDate).First().AccountNumber;

            if (user.Pin != u.Pin || user.AccountNumber != accNum)
            {
                return false;
            }
            return true;
        }

        public bool LoginByPassword(PasswordViewModel user, Int32? userId)
        {
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            User? u = _context.Users.FirstOrDefault(c => c.Id == userId);

            if (u == null)
                return false;

            if (user.Password != u.Password)
            {
                return false;
            }
            return true;
        }

        public User? GetUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(a => a.UserName == userName);
        }

        public Account? GetAccount(int accountId)
        {
            return _context.Accounts
                .Include(a => a.User)
                .FirstOrDefault(account => account.Id == accountId);
        }

        public Account? GetAccountByAccountNumber(string accountNumber)
        {
            return _context.Accounts
                .Include(a => a.User)
                .FirstOrDefault(account => account.AccountNumber == accountNumber);
        }


        public TransactionViewModel? NewTransaction(int accountId)
        {
            Account? account = _context.Accounts
                .Include(a => a.User)
                .FirstOrDefault(acc => acc.Id == accountId);

            if (account == null)
                return null;

            TransactionViewModel transaction = new TransactionViewModel { Account = account };

            return transaction;
        }

        public bool SaveTransaction(int accountId, TransactionViewModel transaction)
        {
            if (transaction.Account == null || accountId == transaction.Account.Id)
                return false;
            if (!Validator.TryValidateObject(transaction, new ValidationContext(transaction, null, null), null))
                return false;

            Account? sourceAcc = GetAccount(accountId);
            _context.Transactions.Add(new Transaction
            {
                AccountId = transaction.Account.Id,
                Type = "inland",
                SourceAccountNumber = sourceAcc.AccountNumber,
                DestinationAccountNumber = transaction.DestinationAccountNumber,
                DestinationAccountName = transaction.DestinationAccountName,
                Date = DateTime.Now,
                Amount = transaction.Amount
            });

            if (sourceAcc != null)
            {
                sourceAcc.Balance -= transaction.Amount;
            }
            Account? destinationAcc = GetAccountByAccountNumber(transaction.DestinationAccountNumber);
            if (destinationAcc != null)
            {
                destinationAcc.Balance += transaction.Amount;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
