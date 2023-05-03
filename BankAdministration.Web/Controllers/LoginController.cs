using BankAdministration.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankAdministration.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly BankService _bankService;
        public LoginController(BankService bankService, IHttpContextAccessor contextAccessor)
        {
            _bankService = bankService;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);
            if (!_bankService.Login(user))
            {
                ModelState.AddModelError("", "Hibás felhasználónév, jelszó, pin vagy számlaszám.");
                return View("Login", user);
            }

            User? u = _bankService.GetUserByUserName(user.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.Name, u.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await _contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            HttpContext.Session.SetString("user", user.UserName);
            HttpContext.Session.SetInt32("userId", u.Id);
            HttpContext.Session.SetString("redirected", "false");
            HttpContext.Session.SetString("safemode", "off");

            return RedirectToAction("Index", "Account", new { userId = u.Id });
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("user") != null)
                HttpContext.Session.Remove("user");
            if (HttpContext.Session.GetString("userId") != null)
                HttpContext.Session.Remove("userId");

            return RedirectToAction("Index", "Login");
        }

        public IActionResult SafeMode()
        {
            HttpContext.Session.SetString("safemode", "on");

            return RedirectToAction("Index", "Account", new { userId = HttpContext.Session.GetInt32("userId") });
        }
    }
}
