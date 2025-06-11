using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AISDproject.Services;
using AISDproject.Models;
using AISDproject.ViewModel;

namespace AISDproject.Controllers
{
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ITableService _tableService;
        private readonly IStaffService _staffService;
        private readonly IMenuItemService _menuItemService;

        public OrdersController(
            IOrderService orderService,
            ITableService tableService,
            IStaffService staffService,
            IMenuItemService menuItemService)
        {
            _orderService = orderService;
            _tableService = tableService;
            _staffService = staffService;
            _menuItemService = menuItemService;
        }

        // GET: /Orders
        [HttpGet("")]
        public IActionResult RedirectToIndex() => RedirectToAction(nameof(Index));

        // GET: /Orders/Index
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // GET: /Orders/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /Orders/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var vm = new OrderCreateViewModel
            {
                Staffs = (await _orderService.GetStaffsAsync())
                    .Select(s => new SelectListItem(s.FullName, s.StaffId.ToString())),
                Tables = (await _orderService.GetTablesAsync())
                    .Select(t => new SelectListItem(t.TableName, t.TableId.ToString())),
                MenuItems = (await _orderService.GetMenuItemsAsync())
                    .Select(m => new SelectListItem(m.Name, m.MenuItemId.ToString()))
                // PaymentMethods and Statuses provided by ViewModel
            };
            return View(vm);
        }

        // POST: /Orders/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Staffs = (await _orderService.GetStaffsAsync())
                    .Select(s => new SelectListItem(s.FullName, s.StaffId.ToString()));
                vm.Tables = (await _orderService.GetTablesAsync())
                    .Select(t => new SelectListItem(t.TableName, t.TableId.ToString()));
                vm.MenuItems = (await _orderService.GetMenuItemsAsync())
                    .Select(m => new SelectListItem(m.Name, m.MenuItemId.ToString()));
                return View(vm);
            }

            var order = new Order
            {
                StaffId = vm.StaffId,
                TableId = vm.TableId,
                Status = vm.Status
            };

            foreach (var it in vm.Items.Where(i => i.Quantity > 0))
            {
                var menu = (await _orderService.GetMenuItemsAsync()).First(m => m.MenuItemId == it.MenuItemId);
                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = it.MenuItemId,
                    Quantity = it.Quantity,
                    MenuItem = menu
                });
            }

            var payment = new Payment { PaymentMethod = vm.PaymentMethod };
            await _orderService.CreateOrderAsync(order, payment);
            return RedirectToAction(nameof(Index));
        }

            // Lưu ý: Dòng này bị dư, đã xử lý phía trên
        

        // GET: /Orders/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            var vm = new OrderEditViewModel(order);
            // populate dropdowns similar to Create GET
            return View(vm);
        }

        // POST: /Orders/Edit/{id}
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid) return View(vm);

            var order = await _orderService.GetOrderByIdAsync(id);
            // map fields from vm to order
            await _orderService.UpdateOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Orders/Delete/{id}
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /Orders/Delete/{id}
        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}