namespace Ionic.Zlib
{
    using System;
    using System.IO;

    internal class ZlibBaseStream : Stream
    {
        protected internal byte[] _buf1 = new byte[1];
        protected internal int _flushMode = 0;
        protected internal bool _leaveOpen;
        protected internal Stream _stream;
        protected internal StreamMode _streamMode = StreamMode.Undefined;
        protected internal bool _wantCompress;
        protected internal byte[] _workingBuffer;
        protected internal ZlibCodec _z = new ZlibCodec();
        private bool nomoreinput = false;
        protected internal readonly int WORKING_BUFFER_SIZE_DEFAULT = 0x4000;
        protected internal readonly int WORKING_BUFFER_SIZE_MIN = 0x80;

        public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, bool wantRfc1950Header, bool leaveOpen)
        {
            this._workingBuffer = new byte[this.WORKING_BUFFER_SIZE_DEFAULT];
            this._stream = stream;
            this._leaveOpen = leaveOpen;
            if (compressionMode == CompressionMode.Decompress)
            {
                this._z.InitializeInflate(wantRfc1950Header);
                this._wantCompress = false;
            }
            else
            {
                this._z.InitializeDeflate(level, wantRfc1950Header);
                this._wantCompress = true;
            }
        }

        public override void Close()
        {
            try
            {
                this.finish();
            }
            catch (IOException)
            {
            }
            finally
            {
                this.end();
                if (!this._leaveOpen)
                {
                    this._stream.Close();
                }
                this._stream = null;
            }
        }

        private void end()
        {
            if (this._z != null)
            {
                if (this._wantCompress)
                {
                    this._z.EndDeflate();
                }
                else
                {
                    this._z.EndInflate();
                }
                this._z = null;
            }
        }

        private void finish()
        {
            if (this._streamMode == StreamMode.Writer)
            {
                do
                {
                    this._z.OutputBuffer = this._workingBuffer;
                    this._z.NextOut = 0;
                    this._z.AvailableBytesOut = this._workingBuffer.Length;
                    int num = this._wantCompress ? this._z.Deflate(4) : this._z.Inflate(4);
                    if ((num != 1) && (num != 0))
                    {
                        throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
                    }
                    if ((this._workingBuffer.Length - this._z.AvailableBytesOut) > 0)
                    {
                        this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                    }
                }
                while ((this._z.AvailableBytesIn > 0) || (this._z.AvailableBytesOut == 0));
                this.Flush();
            }
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public int Read()
        {
            if (this.Read(this._buf1, 0, 1) == -1)
            {
                return -1;
            }
            return (this._buf1[0] & 0xff);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num2;
            if (this._streamMode == StreamMode.Undefined)
            {
                this._streamMode = StreamMode.Reader;
                this._z.AvailableBytesIn = 0;
            }
            if (this._streamMode != StreamMode.Reader)
            {
                throw new ZlibException("Cannot Read after Writing.");
            }
            if (!this._stream.CanRead)
            {
                throw new ZlibException("The stream is not readable.");
            }
            if (count == 0)
            {
                return 0;
            }
            this._z.OutputBuffer = buffer;
            this._z.NextOut = offset;
            this._z.AvailableBytesOut = count;
            this._z.InputBuffer = this._workingBuffer;
            do
            {
                if ((this._z.AvailableBytesIn == 0) && !this.nomoreinput)
                {
                    this._z.NextIn = 0;
                    this._z.AvailableBytesIn = SharedUtils.ReadInput(this._stream, this._workingBuffer, 0, this._workingBuffer.Length);
                    if (this._z.AvailableBytesIn == -1)
                    {
                        this._z.AvailableBytesIn = 0;
                        this.nomoreinput = true;
                    }
                }
                num2 = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
                if (this.nomoreinput && (num2 == -5))
                {
                    return -1;
                }
                if ((num2 != 0) && (num2 != 1))
                {
                    throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
                }
                if ((this.nomoreinput || (num2 == 1)) && (this._z.AvailableBytesOut == count))
                {
                    return -1;
                }
            }
            while ((this._z.AvailableBytesOut == count) && (num2 == 0));
            return (count - this._z.AvailableBytesOut);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int length)
        {
            if (this._streamMode == StreamMode.Undefined)
            {
                this._streamMode = StreamMode.Writer;
            }
            if (this._streamMode != StreamMode.Writer)
            {
                throw new ZlibException("Cannot Write after Reading.");
            }
            if (length != 0)
            {
                this._z.InputBuffer = buffer;
                this._z.NextIn = offset;
                this._z.AvailableBytesIn = length;
                do
                {
                    this._z.OutputBuffer = this._workingBuffer;
                    this._z.NextOut = 0;
                    this._z.AvailableBytesOut = this._workingBuffer.Length;
                    int num = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
                    if ((num != 0) && (num != 1))
                    {
                        throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
                    }
                    this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                }
                while ((this._z.AvailableBytesIn > 0) || (this._z.AvailableBytesOut == 0));
            }
        }

        public override void WriteByte(byte b)
        {
            this._buf1[0] = b;
            this.Write(this._buf1, 0, 1);
        }

        public override bool CanRead
        {
            get
            {
                return this._stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this._stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this._stream.Length;
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

        internal enum StreamMode
        {
            Writer,
            Reader,
            Undefined
        }
    }
}

