using System.ComponentModel.DataAnnotations;

namespace AISDproject.ViewModel
{
    public class OrderItemViewModel
    {
        [Required]
        public int MenuItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
