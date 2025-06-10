using System.ComponentModel.DataAnnotations;

namespace AISDproject.Models
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
