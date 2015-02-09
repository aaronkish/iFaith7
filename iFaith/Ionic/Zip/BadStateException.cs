namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BadStateException : ZipException
    {
        public BadStateException()
        {
        }

        public BadStateException(string message) : base(message)
        {
        }

        protected BadStateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public BadStateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

