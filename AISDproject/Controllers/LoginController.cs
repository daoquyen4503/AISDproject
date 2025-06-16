using Microsoft.AspNetCore.Mvc;
using AISDproject.Models;
using System.Security.Cryptography;
using System.Text;
using AISDproject.Services;

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

            // Hash password
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            var staff = (await _staffService.GetAllStaffAsync())
                        .FirstOrDefault(s => s.Username == model.Username && s.PasswordHash == hashedPassword);

            if (staff != null)
            {
                // Save login info to session
                HttpContext.Session.SetString("Username", staff.Username);
                HttpContext.Session.SetString("Role", staff.Role);

                // Redirect by role
                return staff.Role switch
                {
                    "Admin" => RedirectToAction("Index", "Dashboard"),
                    "Manager" => RedirectToAction("Index", "Dashboard"),
                    "Waiter" => RedirectToAction("Index", "Orders"),
                    "Cashier" => RedirectToAction("Index", "Payment"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }
    }
}
