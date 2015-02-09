namespace ICSharpCode.SharpZipLib.GZip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.IO;

    public class GZipOutputStream : DeflaterOutputStream
    {
        protected Crc32 crc;

        public GZipOutputStream(Stream baseOutputStream) : this(baseOutputStream, 0x1000)
        {
        }

        public GZipOutputStream(Stream baseOutputStream, int size) : base(baseOutputStream, new Deflater(Deflater.DEFAULT_COMPRESSION, true), size)
        {
            this.crc = new Crc32();
            int num = (int) (DateTime.Now.Ticks / 0x2710L);
            byte[] buffer = new byte[10];
            buffer[0] = (byte) (GZipConstants.GZIP_MAGIC >> 8);
            buffer[1] = (byte) GZipConstants.GZIP_MAGIC;
            buffer[2] = (byte) Deflater.DEFLATED;
            buffer[4] = (byte) num;
            buffer[5] = (byte) (num >> 8);
            buffer[6] = (byte) (num >> 0x10);
            buffer[7] = (byte) (num >> 0x18);
            buffer[9] = 0xff;
            byte[] buffer2 = buffer;
            baseOutputStream.Write(buffer2, 0, buffer2.Length);
        }

        public override void Close()
        {
            this.Finish();
            base.baseOutputStream.Close();
        }

        public override void Finish()
        {
            base.Finish();
            int totalIn = base.def.TotalIn;
            int num2 = (int) (((ulong) this.crc.Value) & 0xffffffffL);
            byte[] buffer = new byte[] { (byte) num2, (byte) (num2 >> 8), (byte) (num2 >> 0x10), (byte) (num2 >> 0x18), (byte) totalIn, (byte) (totalIn >> 8), (byte) (totalIn >> 0x10), (byte) (totalIn >> 0x18) };
            base.baseOutputStream.Write(buffer, 0, buffer.Length);
        }

        public override void Write(byte[] buf, int off, int len)
        {
            this.crc.Update(buf, off, len);
            base.Write(buf, off, len);
        }
    }
}

