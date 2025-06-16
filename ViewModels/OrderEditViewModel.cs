// AISDproject/ViewModels/OrderEditViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.ViewModels
{
    public class OrderEditViewModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn bàn.")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn nhân viên.")]
        public int StaffId { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn trạng thái đơn hàng.")]
        public OrderStatus Status { get; set; }

        // Dropdown lists
        public List<SelectListItem> TableList { get; set; } = new();
        public List<SelectListItem> StaffList { get; set; } = new();
        public List<SelectListItem> MenuItemList { get; set; } = new();

        // Dùng để truy xuất đơn giá món ăn
        public List<MenuItem> MenuItems { get; set; } = new();

        // Order items
        [MinLength(1, ErrorMessage = "Bạn phải chọn ít nhất một món ăn.")]
        public List<OrderItemViewModel> Items { get; set; } = new();

        // Tổng tiền đơn hàng (tùy chọn nếu dùng server-side)
        public decimal TotalAmount { get; set; }
    }
}
