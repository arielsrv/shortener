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
    }
}