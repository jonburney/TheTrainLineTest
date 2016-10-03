using System;

namespace AddressProcessing.CSV.Exceptions
{
    public class StreamReaderNotInitialisedException : Exception
    {
        public StreamReaderNotInitialisedException()
        {
        }

        public StreamReaderNotInitialisedException(string message) : base(message)
        {
        }

        public StreamReaderNotInitialisedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
