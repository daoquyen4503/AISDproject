using System.ComponentModel.DataAnnotations;

namespace AISDproject.Models
{
    public class Table
    {
        public enum TableStatus
        {
            Available,
            Unavailable,
        }

        [Key]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Tên bàn không được để trống")]
        public string TableName { get; set; }

        [Range(1, 100, ErrorMessage = "Sức chứa phải lớn hơn 0")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Trạng thái bàn là bắt buộc")]
        public TableStatus Status { get; set; } = TableStatus.Available;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
