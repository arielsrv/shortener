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
        private UrlService urlService;

        [SetUp]
        public void SetUp()
        {
            Mock<IKeyValueStore> keyValueStore = new Mock<IKeyValueStore>();
            this.urlService = new UrlService(keyValueStore.Object);
        }

        [Test]
        public void Create_Short_Url()
        {
            CreateUrlRequest createUrlRequest = new CreateUrlRequest
            {
                Url = "www.google.com"
            };

            Mock.Get(this.urlService.keyValueStore)
                .Setup(behavior => behavior.GetId())
                .Returns(1);

            Task<CreateUrlResponse> awaitable = this.urlService.CreateUrl(createUrlRequest);

            CreateUrlResponse createUrlResponse = awaitable.Result;

            Assert.NotNull(createUrlResponse);
        }
    }
}