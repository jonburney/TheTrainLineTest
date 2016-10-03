using System;

namespace AddressProcessing.CSV.Exceptions
{
    public class StreamWriterNotInitialisedException : Exception
    {
        public StreamWriterNotInitialisedException()
        {
        }

        public StreamWriterNotInitialisedException(string message) : base(message)
        {
        }

        public StreamWriterNotInitialisedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
