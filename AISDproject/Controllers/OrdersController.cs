using AISDproject.Models;
using AISDproject.Services;
using AISDproject.ViewModel;
using AISDproject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            var tables = await _orderService.GetTablesAsync();
            var staffs = await _orderService.GetStaffsAsync();
            var menuItems = await _orderService.GetMenuItemsAsync();

            var viewModel = new OrderCreateViewModel
            {
                TableList = tables.Select(t => new SelectListItem
                {
                    Value = t.TableId.ToString(),
                    Text = t.TableName
                }).ToList(),

                StaffList = staffs.Select(s => new SelectListItem
                {
                    Value = s.StaffId.ToString(),
                    Text = s.FullName
                }).ToList(),

                MenuItemList = menuItems.Select(m => new SelectListItem
                {
                    Value = m.MenuItemId.ToString(),
                    Text = m.Name
                }).ToList(),

                MenuItems = menuItems.ToList(),
                Items = new List<OrderItemViewModel> { new() }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid || model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "Bạn phải chọn ít nhất một món ăn.");

                var tables = await _orderService.GetTablesAsync();
                var staffs = await _orderService.GetStaffsAsync();
                var menuItems = await _orderService.GetMenuItemsAsync();

                model.TableList = tables.Select(t => new SelectListItem
                {
                    Value = t.TableId.ToString(),
                    Text = t.TableName
                }).ToList();

                model.StaffList = staffs.Select(s => new SelectListItem
                {
                    Value = s.StaffId.ToString(),
                    Text = s.FullName
                }).ToList();

                model.MenuItemList = menuItems.Select(m => new SelectListItem
                {
                    Value = m.MenuItemId.ToString(),
                    Text = m.Name
                }).ToList();

                model.MenuItems = menuItems.ToList();

                return View(model);
            }

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TableId = model.TableId,
                StaffId = model.StaffId,
                Status = OrderStatus.Pending,
                OrderItems = model.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                PaymentMethod = PaymentMethod.Cash // default
            };

            await _orderService.CreateOrderAsync(order, payment);

            return RedirectToAction(nameof(Index));
        }
    }
}
