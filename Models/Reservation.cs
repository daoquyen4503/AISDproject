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
        [DataType(DataType.Date)]
        [Display(Name = "Reservation Date")]
        [Range(typeof(DateTime), "2024-01-01", "2099-12-31", ErrorMessage = "Date must be in the future")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Please select a table")]
        public int TableId { get; set; }

        // ✅ Navigation property để nullable để không bị validate
        public Table? Table { get; set; }

        [Required(ErrorMessage = "Please select a time slot")]
        public int TimeSlotId { get; set; }

        // ✅ Navigation property nullable
        public TimeSlot? TimeSlot { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}
