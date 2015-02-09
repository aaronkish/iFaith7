namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ZipException : Exception
    {
        public ZipException()
        {
        }

        public ZipException(string message) : base(message)
        {
        }

        protected ZipException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ZipException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

