namespace Ionic.Zlib
{
    using System;

    public sealed class ZlibCodec
    {
        internal long _Adler32;
        public int AvailableBytesIn;
        public int AvailableBytesOut;
        internal DeflateManager dstate;
        public byte[] InputBuffer;
        internal InflateManager istate;
        public string Message;
        public int NextIn;
        public int NextOut;
        public byte[] OutputBuffer;
        public long TotalBytesIn;
        public long TotalBytesOut;

        public ZlibCodec()
        {
        }

        public ZlibCodec(CompressionMode mode)
        {
            if (mode == CompressionMode.Compress)
            {
                if (this.InitializeDeflate() != 0)
                {
                    throw new ZlibException("Cannot initialize for deflate.");
                }
            }
            else
            {
                if (mode != CompressionMode.Decompress)
                {
                    throw new ZlibException("Invalid ZlibStreamFlavor.");
                }
                if (this.InitializeInflate() != 0)
                {
                    throw new ZlibException("Cannot initialize for inflate.");
                }
            }
        }

        public int Deflate(int flush)
        {
            if (this.dstate == null)
            {
                throw new ZlibException("No Deflate State!");
            }
            return this.dstate.Deflate(this, flush);
        }

        public int EndDeflate()
        {
            if (this.dstate == null)
            {
                throw new ZlibException("No Deflate State!");
            }
            int num = this.dstate.End();
            this.dstate = null;
            return num;
        }

        public int EndInflate()
        {
            if (this.istate == null)
            {
                throw new ZlibException("No Inflate State!");
            }
            int num = this.istate.End(this);
            this.istate = null;
            return num;
        }

        internal void flush_pending()
        {
            int pendingCount = this.dstate.pendingCount;
            if (pendingCount > this.AvailableBytesOut)
            {
                pendingCount = this.AvailableBytesOut;
            }
            if (pendingCount != 0)
            {
                if ((((this.dstate.pending.Length <= this.dstate.nextPending) || (this.OutputBuffer.Length <= this.NextOut)) || (this.dstate.pending.Length < (this.dstate.nextPending + pendingCount))) || (this.OutputBuffer.Length < (this.NextOut + pendingCount)))
                {
                    throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
                }
                Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, pendingCount);
                this.NextOut += pendingCount;
                this.dstate.nextPending += pendingCount;
                this.TotalBytesOut += pendingCount;
                this.AvailableBytesOut -= pendingCount;
                this.dstate.pendingCount -= pendingCount;
                if (this.dstate.pendingCount == 0)
                {
                    this.dstate.nextPending = 0;
                }
            }
        }

        public int Inflate(int f)
        {
            if (this.istate == null)
            {
                throw new ZlibException("No Inflate State!");
            }
            return this.istate.Inflate(this, f);
        }

        public int InitializeDeflate()
        {
            return this.InitializeDeflate(CompressionLevel.DEFAULT, 15);
        }

        public int InitializeDeflate(CompressionLevel level)
        {
            return this.InitializeDeflate(level, 15);
        }

        public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
        {
            return this.InitializeDeflate(level, 15, wantRfc1950Header);
        }

        public int InitializeDeflate(CompressionLevel level, int bits)
        {
            return this.InitializeDeflate(level, bits, true);
        }

        public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
        {
            if (this.istate != null)
            {
                throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
            }
            this.dstate = new DeflateManager();
            this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
            return this.dstate.Initialize(this, level, bits);
        }

        public int InitializeInflate()
        {
            return this.InitializeInflate(15);
        }

        public int InitializeInflate(bool expectRfc1950Header)
        {
            return this.InitializeInflate(15, expectRfc1950Header);
        }

        public int InitializeInflate(int windowBits)
        {
            return this.InitializeInflate(windowBits, true);
        }

        public int InitializeInflate(int windowBits, bool expectRfc1950Header)
        {
            if (this.dstate != null)
            {
                throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
            }
            this.istate = new InflateManager(expectRfc1950Header);
            return this.istate.Initialize(this, windowBits);
        }

        internal int read_buf(byte[] buf, int start, int size)
        {
            int availableBytesIn = this.AvailableBytesIn;
            if (availableBytesIn > size)
            {
                availableBytesIn = size;
            }
            if (availableBytesIn == 0)
            {
                return 0;
            }
            this.AvailableBytesIn -= availableBytesIn;
            if (this.dstate.WantRfc1950HeaderBytes)
            {
                this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, availableBytesIn);
            }
            Array.Copy(this.InputBuffer, this.NextIn, buf, start, availableBytesIn);
            this.NextIn += availableBytesIn;
            this.TotalBytesIn += availableBytesIn;
            return availableBytesIn;
        }

        public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
        {
            if (this.dstate == null)
            {
                throw new ZlibException("No Deflate State!");
            }
            return this.dstate.SetParams(this, level, strategy);
        }

        public int SetDictionary(byte[] dictionary)
        {
            if (this.istate != null)
            {
                return this.istate.SetDictionary(this, dictionary);
            }
            if (this.dstate == null)
            {
                throw new ZlibException("No Inflate or Deflate state!");
            }
            return this.dstate.SetDictionary(this, dictionary);
        }

        public int SyncInflate()
        {
            if (this.istate == null)
            {
                throw new ZlibException("No Inflate State!");
            }
            return this.istate.Sync(this);
        }

        public long Adler32
        {
            get
            {
                return this._Adler32;
            }
        }
    }
}

