using System;

namespace ProfileService.WebApi.Exceptions
{
    public class DuplicateProfileException : Exception
    {
        public DuplicateProfileException(string username) : base($"Profile for user {username} already exists")
        {
        }
    }
}