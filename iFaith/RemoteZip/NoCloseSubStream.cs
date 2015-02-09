namespace iFaith.RemoteZip
{
    using System;
    using System.IO;

    public class NoCloseSubStream : Stream
    {
        private Stream baseStream;

        public NoCloseSubStream(Stream b)
        {
            this.baseStream = b;
        }

        public override void Close()
        {
            this.baseStream = null;
        }

        public override void Flush()
        {
            this.baseStream.Flush();
        }

        public override int Read(byte[] b, int off, int len)
        {
            return this.baseStream.Read(b, off, len);
        }

        public override int ReadByte()
        {
            return this.baseStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        public override void SetLength(long val)
        {
            this.baseStream.SetLength(val);
        }

        public override void Write(byte[] buf, int off, int len)
        {
            this.baseStream.Write(buf, off, len);
        }

        public override void WriteByte(byte bv)
        {
            this.baseStream.WriteByte(bv);
        }

        public override bool CanRead
        {
            get
            {
                return this.baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.baseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseStream.Position;
            }
            set
            {
                this.baseStream.Position = value;
            }
        }
    }
}

