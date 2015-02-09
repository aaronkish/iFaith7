namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BadReadException : ZipException
    {
        public BadReadException()
        {
        }

        public BadReadException(string message) : base(message)
        {
        }

        protected BadReadException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public BadReadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

