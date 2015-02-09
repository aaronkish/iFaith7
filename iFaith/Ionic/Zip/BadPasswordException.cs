namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BadPasswordException : ZipException
    {
        public BadPasswordException()
        {
        }

        public BadPasswordException(string message) : base(message)
        {
        }

        protected BadPasswordException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public BadPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

