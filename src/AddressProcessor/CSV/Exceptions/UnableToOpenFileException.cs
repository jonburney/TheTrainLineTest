using System;

namespace AddressProcessing.CSV.Exceptions
{
    public class UnableToOpenFileException : Exception
    {
        public UnableToOpenFileException()
        {
        }

        public UnableToOpenFileException(string message) : base(message)
        {
        }

        public UnableToOpenFileException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
