using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AISDproject.Models
{
    public enum OrderStatus
    {
        Pending,      // Chờ xử lý
        Serving,      // Đang phục vụ
        Completed,    // Hoàn thành
        Cancelled     // Hủy
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "Thời gian đặt")]
        public DateTime OrderTime { get => CreatedAt; set => CreatedAt = value; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Nhân viên")]
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        [Required]
        [Display(Name = "Bàn")]
        public int TableId { get; set; }
        public Table Table { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [NotMapped]
        public decimal SubTotal => OrderItems.Sum(i => i.LineTotal);

        public Payment Payment { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}