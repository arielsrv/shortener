using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Model;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    /// <summary>
    /// Create Url  
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        /// <summary>
        /// The URL service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlController"/> class.
        /// </summary>
        /// <param name="urlService">The URL service.</param>
        public UrlController(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        /// <summary>
        /// Creates the URL.
        /// </summary>
        /// <param name="createUrlRequest">The create URL request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateUrlResponse>> CreateUrl([FromBody] CreateUrlRequest createUrlRequest)
        {            
            return await this.urlService.CreateUrl(createUrlRequest);
        }
        
        /// <summary>
        /// Redirects to.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{url}")]
        public async Task<ActionResult<GetUrlResponse>> GetUrl(string url)
        {
            return await this.urlService.GetLongUrl(url);
        }
    }
}