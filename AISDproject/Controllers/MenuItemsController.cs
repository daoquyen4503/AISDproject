using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AISDproject.Services;
using AISDproject.Models;

namespace AISDproject.Controllers
{
    [Route("MenuItems")]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _service;

        public MenuItemsController(IMenuItemService service)
        {
            _service = service;
        }

        // Redirect nếu chỉ vào /MenuItems
        [HttpGet("")]
        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuItem item)
        {
            if (!ModelState.IsValid) return View(item);
            await _service.AddAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(MenuItem item)
        {
            if (!ModelState.IsValid) return View(item);
            await _service.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
