using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AISDproject.Models;
using AISDproject.Services;
using AISDproject.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace AISDproject.Controllers
{
    [AuthorizeRole("Admin", "Manager")]
    public class StaffsController : Controller
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Staff model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var staffList = await _staffService.GetAllStaffAsync();
            var staff = staffList.FirstOrDefault(s =>
                s.Username == model.Username &&
                s.PasswordHash == HashPassword(model.PasswordHash));

            if (staff != null)
            {
                HttpContext.Session.SetString("StaffUsername", staff.Username);
                HttpContext.Session.SetString("Role", staff.Role);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ====================== VISIBLE TO BOTH Admin and Manager ======================
        public async Task<IActionResult> Index()
        {
            var staffList = await _staffService.GetAllStaffAsync();
            return View(staffList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
                return NotFound();
            return View(staff);
        }

        // ====================== Admin-only Actions ======================
        [AuthorizeRole("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeRole("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                staff.PasswordHash = HashPassword(staff.PasswordHash);
                await _staffService.CreateStaffAsync(staff);
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        [AuthorizeRole("Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
                return NotFound();
            return View(staff);
        }

        [HttpPost]
        [AuthorizeRole("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
        {
            if (id != staff.StaffId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                staff.PasswordHash = HashPassword(staff.PasswordHash);
                await _staffService.UpdateStaffAsync(staff);
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        [AuthorizeRole("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
                return NotFound();
            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        [AuthorizeRole("Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _staffService.DeleteStaffAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
