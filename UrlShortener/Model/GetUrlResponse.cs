using UrlShortener.Helpers;

namespace UrlShortener.Model
{
    /// <summary>
    /// Url Response
    /// </summary>
    public class GetUrlResponse
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Creates the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
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