using UrlShortener.Helpers;

namespace UrlShortener.Model
{
    public class GetUrlResponse
    {
        public string Url { get; set; }

        public static GetUrlResponse Create(string url)
        {
            Ensure.NotNullOrEmpty(url, "url can't be null");

            GetUrlResponse getUrlResponse = new GetUrlResponse
            {
                Url = url
            };

            return getUrlResponse;
        }
    }
}