namespace Ionic.Zlib
{
    using System;

    internal sealed class InflateManager
    {
        private bool _handleRfc1950HeaderBytes;
        private const int BAD = 13;
        internal InflateBlocks blocks;
        private const int BLOCKS = 7;
        private const int CHECK1 = 11;
        private const int CHECK2 = 10;
        private const int CHECK3 = 9;
        private const int CHECK4 = 8;
        private const int DICT0 = 6;
        private const int DICT1 = 5;
        private const int DICT2 = 4;
        private const int DICT3 = 3;
        private const int DICT4 = 2;
        private const int DONE = 12;
        private const int FLAG = 1;
        private static byte[] mark;
        internal int marker;
        internal int method;
        private const int METHOD = 0;
        internal int mode;
        internal long need;
        private const int PRESET_DICT = 0x20;
        internal long[] was;
        internal int wbits;
        private const int Z_DEFLATED = 8;

        static InflateManager()
        {
            byte[] buffer = new byte[4];
            buffer[2] = 0xff;
            buffer[3] = 0xff;
            mark = buffer;
        }

        public InflateManager()
        {
            this.was = new long[1];
            this._handleRfc1950HeaderBytes = true;
        }

        public InflateManager(bool expectRfc1950HeaderBytes)
        {
            this.was = new long[1];
            this._handleRfc1950HeaderBytes = true;
            this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
        }

        internal int End(ZlibCodec z)
        {
            if (this.blocks != null)
            {
                this.blocks.Free(z);
            }
            this.blocks = null;
            return 0;
        }

        internal int Inflate(ZlibCodec z, int f)
        {
            bool flag = false;
            if (z == null)
            {
                throw new ZlibException("Codec is null. ");
            }
            if (z.istate == null)
            {
                throw new ZlibException("InflateManager is null. ");
            }
            if (z.InputBuffer == null)
            {
                throw new ZlibException("InputBuffer is null. ");
            }
            f = (f == 4) ? -5 : 0;
            int r = -5;
            goto Label_0816;
        Label_01BA:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            int num5 = z.InputBuffer[z.NextIn++] & 0xff;
            if ((((z.istate.method << 8) + num5) % 0x1f) != 0)
            {
                z.istate.mode = 13;
                z.Message = "incorrect header check";
                z.istate.marker = 5;
                goto Label_0816;
            }
            if ((num5 & 0x20) == 0)
            {
                z.istate.mode = 7;
                goto Label_0816;
            }
            z.istate.mode = 2;
        Label_0291:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need = ((z.InputBuffer[z.NextIn++] & 0xff) << 0x18) & -16777216;
            z.istate.mode = 3;
        Label_0310:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += ((z.InputBuffer[z.NextIn++] & 0xff) << 0x10) & 0xff0000L;
            z.istate.mode = 4;
        Label_0397:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += ((z.InputBuffer[z.NextIn++] & 0xff) << 8) & 0xff00L;
            z.istate.mode = 5;
        Label_041D:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += z.InputBuffer[z.NextIn++] & 0xffL;
            z._Adler32 = z.istate.need;
            z.istate.mode = 6;
            return 2;
        Label_0592:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need = ((z.InputBuffer[z.NextIn++] & 0xff) << 0x18) & -16777216;
            z.istate.mode = 9;
        Label_0612:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += ((z.InputBuffer[z.NextIn++] & 0xff) << 0x10) & 0xff0000L;
            z.istate.mode = 10;
        Label_069A:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += ((z.InputBuffer[z.NextIn++] & 0xff) << 8) & 0xff00L;
            z.istate.mode = 11;
        Label_0721:
            if (z.AvailableBytesIn == 0)
            {
                return r;
            }
            r = f;
            z.AvailableBytesIn--;
            z.TotalBytesIn += 1L;
            z.istate.need += z.InputBuffer[z.NextIn++] & 0xffL;
            if (((int) z.istate.was[0]) != ((int) z.istate.need))
            {
                z.istate.mode = 13;
                z.Message = "incorrect data check";
                z.istate.marker = 5;
                goto Label_0816;
            }
            z.istate.mode = 12;
        Label_07ED:
            return 1;
        Label_0816:
            flag = true;
            switch (z.istate.mode)
            {
                case 0:
                    if (z.AvailableBytesIn != 0)
                    {
                        r = f;
                        z.AvailableBytesIn--;
                        z.TotalBytesIn += 1L;
                        if (((z.istate.method = z.InputBuffer[z.NextIn++]) & 15) != 8)
                        {
                            z.istate.mode = 13;
                            z.Message = "unknown compression method";
                            z.istate.marker = 5;
                            goto Label_0816;
                        }
                        if (((z.istate.method >> 4) + 8) > z.istate.wbits)
                        {
                            z.istate.mode = 13;
                            z.Message = "invalid window size";
                            z.istate.marker = 5;
                            goto Label_0816;
                        }
                        z.istate.mode = 1;
                        goto Label_01BA;
                    }
                    return r;

                case 1:
                    goto Label_01BA;

                case 2:
                    goto Label_0291;

                case 3:
                    goto Label_0310;

                case 4:
                    goto Label_0397;

                case 5:
                    goto Label_041D;

                case 6:
                    z.istate.mode = 13;
                    z.Message = "need dictionary";
                    z.istate.marker = 0;
                    return -2;

                case 7:
                    r = z.istate.blocks.Process(z, r);
                    if (r != -3)
                    {
                        if (r == 0)
                        {
                            r = f;
                        }
                        if (r != 1)
                        {
                            return r;
                        }
                        r = f;
                        z.istate.blocks.Reset(z, z.istate.was);
                        if (!z.istate.HandleRfc1950HeaderBytes)
                        {
                            z.istate.mode = 12;
                            goto Label_0816;
                        }
                        z.istate.mode = 8;
                        goto Label_0592;
                    }
                    z.istate.mode = 13;
                    z.istate.marker = 0;
                    goto Label_0816;

                case 8:
                    goto Label_0592;

                case 9:
                    goto Label_0612;

                case 10:
                    goto Label_069A;

                case 11:
                    goto Label_0721;

                case 12:
                    goto Label_07ED;

                case 13:
                    throw new ZlibException(string.Format("Bad state ({0})", z.Message));
            }
            throw new ZlibException("Stream error.");
        }

        internal int Initialize(ZlibCodec z, int w)
        {
            z.Message = null;
            this.blocks = null;
            if ((w < 8) || (w > 15))
            {
                this.End(z);
                throw new ZlibException("Bad window size.");
            }
            this.wbits = w;
            z.istate.blocks = new InflateBlocks(z, z.istate.HandleRfc1950HeaderBytes ? this : null, ((int) 1) << w);
            this.Reset(z);
            return 0;
        }

        internal int Reset(ZlibCodec z)
        {
            if (z == null)
            {
                throw new ZlibException("Codec is null.");
            }
            if (z.istate == null)
            {
                throw new ZlibException("InflateManager is null.");
            }
            z.TotalBytesIn = z.TotalBytesOut = 0L;
            z.Message = null;
            z.istate.mode = z.istate.HandleRfc1950HeaderBytes ? 0 : 7;
            z.istate.blocks.Reset(z, null);
            return 0;
        }

        internal int SetDictionary(ZlibCodec z, byte[] dictionary)
        {
            int start = 0;
            int length = dictionary.Length;
            if (((z == null) || (z.istate == null)) || (z.istate.mode != 6))
            {
                throw new ZlibException("Stream error.");
            }
            if (Adler.Adler32(1L, dictionary, 0, dictionary.Length) != z._Adler32)
            {
                return -3;
            }
            z._Adler32 = Adler.Adler32(0L, null, 0, 0);
            if (length >= (((int) 1) << z.istate.wbits))
            {
                length = (((int) 1) << z.istate.wbits) - 1;
                start = dictionary.Length - length;
            }
            z.istate.blocks.SetDictionary(dictionary, start, length);
            z.istate.mode = 7;
            return 0;
        }

        internal int Sync(ZlibCodec z)
        {
            if ((z == null) || (z.istate == null))
            {
                return -2;
            }
            if (z.istate.mode != 13)
            {
                z.istate.mode = 13;
                z.istate.marker = 0;
            }
            int availableBytesIn = z.AvailableBytesIn;
            if (availableBytesIn == 0)
            {
                return -5;
            }
            int nextIn = z.NextIn;
            int marker = z.istate.marker;
            while ((availableBytesIn != 0) && (marker < 4))
            {
                if (z.InputBuffer[nextIn] == mark[marker])
                {
                    marker++;
                }
                else if (z.InputBuffer[nextIn] != 0)
                {
                    marker = 0;
                }
                else
                {
                    marker = 4 - marker;
                }
                nextIn++;
                availableBytesIn--;
            }
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            z.AvailableBytesIn = availableBytesIn;
            z.istate.marker = marker;
            if (marker != 4)
            {
                return -3;
            }
            long totalBytesIn = z.TotalBytesIn;
            long totalBytesOut = z.TotalBytesOut;
            this.Reset(z);
            z.TotalBytesIn = totalBytesIn;
            z.TotalBytesOut = totalBytesOut;
            z.istate.mode = 7;
            return 0;
        }

        internal int SyncPoint(ZlibCodec z)
        {
            if (((z == null) || (z.istate == null)) || (z.istate.blocks == null))
            {
                return -2;
            }
            return z.istate.blocks.SyncPoint();
        }

        internal bool HandleRfc1950HeaderBytes
        {
            get
            {
                return this._handleRfc1950HeaderBytes;
            }
            set
            {
                this._handleRfc1950HeaderBytes = value;
            }
        }
    }
}

