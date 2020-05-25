using System;

namespace UrlShortener.Exceptions
{
    public class ApiBadRequestException : ApplicationException
    {
        public ApiBadRequestException(string message) : base(message)
        {
        }
    }
}