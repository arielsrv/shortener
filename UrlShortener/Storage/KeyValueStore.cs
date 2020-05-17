using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading.Tasks;

namespace UrlShortener.Storage
{
    /// <summary>
    /// Key Value Store
    /// </summary>
    /// <seealso cref="UrlShortener.Storage.IKeyValueStore" />
    public class KeyValueStore : IKeyValueStore
    {
        /// <summary>
        /// The redis cache client
        /// </summary>
        private readonly IRedisCacheClient redisCacheClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueStore"/> class.
        /// </summary>
        public KeyValueStore(IRedisCacheClient redisCacheClient)
        {
            this.redisCacheClient = redisCacheClient;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task<bool> Add<T>(string key, T value)
        {
            return await redisCacheClient
                .GetDbFromConfiguration()
                .AddAsync(key, value);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            return await redisCacheClient
                .GetDbFromConfiguration()
                .GetAsync<T>(key);
        }



        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetNewId()
        {
            return await this.redisCacheClient
                .GetDbFromConfiguration()
                .HashIncerementByAsync("url:id", "url:id", 1L);
        }
    }
}