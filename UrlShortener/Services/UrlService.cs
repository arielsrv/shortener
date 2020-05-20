using System;
using System.Threading.Tasks;
using Base62;
using Microsoft.AspNetCore.Http;
using UrlShortener.Exceptions;
using UrlShortener.Model;
using UrlShortener.Storage;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        public IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The key value store
        /// </summary>
        public IKeyValueStore keyValueStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlService" /> class.
        /// </summary>
        /// <param name="keyValueStore">The key value store.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UrlService(
            IKeyValueStore keyValueStore,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.keyValueStore = keyValueStore;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Creates the URL.
        /// </summary>
        /// <param name="createUrlRequest">The create URL request.</param>
        /// <returns></returns>
        public async Task<CreateUrlResponse> CreateUrl(CreateUrlRequest createUrlRequest)
        {
            Base62Converter converter = new Base62Converter();

            long id = await this.keyValueStore.GetNewId();
            string segment = converter.Encode(id.ToString());

            string host = this.httpContextAccessor.HttpContext.Request.IsHttps
                ? $"https://{this.httpContextAccessor.HttpContext.Request.Host}"
                : $"http:/{this.httpContextAccessor.HttpContext.Request.Host}";
            
            CreateUrlResponse createUrlResponse = new CreateUrlResponse
            {
                Id = segment,
                LongUrl = createUrlRequest.Url,
                ShortUrl = host + segment
            };

            //ShortUrl = "https://localhost:44332/" + segment
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
                throw new ApiNotFoundException(shortUrl);
            }

            return createUrlResponse.LongUrl;
        }
    }
}