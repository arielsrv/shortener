using UrlShortener.Exceptions;

namespace UrlShortener.Helpers
{
    public class Ensure
    {
        public static void NotNullOrEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ApiBadRequestException(message);
            }
        }
    }
}