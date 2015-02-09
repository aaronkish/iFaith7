namespace Ionic.Zip
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SfxGenerationException : ZipException
    {
        public SfxGenerationException()
        {
        }

        public SfxGenerationException(string message) : base(message)
        {
        }

        protected SfxGenerationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public SfxGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

