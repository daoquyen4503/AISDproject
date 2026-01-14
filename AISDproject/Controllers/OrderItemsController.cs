using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AISDproject.Data;
using AISDproject.Models;
using AISDproject.Filters; // ✅ Bổ sung

namespace AISDproject.Controllers
{
    [AuthorizeRole("Waiter", "Cashier")] // ✅ Chỉ waiter có toàn quyền
    [Route("OrderItems")]
    public class OrderItemsController : Controller
    {
        private readonly RestaurantContext _context;

        public OrderItemsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: /OrderItems
        [HttpGet("")]
        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: /OrderItems/Index
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var data = _context.OrderItems
                .Include(o => o.MenuItem)
                .Include(o => o.Order);
            return View(await data.ToListAsync());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _context.OrderItems
                .Include(o => o.MenuItem)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderItem == null) return NotFound();
            return View(orderItem);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var menuItems = _context.MenuItems.ToList();
            var orders = _context.Orders.ToList();

            ViewData["MenuItemId"] = new SelectList(menuItems, "MenuItemId", "Name");
            ViewData["OrderId"] = new SelectList(orders, "OrderId", "OrderId");

            ViewBag.MenuItems = menuItems;

            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,MenuItemId,Quantity")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                var menuItem = await _context.MenuItems.FindAsync(orderItem.MenuItemId);
                if (menuItem != null)
                {
                
                 orderItem.MenuItem = menuItem; // 🔧 Bổ sung gán
                    

                }

                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            ViewBag.MenuItems = _context.MenuItems.ToList();
            return View(orderItem);
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            return View(orderItem);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,MenuItemId,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var menuItem = await _context.MenuItems.FindAsync(orderItem.MenuItemId);
                    if (menuItem != null)
                    {
                        orderItem.MenuItem = menuItem;

                    }

                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "MenuItemId", "Name", orderItem.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            return View(orderItem);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _context.OrderItems
                .Include(o => o.MenuItem)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderItem == null) return NotFound();
            return View(orderItem);
        }

        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null) _context.OrderItems.Remove(orderItem);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
