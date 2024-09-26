namespace RestaurantBookingSystemMVC.Models.MenuItem.DTOs
{
    public class MenuItemDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryFK { get; set; }

        // TODO: Implement IsPopular
    }
}
