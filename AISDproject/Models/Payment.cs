using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public enum PaymentMethod { Cash, Card, Online }
    public enum PaymentStatus { Pending, Paid, Failed }

    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        // Foreign key
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // ✅ Thêm các thuộc tính sau
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime? PaymentTime { get; set; }
    }

}