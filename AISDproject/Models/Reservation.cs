using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public enum ReservationStatus { Pending, Confirmed, Cancelled }

    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [ForeignKey("Table")]
        public int TableId { get; set; }
        public Table Table { get; set; }

        [ForeignKey("TimeSlot")]
        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}
