using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankAdministration.Web.Controllers
{
    public class PasswordController : Controller
    {
        private readonly BankService _bankService;
        public PasswordController(BankService bankService)
        {
            _bankService = bankService;
        }

        public IActionResult Index()
        {
            return View("Password");
        }

        [HttpGet]
        public IActionResult Password()
        {
            return View("Password");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(PasswordViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Password", user);
            Int32? uId = HttpContext.Session.GetInt32("userId");
            if (!_bankService.LoginByPassword(user, uId))
            {
                ModelState.AddModelError("", "Hibás jelszó.");
                return View("Password", user);
            }

            Int32? accId = HttpContext.Session.GetInt32("accountId");
            HttpContext.Session.SetString("redirected", "true");
            switch (HttpContext.Session.GetString("currentpage"))
            {
                case "account":
                    return RedirectToAction("Details", "Account", new { accountId = accId });

                case "transaction":
                    return RedirectToAction("Index", "Transaction", new { accountId = accId });
            }
            return RedirectToAction("Index", "Account");
        }
    }
}
