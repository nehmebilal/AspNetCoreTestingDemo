using System;

namespace ProfileService.WebApi.Exceptions
{
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException(string username) : base($"Could not find a profile for user {username}")
        {
        }
    }
}