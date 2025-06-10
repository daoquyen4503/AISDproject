using System.ComponentModel.DataAnnotations;

namespace AISDproject.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }

        public string TableName { get; set; }

        public int Capacity { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
