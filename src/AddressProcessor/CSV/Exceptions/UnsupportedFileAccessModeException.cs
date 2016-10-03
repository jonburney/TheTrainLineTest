using System;

namespace AddressProcessing.CSV.Exceptions
{
    public class UnsupportedFileAccessModeException : Exception
    {
        public UnsupportedFileAccessModeException()
        {
        }

        public UnsupportedFileAccessModeException(string message) : base(message)
        {
        }

        public UnsupportedFileAccessModeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
