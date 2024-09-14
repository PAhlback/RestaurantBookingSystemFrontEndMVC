namespace RestaurantBookingSystemMVC.Models.MenuItem
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public bool IsAvailable { get; set; }

        public MenuItemCategoryNoItemsViewModel? Category { get; set; }

        public bool IsPopular { get; set; }
    }
}
