using Base62;
using System;
using System.Threading.Tasks;
using UrlShortener.Model;
using UrlShortener.Storage;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        /// <summary>
        /// The key value store
        /// </summary>
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
        /// <returns>c:\projects\UrlShortener\UrlShortener\Services\UrlService.cs</returns>
        public async Task<CreateUrlResponse> CreateUrl(CreateUrlRequest createUrlRequest)
        {
            Base62Converter converter = new Base62Converter();

            long id = await this.keyValueStore.GetNewId();
            string segment = converter.Encode(id.ToString());

            CreateUrlResponse createUrlResponse = new CreateUrlResponse
            {
                Id = segment,
                LongUrl = createUrlRequest.Url,
                ShortUrl = "http://localhost:44332/" + segment
            };

            await this.keyValueStore.Add(segment, createUrlResponse);

            return createUrlResponse;
        }

        /// <summary>
        /// Redirects to original URL.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<string> GetLongUrl(string shortUrl)
        {
            CreateUrlResponse createUrlResponse = await this.keyValueStore.Get<CreateUrlResponse>(shortUrl);

            if (createUrlResponse == null)
            {
                throw new ApplicationException();
            }

            return createUrlResponse.LongUrl;
        }
    }
}