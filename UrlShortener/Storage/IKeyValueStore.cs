namespace UrlShortener.Storage
{
    public interface IKeyValueStore
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns></returns>
        long GetId();
    }
}