using System.Threading.Tasks;

namespace UrlShortener.Storage
{
    public interface IKeyValueStore
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        Task<long> GetNewId();

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        Task<long> GetId();

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<bool> Add<T>(string key, T value);
    }
}