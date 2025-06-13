using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AISDproject.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Tên món ăn không được để trống")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Giá món ăn không được để trống")]
        [Range(0.01, 1000000, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool Status { get; set; } = true; // true = Đang bán, false = Ngừng bán

        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
