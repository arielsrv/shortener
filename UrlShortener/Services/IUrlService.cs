using System.Threading.Tasks;
using UrlShortener.Model;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        /// <summary>
        /// Creates the URL.
        /// </summary>
        /// <param name="createUrlRequest">The create URL request.</param>
        /// <returns></returns>
        Task<CreateUrlResponse> CreateUrl(CreateUrlRequest createUrlRequest);
    }
}