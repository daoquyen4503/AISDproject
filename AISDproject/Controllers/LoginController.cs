using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AISDproject.Models;
using AISDproject.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AISDproject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IStaffService _staffService;

        public LoginController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Hash password để so khớp
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            var staff = (await _staffService.GetAllStaffAsync())
                        .FirstOrDefault(s => s.Username == model.Username && s.PasswordHash == hashedPassword);

            if (staff != null)
            {
                // Tạo Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, staff.Username),
                    new Claim(ClaimTypes.Role, staff.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect theo vai trò
                switch (staff.Role)
                {
                    case "Admin":
                    case "Manager":
                        return RedirectToAction("Index", "Dashboard");

                    case "Waiter":
                    case "Cashier":
                        return RedirectToAction("Index", "Orders");

                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
