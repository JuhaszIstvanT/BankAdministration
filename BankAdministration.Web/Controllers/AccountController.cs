using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private BankAdministrationContext _context;
        public AccountController(BankAdministrationContext context)
        {
            _context = context;
        }

        public IActionResult Index(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
                return NotFound();

            return View("Index", _context.Accounts
                .Include(a => a.User)
                .Where(a => a.UserId == userId));
        }

        public IActionResult Details(int accountId)
        {
            String? redirected = HttpContext.Session.GetString("redirected");

            if (HttpContext.Session.GetString("safemode") == "on" && redirected == "false")
            {
                HttpContext.Session.SetInt32("accountId", accountId);
                HttpContext.Session.SetString("currentpage", "account");
                return RedirectToAction("Password", "Password");
            } else if ((HttpContext.Session.GetString("safemode") == "on" && redirected == "true") ||
                HttpContext.Session.GetString("safemode") == "off") {
                HttpContext.Session.SetString("redirected", "false");
                Account? account = _context.Accounts
                    .Include(a => a.User)
                    .Include(a => a.Transactions)
                    .FirstOrDefault(a => a.Id == accountId);

                if (account == null)
                    return NotFound();

                ViewBag.Title = $"Számla tranzakciói: {account.Name} ({account.User.FullName})";
                return View("Details", account);
            }
            return RedirectToAction("Password", "Password");
        }
    }
}
