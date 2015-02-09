namespace Ionic.Zip
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate bool ReReadApprovalCallback(long uncompressedSize, long compressedSize, string filename);
}

