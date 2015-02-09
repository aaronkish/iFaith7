namespace ICSharpCode.SharpZipLib
{
    using System;

    public class ZipException : Exception
    {
        public ZipException()
        {
        }

        public ZipException(string msg) : base(msg)
        {
        }
    }
}

