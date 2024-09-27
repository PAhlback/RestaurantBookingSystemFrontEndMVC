using System.Net;

namespace RestaurantBookingSystemMVC.Helpers
{
    public class CustomHttpClientHandler : HttpClientHandler
    {
        private static readonly CookieContainer _cookieContainer = new CookieContainer();

        public CustomHttpClientHandler(CookieContainer cookieContainer)
        {
            this.UseCookies = true;
            this.CookieContainer = _cookieContainer;
        }
        public static IEnumerable<string> GetCookies(string uri)
        {
            return _cookieContainer.GetCookies(new Uri(uri)).Cast<Cookie>().Select(c => $"{c.Name}={c.Value}");
        }
    }
}
