using System.ComponentModel.DataAnnotations;

namespace AISDproject.ViewModels
{
    public class OrderItemViewModel
    {
        [Required(ErrorMessage = "Bạn phải chọn món ăn.")]
        public int MenuItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        public string? MenuItemName { get; internal set; }
        public decimal UnitPrice { get; internal set; }
        public decimal LineTotal { get; internal set; }
    }
}