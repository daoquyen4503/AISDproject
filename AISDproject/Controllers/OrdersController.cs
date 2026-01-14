using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AISDproject.Models;
using AISDproject.Services;
using AISDproject.ViewModels;
using AISDproject.Data;
using AISDproject.Filters;

namespace AISDproject.Controllers
{
    [AuthorizeRole("Admin", "Waiter", "Cashier")] // ✅ Cho phép cả 3 vai trò

    //[Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly RestaurantContext _context;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger, RestaurantContext context)
        {
            _orderService = orderService;
            _logger = logger;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var vm = new List<OrderDetailViewModel>();

            foreach (var o in orders)
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == o.OrderId);

                vm.Add(new OrderDetailViewModel
                {
                    OrderId = o.OrderId,
                    OrderTime = o.OrderTime,
                    TableName = o.Table?.TableName,
                    StaffName = o.Staff?.FullName,
                    Status = o.Status,
                    Items = o.OrderItems.Select(i => new OrderItemDetailViewModel
                    {
                        MenuItemName = i.MenuItem?.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.MenuItem?.Price ?? 0
                    }).ToList(),
                    TotalAmount = o.OrderItems.Sum(i => i.Quantity * (i.MenuItem?.Price ?? 0)),
                    PaymentStatus = payment?.Status ?? PaymentStatus.Pending
                });
            }

            return View(vm);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            var viewModel = new OrderDetailViewModel
            {
                OrderId = order.OrderId,
                TableName = order.Table?.TableName,
                StaffName = order.Staff?.FullName,
                OrderTime = order.OrderTime,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentStatus = order.Payment?.Status ?? PaymentStatus.Pending, // ⚠️ Gán từ order.Payment
                Items = order.OrderItems.Select(oi => new OrderItemDetailViewModel
                {
                    MenuItemName = oi.MenuItem?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.MenuItem?.Price ?? 0
                }).ToList()

            };

            return View(viewModel);
        }



        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            var vm = new OrderCreateViewModel
            {
                Items = new List<OrderItemViewModel> { new() }
            };
            await PopulateDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (model.Items == null || !model.Items.Any(i => i.MenuItemId > 0 && i.Quantity > 0))
                ModelState.AddModelError("", "Bạn phải chọn ít nhất một món ăn hợp lệ.");

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View(model);
            }

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TableId = model.TableId,
                StaffId = model.StaffId,
                Status = OrderStatus.Pending,
                OrderItems = model.Items
                    .Where(i => i.MenuItemId > 0 && i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        MenuItemId = i.MenuItemId,
                        Quantity = i.Quantity
                    }).ToList()
            };

            try
            {
                await _orderService.CreateOrderAsync(order); // ✅ Không tạo Payment ở đây
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var detailed = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Error creating order: {Detail}", detailed);
                ModelState.AddModelError(string.Empty, "Đã có lỗi khi tạo đơn hàng: " + detailed);
                await PopulateDropdowns(model);
                return View(model);
            }
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            var vm = new OrderEditViewModel
            {
                OrderId = order.OrderId,
                TableId = order.TableId,
                StaffId = order.StaffId,
                Status = order.Status,
                Items = order.OrderItems
                    .Select(oi => new OrderItemViewModel
                    {
                        MenuItemId = oi.MenuItemId,
                        Quantity = oi.Quantity
                    }).ToList()
            };
            await PopulateDropdowns(vm);
            return View(vm);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderEditViewModel model)
        {
            if (id != model.OrderId) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View(model);
            }

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                order.TableId = model.TableId;
                order.StaffId = model.StaffId;
                order.Status = model.Status;
                order.OrderItems.Clear();
                order.OrderItems = model.Items
                    .Where(i => i.MenuItemId > 0 && i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        MenuItemId = i.MenuItemId,
                        Quantity = i.Quantity
                    }).ToList();

                await _orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing order");
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdowns(model);
                return View(model);
            }
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PopulateDropdowns<T>(T model) where T : class
        {
            var tables = await _orderService.GetTablesAsync();
            var staffs = await _orderService.GetStaffsAsync();
            var menuItems = await _orderService.GetMenuItemsAsync();

            dynamic vm = model;
            vm.TableList = tables.Select(t => new SelectListItem(t.TableName, t.TableId.ToString())).ToList();
            vm.StaffList = staffs.Select(s => new SelectListItem(s.FullName, s.StaffId.ToString())).ToList();
            vm.MenuItemList = menuItems.Select(m => new SelectListItem(m.Name, m.MenuItemId.ToString())).ToList();
            if (vm is OrderCreateViewModel)
                vm.MenuItems = menuItems.ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Staff)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            var totalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.MenuItem.Price);

            ViewBag.TotalAmount = totalAmount;
            ViewBag.OrderId = order.OrderId;
            ViewBag.TableName = order.Table.TableName;

            return View(new CheckoutViewModel { OrderId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.OrderId == model.OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                var payment = new Payment
                {
                    OrderId = model.OrderId,
                    Amount = model.Amount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentDate = DateTime.Now,
                    PaymentTime = DateTime.Now,
                    Status = PaymentStatus.Paid
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = model.OrderId });
            }

            return View(model);
        }


    }
}
