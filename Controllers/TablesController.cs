using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AISDproject.Models;
using AISDproject.Services;

namespace AISDproject.Controllers
{
    public class TablesController : Controller
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // GET: Tables
        public async Task<IActionResult> Index()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return View(tables);
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
                return NotFound();
            return View(table);
        }

        // GET: Tables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Table table)
        {
            if (ModelState.IsValid)
            {
                await _tableService.CreateTableAsync(table);
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // GET: Tables/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
                return NotFound();
            return View(table);
        }

        // POST: Tables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Table table)
        {
            if (id != table.TableId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                await _tableService.UpdateTableAsync(table);
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null)
                return NotFound();
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tableService.DeleteTableAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
