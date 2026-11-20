using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BillPaymentSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BillPaymentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly PasswordHasher<string> _hasher = new();

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!KimlikDogru(model.Username, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            var kimlik = new ClaimsIdentity(
                [new Claim(ClaimTypes.Name, model.Username)],
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(kimlik));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AdminPanel", "Baslangic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private bool KimlikDogru(string kullaniciAdi, string sifre)
        {
            var beklenenKullanici = _config["AdminAuth:Username"];
            var hash = _config["AdminAuth:PasswordHash"];
            if (string.IsNullOrEmpty(beklenenKullanici) || string.IsNullOrEmpty(hash))
            {
                return false;
            }

            // Kullanıcı adı yanlış olsa da hash doğrulaması çalışsın, yanıt süresi bilgi sızdırmasın.
            var sifreDogru = _hasher.VerifyHashedPassword(string.Empty, hash, sifre)
                != PasswordVerificationResult.Failed;
            var kullaniciDogru = CryptographicOperations.FixedTimeEquals(
                SHA256.HashData(Encoding.UTF8.GetBytes(kullaniciAdi)),
                SHA256.HashData(Encoding.UTF8.GetBytes(beklenenKullanici)));
            return sifreDogru && kullaniciDogru;
        }
    }
}
