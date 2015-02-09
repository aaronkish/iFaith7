namespace ICSharpCode.SharpZipLib.BZip2
{
    using System;
    using System.IO;

    public sealed class BZip2
    {
        public static void Compress(Stream instream, Stream outstream, int blockSize)
        {
            Stream inStream = outstream;
            inStream.WriteByte(66);
            inStream.WriteByte(90);
            Stream stream2 = instream;
            int num = stream2.ReadByte();
            BZip2OutputStream stream3 = new BZip2OutputStream(inStream);
            while (num != -1)
            {
                stream3.WriteByte((byte) num);
                num = stream2.ReadByte();
            }
            stream2.Close();
            stream3.Close();
        }

        public static void Decompress(Stream instream, Stream outstream)
        {
            Stream stream = outstream;
            Stream zStream = instream;
            if ((zStream.ReadByte() == 66) && (zStream.ReadByte() == 90))
            {
                BZip2InputStream stream3 = new BZip2InputStream(zStream);
                for (int i = stream3.ReadByte(); i != -1; i = stream3.ReadByte())
                {
                    stream.WriteByte((byte) i);
                }
                stream.Flush();
            }
        }
    }
}

