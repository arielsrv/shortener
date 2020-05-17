using System.Threading.Tasks;
using UrlShortener.Model;
using UrlShortener.Storage;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        public IKeyValueStore keyValueStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlService"/> class.
        /// </summary>
        /// <param name="keyValueStore">The key value store.</param>
        public UrlService(IKeyValueStore keyValueStore)
        {
            this.keyValueStore = keyValueStore;
        }

        /// <summary>
        /// Creates the URL.
        /// </summary>
        /// <param name="createUrlRequest">The create URL request.</param>
        /// <returns></returns>
        public async Task<CreateUrlResponse> CreateUrl(CreateUrlRequest createUrlRequest)
        {
            return new CreateUrlResponse
            {
                Id = this.keyValueStore.GetId().ToString(),
                ShortUrl = createUrlRequest.Url
            };
        }
    }
}