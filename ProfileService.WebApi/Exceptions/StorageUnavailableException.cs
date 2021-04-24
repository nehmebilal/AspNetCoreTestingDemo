using System;

namespace ProfileService.WebApi.Exceptions
{
    public class StorageUnavailableException : ServiceUnavailableException
    {
        public StorageUnavailableException(string message) : base(message)
        {
        }
        public StorageUnavailableException(string message, Exception e) : base(message, e)
        {
        }
    }

    public class StorageException : Exception
    {
    }

    public class TransientStorageException : StorageException
    {
        
    }

    public enum TransientStorageError
    {
        TooManyRequests
    }
}