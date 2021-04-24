using System;

namespace ProfileService.WebApi.Exceptions
{
    public class EmployerServiceUnavailableException : ServiceUnavailableException
    {
        public EmployerServiceUnavailableException(string message) : base(message)
        {
        }
        
        public EmployerServiceUnavailableException(string message, Exception e) : base(message, e)
        {
        }
    }
}