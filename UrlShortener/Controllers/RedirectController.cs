using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        /// <summary>
        /// The URL service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlController"/> class.
        /// </summary>
        /// <param name="urlService">The URL service.</param>
        public RedirectController(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        /// <summary>
        /// Redirects to.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/{url}")]
        public async Task<RedirectResult> RedirectTo(string url)
        {
            string longUrl = await this.urlService.GetLongUrl(url);
            return new RedirectResult(longUrl);
        }
    }
}