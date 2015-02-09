namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BadCrcException : ZipException
    {
        public BadCrcException()
        {
        }

        public BadCrcException(string message) : base(message)
        {
        }

        protected BadCrcException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public BadCrcException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

