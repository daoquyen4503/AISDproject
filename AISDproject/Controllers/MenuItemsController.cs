using AISDproject.Models;
using AISDproject.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using AISDproject.Filters; // Thêm namespace cho filter
using Microsoft.AspNetCore.Mvc.Rendering; // để dùng SelectList
using AISDproject.Data; // thêm dòng này để dùng RestaurantContext

namespace AISDproject.Controllers
{
    
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _service;
        private readonly RestaurantContext _context; // thêm dòng này

        public MenuItemsController(IMenuItemService service, RestaurantContext context) // sửa constructor
        {
            _service = service;
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        [AuthorizeRole("Admin", "Manager")]
        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        [AuthorizeRole("Admin", "Manager")]
        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", item.CategoryId);
                return View(item);
            }

            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(item.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(stream);
                }

                item.ImageUrl = "/images/menu/" + uniqueFileName;
            }

            await _service.AddAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRole("Admin", "Manager")]
        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", item.CategoryId);
            return View(item);
        }

        [AuthorizeRole("Admin", "Manager")]
        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", item.CategoryId);
                return View(item);
            }

            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "menu");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(item.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(stream);
                }

                item.ImageUrl = "/images/menu/" + uniqueFileName;
            }

            await _service.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRole("Admin", "Manager")]
        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [AuthorizeRole("Admin", "Manager")]
        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [AuthorizeRole("Admin", "Manager")]
        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}