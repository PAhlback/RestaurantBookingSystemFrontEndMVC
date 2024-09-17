namespace RestaurantBookingSystemMVC.Models.Reservation
{
    public class Reservation
    {
        // Comes from API /reservations endpoint - is a viewmodel from the API.
        public int Id { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime DateAndTime { get; set; }
        public ReservationCustomer Customer { get; set; }
        public ReservationTable Table { get; set; }
    }
}
