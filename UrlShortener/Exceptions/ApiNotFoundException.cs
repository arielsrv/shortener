using System;

namespace UrlShortener.Exceptions
{
    /// <summary>
    /// Not Found
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class ApiNotFoundException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ApiNotFoundException(string message) : base(message)
        {
        }
    }
}