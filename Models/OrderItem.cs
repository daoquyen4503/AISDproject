using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        [Display(Name = "Món ăn")]
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal LineTotal => MenuItem != null ? Quantity * MenuItem.Price : 0;

        [NotMapped]
        public decimal Subtotal => LineTotal;
    }
}
