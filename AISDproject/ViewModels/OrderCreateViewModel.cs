// AISDproject/ViewModels/OrderCreateViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AISDproject.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required(ErrorMessage = "Bạn phải chọn bàn.")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn nhân viên.")]
        public int StaffId { get; set; }

        // Dropdown lists
        public List<SelectListItem> TableList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StaffList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MenuItemList { get; set; } = new List<SelectListItem>();

        // Data for client-side price
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        // Order items: at least one
        [MinLength(1, ErrorMessage = "Bạn phải chọn ít nhất một món ăn.")]
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel> { new() };

        // Total amount calculated on server
        public decimal TotalAmount { get; set; }
    }
}