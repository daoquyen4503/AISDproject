using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderTime { get; set; } = DateTime.Now;

        [ForeignKey("Table")]
        public int TableId { get; set; }
        public Table Table { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }
        public Staff Staff { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Payment? Payment { get; set; }

        public string Status { get; set; } // Pending, Completed, Cancelled
    }
}
