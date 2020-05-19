using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UrlShortener.Model;
using UrlShortener.Services;
using UrlShortener.Storage;

namespace UrlShortener.Test.Services
{
    public class UrlServiceTest
    {
        /// <summary>
        /// The URL service
        /// </summary>
        private UrlService urlService;

        /// <summary>
        /// Sets up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Mock<IKeyValueStore> keyValueStore = new Mock<IKeyValueStore>();
            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            this.urlService = new UrlService(keyValueStore.Object, httpContextAccessor.Object);
        }

        /// <summary>
        /// Creates the short URL.
        /// </summary>
        [Test]
        public void Create_Short_Url()
        {
            CreateUrlRequest createUrlRequest = new CreateUrlRequest
            {
                Url = "a"
            };

            Mock.Get(this.urlService.keyValueStore)
                .Setup(behavior => behavior.GetNewId())
                .Returns(Task.FromResult(1L));
            
            Mock.Get(this.urlService.httpContextAccessor)
                .Setup(behavior => behavior.HttpContext.Request.Host)
                .Returns(new HostString("localhost:44332/"));
            
            Mock.Get(this.urlService.httpContextAccessor)
                .Setup(behavior => behavior.HttpContext.Request.IsHttps)
                .Returns(true);

            Task<CreateUrlResponse> awaitable = this.urlService.CreateUrl(createUrlRequest);

            CreateUrlResponse createUrlResponse = awaitable.Result;

            Assert.NotNull(createUrlResponse);
            Assert.AreEqual("n", createUrlResponse.Id);
            Assert.AreEqual("https://localhost:44332/n", createUrlResponse.ShortUrl);
            Assert.AreEqual("a", createUrlResponse.LongUrl);
        }

        [Test]
        public void Redirect_To_Original_Url()
        { 
            Mock.Get(this.urlService.keyValueStore)
                .Setup(behavior => behavior.Get<CreateUrlResponse>("3HA"))
                .Returns(Task.FromResult(new CreateUrlResponse{
                    LongUrl = "https://www.facebook.com" }));
            
            Task<string> awaitable = this.urlService.GetLongUrl("3HA");

            string longUrl = awaitable.Result;
            
            Assert.NotNull(longUrl);
            Assert.AreEqual("https://www.facebook.com", longUrl);
        }
    }
}