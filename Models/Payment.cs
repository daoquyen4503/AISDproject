using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public enum PaymentMethod { Cash, Card, Online }
    public enum PaymentStatus { Pending, Paid, Failed }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        // Ngày tạo giao dịch (dự kiến tạo khi order hoàn tất)
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // Số tiền phải thanh toán
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Hình thức thanh toán (tiền mặt, thẻ, online)
        public PaymentMethod PaymentMethod { get; set; }

        // Khóa ngoại liên kết đến đơn hàng
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        // Trạng thái giao dịch (đang chờ, đã thanh toán, thất bại)
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Thời điểm hoàn tất thanh toán (nếu thành công)
        public DateTime? PaymentTime { get; set; }
    }


}