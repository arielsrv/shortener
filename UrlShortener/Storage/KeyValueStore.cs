using StackExchange.Redis;

namespace UrlShortener.Storage
{
    public class KeyValueStore : IKeyValueStore
    {
        /// <summary>
        /// The redis
        /// </summary>
        private readonly ConnectionMultiplexer redis;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueStore"/> class.
        /// </summary>
        public KeyValueStore()
        {
            this.redis = ConnectionMultiplexer.Connect("localhost");
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            return this.redis.GetDatabase().HashIncrement(new RedisKey("url:id"), new RedisValue("1"));
        }
    }
}