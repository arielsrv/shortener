using System;

namespace UrlShortener.Exceptions
{
    /// <summary>
    /// Bad Request
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class ApiBadRequestException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiBadRequestException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ApiBadRequestException(string message) : base(message)
        {
        }
    }
}