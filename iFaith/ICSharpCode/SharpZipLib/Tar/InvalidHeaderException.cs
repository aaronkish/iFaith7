namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;

    public class InvalidHeaderException : IOException
    {
        public InvalidHeaderException()
        {
        }

        public InvalidHeaderException(string msg) : base(msg)
        {
        }
    }
}

