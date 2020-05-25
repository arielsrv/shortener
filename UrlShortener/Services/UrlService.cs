using Base62;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UrlShortener.Exceptions;
using UrlShortener.Helpers;
using UrlShortener.Model;
using UrlShortener.Storage;

namespace UrlShortener.Services
{
    /// <summary>
    /// Url Service
    /// </summary>
    /// <seealso cref="UrlShortener.Services.IUrlService" />
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
            Ensure.NotNullOrEmpty(createUrlRequest.Url, "url params can't be null");
            
            Base62Converter converter = new Base62Converter();

            long id = await this.keyValueStore.GetNewId();
            string segment = converter.Encode(id.ToString());

            string host = GetHost();

            CreateUrlResponse createUrlResponse = CreateUrlResponse
                .Create(segment,  $"{host}/{segment}", createUrlRequest.Url);

            await this.keyValueStore.Add(segment, createUrlResponse);

            return createUrlResponse;
        }

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <returns></returns>
        private string GetHost()
        {
            string host = this.httpContextAccessor.HttpContext.Request.IsHttps
                ? $"https://{this.httpContextAccessor.HttpContext.Request.Host}"
                : $"http://{this.httpContextAccessor.HttpContext.Request.Host}";
            
            return host;
        }

        /// <summary>
        /// Redirects to original URL.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<GetUrlResponse> GetLongUrl(string shortUrl)
        {
            Ensure.NotNullOrEmpty(shortUrl, "url can't be null");
            
            CreateUrlResponse createUrlResponse = await this.keyValueStore.Get<CreateUrlResponse>(shortUrl);

            if (createUrlResponse == null)
            {
                throw new ApiNotFoundException($"Url {shortUrl} not found");
            }

            GetUrlResponse getUrlResponse = GetUrlResponse.Create(createUrlResponse.LongUrl);

            return getUrlResponse;
        }
    }
}