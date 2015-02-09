namespace Ionic.Zlib
{
    using System;
    using System.IO;

    public class ZlibStream : Stream
    {
        internal ZlibBaseStream _baseStream;

        public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.DEFAULT, false)
        {
        }

        public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
        {
        }

        public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.DEFAULT, leaveOpen)
        {
        }

        public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
        {
            this._baseStream = new ZlibBaseStream(stream, mode, level, true, leaveOpen);
        }

        public override void Close()
        {
            this._baseStream.Close();
        }

        public override void Flush()
        {
            this._baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._baseStream.Read(buffer, offset, count);
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
            this._baseStream.Write(buffer, offset, count);
        }

        public int BufferSize
        {
            get
            {
                return this._baseStream._workingBuffer.Length;
            }
            set
            {
                if (value < this._baseStream.WORKING_BUFFER_SIZE_MIN)
                {
                    throw new ZlibException("Don't be silly. Use a bigger buffer.");
                }
                this._baseStream._workingBuffer = new byte[value];
            }
        }

        public override bool CanRead
        {
            get
            {
                return this._baseStream._stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._baseStream._stream.CanWrite;
            }
        }

        public virtual int FlushMode
        {
            get
            {
                return this._baseStream._flushMode;
            }
            set
            {
                this._baseStream._flushMode = value;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual long TotalIn
        {
            get
            {
                return this._baseStream._z.TotalBytesIn;
            }
        }

        public virtual long TotalOut
        {
            get
            {
                return this._baseStream._z.TotalBytesOut;
            }
        }
    }
}

