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

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
        public ApiNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}