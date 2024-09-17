using System.Text.Json;

namespace RestaurantBookingSystemMVC.Helpers
{
    public class JsonHelper
    {
        public static JsonSerializerOptions JsonOptionsHelper()
        {
            return new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true,
            };
        }
    }
}
