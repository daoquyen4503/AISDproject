using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AISDproject.Models;
using AISDproject.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.ViewModels
{
    public class OrderCreateViewModel
    {
        public int TableId { get; set; }
        public int StaffId { get; set; }

        public required List<SelectListItem> TableList { get; set; }
        public required List<SelectListItem> StaffList { get; set; }
        public required List<SelectListItem> MenuItemList { get; set; }

        public required List<MenuItem> MenuItems { get; set; } // 👈 cần thêm dòng này để gửi dữ liệu giá

        public required List<OrderItemViewModel> Items { get; set; }

        public decimal TotalAmount { get; set; } // Tổng tiền được submit từ view

    }

}
