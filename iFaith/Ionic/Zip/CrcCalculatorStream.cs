namespace Ionic.Zip
{
    using System;
    using System.IO;

    public class CrcCalculatorStream : Stream
    {
        private CRC32 _Crc32;
        private Stream _InnerStream;
        private long _length;

        public CrcCalculatorStream(Stream stream)
        {
            this._length = 0L;
            this._InnerStream = stream;
            this._Crc32 = new CRC32();
        }

        public CrcCalculatorStream(Stream stream, long length)
        {
            this._length = 0L;
            this._InnerStream = stream;
            this._Crc32 = new CRC32();
            this._length = length;
        }

        public override void Flush()
        {
            this._InnerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = count;
            if (this._length != 0L)
            {
                if (this._Crc32.TotalBytesRead >= this._length)
                {
                    return 0;
                }
                long num3 = this._length - this._Crc32.TotalBytesRead;
                if (num3 < count)
                {
                    num = (int) num3;
                }
            }
            int num4 = this._InnerStream.Read(buffer, offset, num);
            if (num4 > 0)
            {
                this._Crc32.SlurpBlock(buffer, offset, num4);
            }
            return num4;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count > 0)
            {
                this._Crc32.SlurpBlock(buffer, offset, count);
            }
            this._InnerStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get
            {
                return this._InnerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this._InnerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._InnerStream.CanWrite;
            }
        }

        public int Crc32
        {
            get
            {
                return this._Crc32.Crc32Result;
            }
        }

        public override long Length
        {
            get
            {
                if (this._length == 0L)
                {
                    throw new NotImplementedException();
                }
                return this._length;
            }
        }

        public override long Position
        {
            get
            {
                return this._Crc32.TotalBytesRead;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public long TotalBytesSlurped
        {
            get
            {
                return this._Crc32.TotalBytesRead;
            }
        }
    }
}

