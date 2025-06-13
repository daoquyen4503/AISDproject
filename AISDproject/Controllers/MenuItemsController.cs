using AISDproject.Models;
using AISDproject.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AISDproject.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _service;

        public MenuItemsController(IMenuItemService service)
        {
            _service = service;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem item)
        {
            if (!ModelState.IsValid)
                return View(item);

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

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItem item)
        {
            if (!ModelState.IsValid)
                return View(item);

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

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

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
