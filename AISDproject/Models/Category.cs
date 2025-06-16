using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AISDproject.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [ValidateNever]
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
