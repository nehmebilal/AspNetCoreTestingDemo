using System;

namespace ProfileService.WebApi.Exceptions
{
    public class EmployerNotFoundException : Exception
    {
        public EmployerNotFoundException(string employerName) : base($"The employer {employerName} was not found")
        {
        }
    }
}