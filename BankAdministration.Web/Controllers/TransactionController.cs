using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankService _bankService;

        public TransactionController(BankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public IActionResult Index(int accountId)
        {
            String? redirected = HttpContext.Session.GetString("redirected");

            if (HttpContext.Session.GetString("safemode") == "on" && redirected == "false")
            {
                HttpContext.Session.SetInt32("accountId", accountId);
                HttpContext.Session.SetString("currentpage", "transaction");
                return RedirectToAction("Password", "Password");
            }
            else if ((HttpContext.Session.GetString("safemode") == "on" && redirected == "true") ||
                HttpContext.Session.GetString("safemode") == "off")
            {
                HttpContext.Session.SetString("redirected", "false");
                TransactionViewModel? transaction = _bankService.NewTransaction(accountId);

                if (transaction == null)
                    return RedirectToAction("Index", "Account");

                return View("Index", transaction);
            }
            return RedirectToAction("Password", "Password");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int accountId, TransactionViewModel transaction)
        {
            transaction.Account = _bankService.GetAccount(accountId);

            if (transaction.Account == null)
                return RedirectToAction("Index", "Home");

            if (transaction.Amount > transaction.Account.Balance)
            {
                ModelState.AddModelError("TransactionAmount", "A megadott számlán nincs elég pénz!");
            }

            if (!ModelState.IsValid)
                return View("Index", transaction);

            if (!_bankService.SaveTransaction(accountId, transaction))
            {
                ModelState.AddModelError("", "A tranzakció sikertelen, próbálkozzon újra!");
                return View("Index", transaction);
            }

            ViewBag.Message = "A tranzakció sikeres volt!";
            return View("Result", transaction);
        }
    }
}
