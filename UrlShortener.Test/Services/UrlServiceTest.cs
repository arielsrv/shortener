using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
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
            this.urlService = new UrlService(keyValueStore.Object);
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

            Task<CreateUrlResponse> awaitable = this.urlService.CreateUrl(createUrlRequest);

            CreateUrlResponse createUrlResponse = awaitable.Result;

            Assert.NotNull(createUrlResponse);
            Assert.AreEqual("n", createUrlResponse.Id);
            Assert.AreEqual("http://localhost:44332/n", createUrlResponse.ShortUrl);
            Assert.AreEqual("a", createUrlResponse.LongUrl);
        }
    }
}