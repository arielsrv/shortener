using Base62;
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
            Base62Converter converter = new Base62Converter();
            
            string key = converter.Encode(createUrlRequest.Url);
            CreateUrlResponse createUrlResponse = await this.keyValueStore.Get<CreateUrlResponse>(key);

            if (createUrlResponse != null)
            {
                return createUrlResponse;
            }

            long identifier = await this.keyValueStore.GetId();
            string segment  = converter.Encode(identifier.ToString());

            createUrlResponse = new CreateUrlResponse
            {
                Id = segment,
                LongUrl = createUrlRequest.Url,
                ShortUrl = "http://localhost:44332/" + segment
            };

            await this.keyValueStore.Add(key, createUrlResponse);

            return createUrlResponse;
        }
    }
}