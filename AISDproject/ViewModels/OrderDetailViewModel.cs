// AISDproject/ViewModels/OrderDetailViewModel.cs
using System;
using System.Collections.Generic;
using AISDproject.Models;

namespace AISDproject.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public string TableName { get; set; }
        public string StaffName { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderItemDetailViewModel> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

    }

    public class OrderItemDetailViewModel
    {
        public string MenuItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;

    }
}
