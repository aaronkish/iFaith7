namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;

    public class DeflaterOutputStream : Stream
    {
        protected Stream baseOutputStream;
        protected byte[] buf;
        protected Deflater def;

        public DeflaterOutputStream(Stream baseOutputStream) : this(baseOutputStream, new Deflater(), 0x200)
        {
        }

        public DeflaterOutputStream(Stream baseOutputStream, Deflater defl) : this(baseOutputStream, defl, 0x200)
        {
        }

        public DeflaterOutputStream(Stream baseOutputStream, Deflater defl, int bufsize)
        {
            this.baseOutputStream = baseOutputStream;
            if (bufsize <= 0)
            {
                throw new InvalidOperationException("bufsize <= 0");
            }
            this.buf = new byte[bufsize];
            this.def = defl;
        }

        public override void Close()
        {
            this.Finish();
            this.baseOutputStream.Close();
        }

        protected void deflate()
        {
            while (!this.def.IsNeedingInput)
            {
                int count = this.def.Deflate(this.buf, 0, this.buf.Length);
                if (count <= 0)
                {
                    break;
                }
                this.baseOutputStream.Write(this.buf, 0, count);
            }
            if (!this.def.IsNeedingInput)
            {
                throw new ApplicationException("Can't deflate all input?");
            }
        }

        public virtual void Finish()
        {
            this.def.Finish();
            while (!this.def.IsFinished)
            {
                int count = this.def.Deflate(this.buf, 0, this.buf.Length);
                if (count <= 0)
                {
                    break;
                }
                this.baseOutputStream.Write(this.buf, 0, count);
            }
            if (!this.def.IsFinished)
            {
                throw new ApplicationException("Can't deflate all input?");
            }
            this.baseOutputStream.Flush();
        }

        public override void Flush()
        {
            this.def.Flush();
            this.deflate();
            this.baseOutputStream.Flush();
        }

        public override int Read(byte[] b, int off, int len)
        {
            return this.baseOutputStream.Read(b, off, len);
        }

        public override int ReadByte()
        {
            return this.baseOutputStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseOutputStream.Seek(offset, origin);
        }

        public override void SetLength(long val)
        {
            this.baseOutputStream.SetLength(val);
        }

        public override void Write(byte[] buf, int off, int len)
        {
            this.def.SetInput(buf, off, len);
            this.deflate();
        }

        public override void WriteByte(byte bval)
        {
            byte[] buffer = new byte[] { bval };
            this.Write(buffer, 0, 1);
        }

        public override bool CanRead
        {
            get
            {
                return this.baseOutputStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.baseOutputStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseOutputStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.baseOutputStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseOutputStream.Position;
            }
            set
            {
                this.baseOutputStream.Position = value;
            }
        }
    }
}

