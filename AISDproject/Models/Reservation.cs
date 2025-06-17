using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISDproject.Models
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Reservation date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Reservation Date")]
        [Range(typeof(DateTime), "2024-01-01", "2099-12-31", ErrorMessage = "Date must be in the future")]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Please select a table")]
        [Display(Name = "Table")]
        public int TableId { get; set; }

        public Table? Table { get; set; }  // Navigation property

        [Required(ErrorMessage = "Please select a time slot")]
        [Display(Name = "Time Slot")]
        public int TimeSlotId { get; set; }

        public TimeSlot? TimeSlot { get; set; }  // Navigation property

        [Display(Name = "Reservation Status")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}
