using System.ComponentModel.DataAnnotations;

namespace AISDproject.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
