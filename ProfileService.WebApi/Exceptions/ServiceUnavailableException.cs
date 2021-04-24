using System;

namespace ProfileService.WebApi.Exceptions
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string message) : base(message)
        {
        }
        
        public ServiceUnavailableException(string message, Exception e) : base(message, e)
        {
        }    
    }
}