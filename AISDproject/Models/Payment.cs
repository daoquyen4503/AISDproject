using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public enum PaymentMethod { Cash, Card, Online }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Display(Name = "Ngày thanh toán")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Display(Name = "Phương thức thanh toán")]
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Số tiền thanh toán")]
        public decimal Amount { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}