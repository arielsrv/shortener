using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UrlShortener.Exceptions;
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
                .Returns(new HostString("localhost:44332"));

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
        public void Create_Insecure_Short_Url()
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
                .Returns(new HostString("localhost:44332"));

            Mock.Get(this.urlService.httpContextAccessor)
                .Setup(behavior => behavior.HttpContext.Request.IsHttps)
                .Returns(false);

            Task<CreateUrlResponse> awaitable = this.urlService.CreateUrl(createUrlRequest);

            CreateUrlResponse createUrlResponse = awaitable.Result;

            Assert.NotNull(createUrlResponse);
            Assert.AreEqual("n", createUrlResponse.Id);
            Assert.AreEqual("http://localhost:44332/n", createUrlResponse.ShortUrl);
            Assert.AreEqual("a", createUrlResponse.LongUrl);
        }

        /// <summary>
        /// Redirects to original URL.
        /// </summary>
        [Test]
        public void Redirect_To_Original_Url()
        {
            Mock.Get(this.urlService.keyValueStore)
                .Setup(behavior => behavior.Get<CreateUrlResponse>("3HA"))
                .Returns(Task.FromResult(new CreateUrlResponse
                {
                    LongUrl = "https://www.facebook.com"
                }));

            Task<GetUrlResponse> awaitable = this.urlService.GetLongUrl("3HA");

            GetUrlResponse getUrlResponse = awaitable.Result;

            Assert.NotNull(getUrlResponse);
            Assert.NotNull(getUrlResponse.Url);
            Assert.AreEqual("https://www.facebook.com", getUrlResponse.Url);
        }

        [Test]
        public void Redirect_To_Original_Url_Not_Found()
        {
            Mock.Get(this.urlService.keyValueStore)
                .Setup(behavior => behavior.Get<CreateUrlResponse>("3HA"))
                .Returns(Task.FromResult(default(CreateUrlResponse)));

            Task<GetUrlResponse> awaitable = this.urlService.GetLongUrl("3HA");

            try
            {
                GetUrlResponse getUrlResponse = awaitable.Result;
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ApiNotFoundException), e.InnerException.GetType());
                Assert.AreEqual("Url 3HA not found", e.InnerException.Message);
            }
        }

        [Test]
        public void Redirect_To_Original_Url_Bad_Request()
        {
            Task<GetUrlResponse> awaitable = this.urlService.GetLongUrl(null);

            try
            {
                GetUrlResponse getUrlResponse = awaitable.Result;
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ApiBadRequestException), e.InnerException.GetType());
                Assert.AreEqual("url can't be null", e.InnerException.Message);
            }
        }
    }
}