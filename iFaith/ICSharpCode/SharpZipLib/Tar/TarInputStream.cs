namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;

    public class TarInputStream : Stream
    {
        protected TarBuffer buffer;
        protected TarEntry currEntry;
        protected bool debug;
        protected IEntryFactory eFactory;
        protected int entryOffset;
        protected int entrySize;
        protected bool hasHitEOF;
        private Stream inputStream;
        protected byte[] readBuf;

        public TarInputStream(Stream inputStream) : this(inputStream, TarBuffer.DEFAULT_BLKSIZE, TarBuffer.DEFAULT_RCDSIZE)
        {
        }

        public TarInputStream(Stream inputStream, int blockSize) : this(inputStream, blockSize, TarBuffer.DEFAULT_RCDSIZE)
        {
        }

        public TarInputStream(Stream inputStream, int blockSize, int recordSize)
        {
            this.inputStream = inputStream;
            this.buffer = TarBuffer.CreateInputTarBuffer(inputStream, blockSize, recordSize);
            this.readBuf = null;
            this.debug = false;
            this.hasHitEOF = false;
            this.eFactory = null;
        }

        public override void Close()
        {
            this.buffer.Close();
        }

        public void CopyEntryContents(Stream outputStream)
        {
            byte[] buffer = new byte[0x8000];
            while (true)
            {
                int count = this.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return;
                }
                outputStream.Write(buffer, 0, count);
            }
        }

        public override void Flush()
        {
            this.inputStream.Flush();
        }

        public TarEntry GetNextEntry()
        {
            if (this.hasHitEOF)
            {
                return null;
            }
            if (this.currEntry != null)
            {
                int numToSkip = this.entrySize - this.entryOffset;
                bool debug = this.debug;
                if (numToSkip > 0)
                {
                    this.Skip(numToSkip);
                }
                this.readBuf = null;
            }
            byte[] record = this.buffer.ReadRecord();
            if (record == null)
            {
                bool flag2 = this.debug;
                this.hasHitEOF = true;
            }
            else if (this.buffer.IsEOFRecord(record))
            {
                bool flag3 = this.debug;
                this.hasHitEOF = true;
            }
            if (this.hasHitEOF)
            {
                this.currEntry = null;
            }
            else
            {
                try
                {
                    if (this.eFactory == null)
                    {
                        this.currEntry = new TarEntry(record);
                    }
                    else
                    {
                        this.currEntry = this.eFactory.CreateEntry(record);
                    }
                    if (((record[0x101] != 0x75) || (record[0x102] != 0x73)) || (((record[0x103] != 0x74) || (record[260] != 0x61)) || (record[0x105] != 0x72)))
                    {
                        throw new InvalidHeaderException(string.Concat(new object[] { "header magic is not 'ustar', but '", record[0x101], record[0x102], record[0x103], record[260], record[0x105], "', or (dec) ", (int) record[0x101], ", ", (int) record[0x102], ", ", (int) record[0x103], ", ", (int) record[260], ", ", (int) record[0x105] }));
                    }
                    bool flag4 = this.debug;
                    this.entryOffset = 0;
                    this.entrySize = (int) this.currEntry.Size;
                }
                catch (InvalidHeaderException exception)
                {
                    this.entrySize = 0;
                    this.entryOffset = 0;
                    this.currEntry = null;
                    throw new InvalidHeaderException(string.Concat(new object[] { "bad header in block ", this.buffer.GetCurrentBlockNum(), " record ", this.buffer.GetCurrentRecordNum(), ", ", exception.Message }));
                }
            }
            return this.currEntry;
        }

        public int GetRecordSize()
        {
            return this.buffer.GetRecordSize();
        }

        public void Mark(int markLimit)
        {
        }

        public override int Read(byte[] buf, int offset, int numToRead)
        {
            int num = 0;
            if (this.entryOffset >= this.entrySize)
            {
                return -1;
            }
            if ((numToRead + this.entryOffset) > this.entrySize)
            {
                numToRead = this.entrySize - this.entryOffset;
            }
            if (this.readBuf != null)
            {
                int length = (numToRead > this.readBuf.Length) ? this.readBuf.Length : numToRead;
                Array.Copy(this.readBuf, 0, buf, offset, length);
                if (length >= this.readBuf.Length)
                {
                    this.readBuf = null;
                }
                else
                {
                    int num3 = this.readBuf.Length - length;
                    byte[] destinationArray = new byte[num3];
                    Array.Copy(this.readBuf, length, destinationArray, 0, num3);
                    this.readBuf = destinationArray;
                }
                num += length;
                numToRead -= length;
                offset += length;
            }
            while (numToRead > 0)
            {
                byte[] sourceArray = this.buffer.ReadRecord();
                if (sourceArray == null)
                {
                    throw new IOException("unexpected EOF with " + numToRead + " bytes unread");
                }
                int num4 = numToRead;
                int num5 = sourceArray.Length;
                if (num5 > num4)
                {
                    Array.Copy(sourceArray, 0, buf, offset, num4);
                    this.readBuf = new byte[num5 - num4];
                    Array.Copy(sourceArray, num4, this.readBuf, 0, num5 - num4);
                }
                else
                {
                    num4 = num5;
                    Array.Copy(sourceArray, 0, buf, offset, num5);
                }
                num += num4;
                numToRead -= num4;
                offset += num4;
            }
            this.entryOffset += num;
            return num;
        }

        public override int ReadByte()
        {
            byte[] buffer = new byte[1];
            if (this.Read(buffer, 0, 1) <= 0)
            {
                return -1;
            }
            return buffer[0];
        }

        public void Reset()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.inputStream.Seek(offset, origin);
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

        public void SetEntryFactory(IEntryFactory factory)
        {
            this.eFactory = factory;
        }

        public override void SetLength(long val)
        {
            this.inputStream.SetLength(val);
        }

        public void Skip(int numToSkip)
        {
            int num2;
            byte[] buffer = new byte[0x2000];
            for (int i = numToSkip; i > 0; i -= num2)
            {
                num2 = this.Read(buffer, 0, (i > buffer.Length) ? buffer.Length : i);
                if (num2 == -1)
                {
                    return;
                }
            }
        }

        public override void Write(byte[] array, int offset, int count)
        {
            this.inputStream.Write(array, offset, count);
        }

        public override void WriteByte(byte val)
        {
            this.inputStream.WriteByte(val);
        }

        public int Available
        {
            get
            {
                return (this.entrySize - this.entryOffset);
            }
        }

        public override bool CanRead
        {
            get
            {
                return this.inputStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.inputStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.inputStream.CanWrite;
            }
        }

        public bool IsMarkSupported
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return this.inputStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.inputStream.Position;
            }
            set
            {
                this.inputStream.Position = value;
            }
        }

        public class EntryFactoryAdapter : TarInputStream.IEntryFactory
        {
            public TarEntry CreateEntry(string name)
            {
                return TarEntry.CreateTarEntry(name);
            }

            public TarEntry CreateEntry(byte[] headerBuf)
            {
                return new TarEntry(headerBuf);
            }

            public TarEntry CreateEntryFromFile(string fileName)
            {
                return TarEntry.CreateEntryFromFile(fileName);
            }
        }

        public interface IEntryFactory
        {
            TarEntry CreateEntry(string name);
            TarEntry CreateEntry(byte[] headerBuf);
            TarEntry CreateEntryFromFile(string fileName);
        }
    }
}

