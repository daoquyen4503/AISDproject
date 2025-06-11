using AISDproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace AISDproject.ViewModel
{
    public class OrderCreateViewModel
    {
        [Display(Name = "Thời gian đặt")]
        [Required]
        public DateTime OrderTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Phải chọn nhân viên")]
        public int StaffId { get; set; }

        [Required(ErrorMessage = "Phải chọn bàn")]
        public int TableId { get; set; }

        public IEnumerable<SelectListItem> Staffs { get; set; }
        public IEnumerable<SelectListItem> Tables { get; set; }
        public IEnumerable<SelectListItem> MenuItems { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Phải chọn ít nhất một món")]
        public List<OrderItemInput> Items { get; set; } = new() { new OrderItemInput() };

        [Required(ErrorMessage = "Phải chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public PaymentMethod PaymentMethod { get; set; }

        public IEnumerable<SelectListItem> PaymentMethods =>
            Enum.GetValues(typeof(PaymentMethod))
                .Cast<PaymentMethod>()
                .Select(m => new SelectListItem(m.ToString(), m.ToString()));

        [Required(ErrorMessage = "Phải chọn trạng thái đơn hàng")]
        [Display(Name = "Trạng thái")]
        public OrderStatus Status { get; set; }

        public IEnumerable<SelectListItem> Statuses =>
            Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => new SelectListItem(s.ToString(), s.ToString()));
    }

    public class OrderItemInput
    {
        [Required(ErrorMessage = "Phải chọn món ăn")]
        [Range(1, int.MaxValue, ErrorMessage = "Phải chọn món ăn")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Phải nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
    // ViewModel cho Edit Order
    public class OrderEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải chọn nhân viên")]
        public int StaffId { get; set; }

        [Required(ErrorMessage = "Phải chọn bàn")]
        public int TableId { get; set; }

        public IEnumerable<SelectListItem> Staffs { get; set; }
        public IEnumerable<SelectListItem> Tables { get; set; }
        public List<OrderItemInput> Items { get; set; }

        [Required(ErrorMessage = "Phải chọn phương thức thanh toán")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Phải chọn trạng thái đơn hàng")]
        public OrderStatus Status { get; set; }

        public OrderEditViewModel() { }

        // Constructor từ model Order
        public OrderEditViewModel(Order order)
        {
            Id = order.OrderId;
            StaffId = order.StaffId;
            TableId = order.TableId;
            Items = order.OrderItems.Select(oi => new OrderItemInput
            {
                MenuItemId = oi.MenuItemId,
                Quantity = oi.Quantity
            }).ToList();
            PaymentMethod = order.Payment.PaymentMethod;
            Status = order.Status;
        }
    }

}

