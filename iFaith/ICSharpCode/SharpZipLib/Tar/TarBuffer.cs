namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;

    public class TarBuffer
    {
        private byte[] blockBuffer;
        private int blockSize;
        private int currBlkIdx;
        private int currRecIdx;
        private bool debug;
        public static readonly int DEFAULT_BLKSIZE = (DEFAULT_RCDSIZE * 20);
        public static readonly int DEFAULT_RCDSIZE = 0x200;
        private Stream inputStream;
        private Stream outputStream;
        private int recordSize;
        private int recsPerBlock;

        protected TarBuffer()
        {
        }

        public void Close()
        {
            bool debug = this.debug;
            if (this.outputStream != null)
            {
                this.Flush();
                this.outputStream.Close();
                this.outputStream = null;
            }
            else if (this.inputStream != null)
            {
                this.inputStream.Close();
                this.inputStream = null;
            }
        }

        public static TarBuffer CreateInputTarBuffer(Stream inputStream)
        {
            return CreateInputTarBuffer(inputStream, DEFAULT_BLKSIZE);
        }

        public static TarBuffer CreateInputTarBuffer(Stream inputStream, int blockSize)
        {
            return CreateInputTarBuffer(inputStream, blockSize, DEFAULT_RCDSIZE);
        }

        public static TarBuffer CreateInputTarBuffer(Stream inputStream, int blockSize, int recordSize)
        {
            TarBuffer buffer = new TarBuffer();
            buffer.inputStream = inputStream;
            buffer.outputStream = null;
            buffer.Initialize(blockSize, recordSize);
            return buffer;
        }

        public static TarBuffer CreateOutputTarBuffer(Stream outputStream)
        {
            return CreateOutputTarBuffer(outputStream, DEFAULT_BLKSIZE);
        }

        public static TarBuffer CreateOutputTarBuffer(Stream outputStream, int blockSize)
        {
            return CreateOutputTarBuffer(outputStream, blockSize, DEFAULT_RCDSIZE);
        }

        public static TarBuffer CreateOutputTarBuffer(Stream outputStream, int blockSize, int recordSize)
        {
            TarBuffer buffer = new TarBuffer();
            buffer.inputStream = null;
            buffer.outputStream = outputStream;
            buffer.Initialize(blockSize, recordSize);
            return buffer;
        }

        private void Flush()
        {
            bool debug = this.debug;
            if (this.outputStream == null)
            {
                throw new IOException("no output base stream defined");
            }
            if (this.currRecIdx > 0)
            {
                this.WriteBlock();
            }
            this.outputStream.Flush();
        }

        public int GetBlockSize()
        {
            return this.blockSize;
        }

        public int GetCurrentBlockNum()
        {
            return this.currBlkIdx;
        }

        public int GetCurrentRecordNum()
        {
            return (this.currRecIdx - 1);
        }

        public int GetRecordSize()
        {
            return this.recordSize;
        }

        private void Initialize(int blockSize, int recordSize)
        {
            this.debug = false;
            this.blockSize = blockSize;
            this.recordSize = recordSize;
            this.recsPerBlock = this.blockSize / this.recordSize;
            this.blockBuffer = new byte[this.blockSize];
            if (this.inputStream != null)
            {
                this.currBlkIdx = -1;
                this.currRecIdx = this.recsPerBlock;
            }
            else
            {
                this.currBlkIdx = 0;
                this.currRecIdx = 0;
            }
        }

        public bool IsEOFRecord(byte[] record)
        {
            int index = 0;
            int recordSize = this.GetRecordSize();
            while (index < recordSize)
            {
                if (record[index] != 0)
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        private bool ReadBlock()
        {
            bool debug = this.debug;
            if (this.inputStream == null)
            {
                throw new IOException("no input stream stream defined");
            }
            this.currRecIdx = 0;
            int offset = 0;
            int blockSize = this.blockSize;
            while (blockSize > 0)
            {
                long num3 = this.inputStream.Read(this.blockBuffer, offset, blockSize);
                if (num3 <= 0L)
                {
                    break;
                }
                offset += (int) num3;
                blockSize -= (int) num3;
                if (num3 != this.blockSize)
                {
                    bool flag2 = this.debug;
                }
            }
            this.currBlkIdx++;
            return true;
        }

        public byte[] ReadRecord()
        {
            bool debug = this.debug;
            if (this.inputStream == null)
            {
                throw new IOException("no input stream defined");
            }
            if ((this.currRecIdx >= this.recsPerBlock) && !this.ReadBlock())
            {
                return null;
            }
            byte[] destinationArray = new byte[this.recordSize];
            Array.Copy(this.blockBuffer, this.currRecIdx * this.recordSize, destinationArray, 0, this.recordSize);
            this.currRecIdx++;
            return destinationArray;
        }

        public void SetDebug(bool debug)
        {
            this.debug = debug;
        }

        public void SkipRecord()
        {
            bool debug = this.debug;
            if (this.inputStream == null)
            {
                throw new IOException("no input stream defined");
            }
            if ((this.currRecIdx < this.recsPerBlock) || this.ReadBlock())
            {
                this.currRecIdx++;
            }
        }

        private void WriteBlock()
        {
            bool debug = this.debug;
            if (this.outputStream == null)
            {
                throw new IOException("no output stream defined");
            }
            this.outputStream.Write(this.blockBuffer, 0, this.blockSize);
            this.outputStream.Flush();
            this.currRecIdx = 0;
            this.currBlkIdx++;
        }

        public void WriteRecord(byte[] record)
        {
            bool debug = this.debug;
            if (this.outputStream == null)
            {
                throw new IOException("no output stream defined");
            }
            if (record.Length != this.recordSize)
            {
                throw new IOException(string.Concat(new object[] { "record to write has length '", record.Length, "' which is not the record size of '", this.recordSize, "'" }));
            }
            if (this.currRecIdx >= this.recsPerBlock)
            {
                this.WriteBlock();
            }
            Array.Copy(record, 0, this.blockBuffer, this.currRecIdx * this.recordSize, this.recordSize);
            this.currRecIdx++;
        }

        public void WriteRecord(byte[] buf, int offset)
        {
            bool debug = this.debug;
            if (this.outputStream == null)
            {
                throw new IOException("no output stream stream defined");
            }
            if ((offset + this.recordSize) > buf.Length)
            {
                throw new IOException(string.Concat(new object[] { "record has length '", buf.Length, "' with offset '", offset, "' which is less than the record size of '", this.recordSize, "'" }));
            }
            if (this.currRecIdx >= this.recsPerBlock)
            {
                this.WriteBlock();
            }
            Array.Copy(buf, offset, this.blockBuffer, this.currRecIdx * this.recordSize, this.recordSize);
            this.currRecIdx++;
        }
    }
}

