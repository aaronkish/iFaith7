namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;

    public class TarOutputStream : Stream
    {
        protected byte[] assemBuf;
        protected int assemLen;
        protected TarBuffer buffer;
        protected int currBytes;
        protected int currSize;
        protected bool debug;
        protected Stream outputStream;
        protected byte[] recordBuf;

        public TarOutputStream(Stream outputStream) : this(outputStream, TarBuffer.DEFAULT_BLKSIZE, TarBuffer.DEFAULT_RCDSIZE)
        {
        }

        public TarOutputStream(Stream outputStream, int blockSize) : this(outputStream, blockSize, TarBuffer.DEFAULT_RCDSIZE)
        {
        }

        public TarOutputStream(Stream outputStream, int blockSize, int recordSize)
        {
            this.outputStream = outputStream;
            this.buffer = TarBuffer.CreateOutputTarBuffer(outputStream, blockSize, recordSize);
            this.debug = false;
            this.assemLen = 0;
            this.assemBuf = new byte[recordSize];
            this.recordBuf = new byte[recordSize];
        }

        public override void Close()
        {
            this.Finish();
            this.buffer.Close();
        }

        public void CloseEntry()
        {
            if (this.assemLen > 0)
            {
                for (int i = this.assemLen; i < this.assemBuf.Length; i++)
                {
                    this.assemBuf[i] = 0;
                }
                this.buffer.WriteRecord(this.assemBuf);
                this.currBytes += this.assemLen;
                this.assemLen = 0;
            }
            if (this.currBytes < this.currSize)
            {
                throw new IOException(string.Concat(new object[] { "entry closed at '", this.currBytes, "' before the '", this.currSize, "' bytes specified in the header were written" }));
            }
        }

        public void Finish()
        {
            this.WriteEOFRecord();
        }

        public override void Flush()
        {
            this.outputStream.Flush();
        }

        public int GetRecordSize()
        {
            return this.buffer.GetRecordSize();
        }

        public void PutNextEntry(TarEntry entry)
        {
            if (entry.TarHeader.name.Length > TarHeader.NAMELEN)
            {
                throw new InvalidHeaderException(string.Concat(new object[] { "file name '", entry.TarHeader.name, "' is too long ( > ", TarHeader.NAMELEN, " bytes )" }));
            }
            entry.WriteEntryHeader(this.recordBuf);
            this.buffer.WriteRecord(this.recordBuf);
            this.currBytes = 0;
            this.currSize = entry.IsDirectory ? 0 : ((int) entry.Size);
        }

        public override int Read(byte[] b, int off, int len)
        {
            return this.outputStream.Read(b, off, len);
        }

        public override int ReadByte()
        {
            return this.outputStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.outputStream.Seek(offset, origin);
        }

        public void SetBufferDebug(bool debug)
        {
            this.buffer.SetDebug(debug);
        }

        public void SetDebug(bool debugF)
        {
            this.debug = debugF;
            this.SetBufferDebug(debugF);
        }

        public override void SetLength(long val)
        {
            this.outputStream.SetLength(val);
        }

        public override void Write(byte[] wBuf, int wOffset, int numToWrite)
        {
            if ((this.currBytes + numToWrite) > this.currSize)
            {
                throw new IOException(string.Concat(new object[] { "request to write '", numToWrite, "' bytes exceeds size in header of '", this.currSize, "' bytes" }));
            }
            if (this.assemLen > 0)
            {
                if ((this.assemLen + numToWrite) >= this.recordBuf.Length)
                {
                    int length = this.recordBuf.Length - this.assemLen;
                    Array.Copy(this.assemBuf, 0, this.recordBuf, 0, this.assemLen);
                    Array.Copy(wBuf, wOffset, this.recordBuf, this.assemLen, length);
                    this.buffer.WriteRecord(this.recordBuf);
                    this.currBytes += this.recordBuf.Length;
                    wOffset += length;
                    numToWrite -= length;
                    this.assemLen = 0;
                }
                else
                {
                    Array.Copy(wBuf, wOffset, this.assemBuf, this.assemLen, numToWrite);
                    wOffset += numToWrite;
                    this.assemLen += numToWrite;
                    numToWrite -= numToWrite;
                }
            }
            while (numToWrite > 0)
            {
                if (numToWrite < this.recordBuf.Length)
                {
                    Array.Copy(wBuf, wOffset, this.assemBuf, this.assemLen, numToWrite);
                    this.assemLen += numToWrite;
                    return;
                }
                this.buffer.WriteRecord(wBuf, wOffset);
                int num2 = this.recordBuf.Length;
                this.currBytes += num2;
                numToWrite -= num2;
                wOffset += num2;
            }
        }

        public override void WriteByte(byte b)
        {
            this.Write(new byte[] { b }, 0, 1);
        }

        private void WriteEOFRecord()
        {
            for (int i = 0; i < this.recordBuf.Length; i++)
            {
                this.recordBuf[i] = 0;
            }
            this.buffer.WriteRecord(this.recordBuf);
        }

        public override bool CanRead
        {
            get
            {
                return this.outputStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.outputStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.outputStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.outputStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.outputStream.Position;
            }
            set
            {
                this.outputStream.Position = value;
            }
        }
    }
}

