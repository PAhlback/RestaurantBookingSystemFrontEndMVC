using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystemMVC.Models.Reservation
{
    public class NewReservationDTO
    {
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public DateTime DateAndTime { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
    }
}
