namespace RestaurantBookingSystemMVC.Models.Reservation
{
    public class UpdateReservationDTO
    {
        public int NumberOfGuests { get; set; }
        public DateTime DateAndTime { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int TableId { get; set; }
        public int TableNumber { get; set; }
    }
}
