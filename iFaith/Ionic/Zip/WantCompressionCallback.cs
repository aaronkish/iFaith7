namespace Ionic.Zip
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate bool WantCompressionCallback(string localFilename, string filenameInArchive);
}

