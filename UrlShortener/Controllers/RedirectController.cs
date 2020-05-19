using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    /// <summary>
    /// Redirect to Original Url.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
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
        public async Task<IActionResult> RedirectTo(string url)
        {
            string longUrl = await this.urlService.GetLongUrl(url);
            return Redirect(longUrl);
        }
    }
}