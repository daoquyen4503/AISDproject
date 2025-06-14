using System.ComponentModel.DataAnnotations;
using AISDproject.Models;

namespace AISDproject.ViewModels
{
    public class CheckoutViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Số tiền thanh toán")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn hoặc bằng 0")]
        public decimal Amount { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
