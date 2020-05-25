using UrlShortener.Helpers;

namespace UrlShortener.Model
{
    public class CreateUrlResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the short URL.
        /// </summary>
        /// <value>
        /// The short URL.
        /// </value>
        public string ShortUrl { get; set; }

        /// <summary>
        /// Gets or sets the original URL.
        /// </summary>
        /// <value>
        /// The original URL.
        /// </value>
        public string LongUrl { get; set; }

        public static CreateUrlResponse Create(string id, string shortUrl, string longUrl)
        {
            Ensure.NotNullOrEmpty(id, "id can't be null");
            Ensure.NotNullOrEmpty(shortUrl, "shortUrl can't be null");
            Ensure.NotNullOrEmpty(longUrl, "longUrl can't be null");

            CreateUrlResponse createUrlResponse = new CreateUrlResponse
            {
                Id = id,
                ShortUrl = shortUrl,
                LongUrl = longUrl
            };

            return createUrlResponse;
        }
    }
}