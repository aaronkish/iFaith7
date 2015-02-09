namespace Ionic.Zlib
{
    using System;

    internal sealed class InflateBlocks
    {
        private const int BAD = 9;
        internal int[] bb = new int[1];
        internal int bitb;
        internal int bitk;
        internal int[] blens;
        internal static readonly int[] border = new int[] { 
            0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
            14, 1, 15
         };
        private const int BTREE = 4;
        internal long check;
        internal object checkfn;
        internal InflateCodes codes = new InflateCodes();
        private const int CODES = 6;
        private const int DONE = 8;
        private const int DRY = 7;
        private const int DTREE = 5;
        internal int end;
        internal int[] hufts = new int[0x10e0];
        internal int index;
        private static readonly int[] inflate_mask = new int[] { 
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
            0xffff
         };
        internal InfTree inftree = new InfTree();
        internal int last;
        internal int left;
        private const int LENS = 1;
        private const int MANY = 0x5a0;
        internal int mode;
        internal int read;
        private const int STORED = 2;
        internal int table;
        private const int TABLE = 3;
        internal int[] tb = new int[1];
        private const int TYPE = 0;
        internal byte[] window;
        internal int write;

        internal InflateBlocks(ZlibCodec z, object checkfn, int w)
        {
            this.window = new byte[w];
            this.end = w;
            this.checkfn = checkfn;
            this.mode = 0;
            this.Reset(z, null);
        }

        internal int Flush(ZlibCodec z, int r)
        {
            int nextOut = z.NextOut;
            int read = this.read;
            int len = ((read <= this.write) ? this.write : this.end) - read;
            if (len > z.AvailableBytesOut)
            {
                len = z.AvailableBytesOut;
            }
            if ((len != 0) && (r == -5))
            {
                r = 0;
            }
            z.AvailableBytesOut -= len;
            z.TotalBytesOut += len;
            if (this.checkfn != null)
            {
                z._Adler32 = this.check = Adler.Adler32(this.check, this.window, read, len);
            }
            Array.Copy(this.window, read, z.OutputBuffer, nextOut, len);
            nextOut += len;
            read += len;
            if (read == this.end)
            {
                read = 0;
                if (this.write == this.end)
                {
                    this.write = 0;
                }
                len = this.write - read;
                if (len > z.AvailableBytesOut)
                {
                    len = z.AvailableBytesOut;
                }
                if ((len != 0) && (r == -5))
                {
                    r = 0;
                }
                z.AvailableBytesOut -= len;
                z.TotalBytesOut += len;
                if (this.checkfn != null)
                {
                    z._Adler32 = this.check = Adler.Adler32(this.check, this.window, read, len);
                }
                Array.Copy(this.window, read, z.OutputBuffer, nextOut, len);
                nextOut += len;
                read += len;
            }
            z.NextOut = nextOut;
            this.read = read;
            return r;
        }

        internal void Free(ZlibCodec z)
        {
            this.Reset(z, null);
            this.window = null;
            this.hufts = null;
        }

        internal int Process(ZlibCodec z, int r)
        {
            bool flag = false;
            int[] numArray;
            int[] numArray2;
            int num11;
            int nextIn = z.NextIn;
            int availableBytesIn = z.AvailableBytesIn;
            int bitb = this.bitb;
            int bitk = this.bitk;
            int write = this.write;
            int num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
            goto Label_1121;
        Label_08EB:
            while (this.index < (4 + SharedUtils.URShift(this.table, 10)))
            {
                while (bitk < 3)
                {
                    if (availableBytesIn != 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        this.bitb = bitb;
                        this.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        this.write = write;
                        return this.Flush(z, r);
                    }
                    availableBytesIn--;
                    bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                    bitk += 8;
                }
                this.blens[border[this.index++]] = bitb & 7;
                bitb = SharedUtils.URShift(bitb, 3);
                bitk -= 3;
            }
            while (this.index < 0x13)
            {
                this.blens[border[this.index++]] = 0;
            }
            this.bb[0] = 7;
            int index = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
            if (index != 0)
            {
                r = index;
                if (r == -3)
                {
                    this.blens = null;
                    this.mode = 9;
                }
                this.bitb = bitb;
                this.bitk = bitk;
                z.AvailableBytesIn = availableBytesIn;
                z.TotalBytesIn += nextIn - z.NextIn;
                z.NextIn = nextIn;
                this.write = write;
                return this.Flush(z, r);
            }
            this.index = 0;
            this.mode = 5;
        Label_0D6F:
            flag = true;
            index = this.table;
            if (this.index >= ((0x102 + (index & 0x1f)) + ((index >> 5) & 0x1f)))
            {
                this.tb[0] = -1;
                numArray = new int[] { 9 };
                numArray2 = new int[] { 6 };
                int[] tl = new int[1];
                int[] td = new int[1];
                index = this.table;
                index = this.inftree.inflate_trees_dynamic(0x101 + (index & 0x1f), 1 + ((index >> 5) & 0x1f), this.blens, numArray, numArray2, tl, td, this.hufts, z);
                if (index != 0)
                {
                    if (index == -3)
                    {
                        this.blens = null;
                        this.mode = 9;
                    }
                    r = index;
                    this.bitb = bitb;
                    this.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    this.write = write;
                    return this.Flush(z, r);
                }
                this.codes.Init(numArray[0], numArray2[0], this.hufts, tl[0], this.hufts, td[0], z);
                this.mode = 6;
            }
            else
            {
                index = this.bb[0];
                while (bitk < index)
                {
                    if (availableBytesIn != 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        this.bitb = bitb;
                        this.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        this.write = write;
                        return this.Flush(z, r);
                    }
                    availableBytesIn--;
                    bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                    bitk += 8;
                }
                if (this.tb[0] == -1)
                {
                }
                index = this.hufts[((this.tb[0] + (bitb & inflate_mask[index])) * 3) + 1];
                int num12 = this.hufts[((this.tb[0] + (bitb & inflate_mask[index])) * 3) + 2];
                if (num12 < 0x10)
                {
                    bitb = SharedUtils.URShift(bitb, index);
                    bitk -= index;
                    this.blens[this.index++] = num12;
                }
                else
                {
                    num11 = (num12 == 0x12) ? 7 : (num12 - 14);
                    int num13 = (num12 == 0x12) ? 11 : 3;
                    while (bitk < (index + num11))
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            this.write = write;
                            return this.Flush(z, r);
                        }
                        availableBytesIn--;
                        bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    bitb = SharedUtils.URShift(bitb, index);
                    bitk -= index;
                    num13 += bitb & inflate_mask[num11];
                    bitb = SharedUtils.URShift(bitb, num11);
                    bitk -= num11;
                    num11 = this.index;
                    index = this.table;
                    if (((num11 + num13) > ((0x102 + (index & 0x1f)) + ((index >> 5) & 0x1f))) || ((num12 == 0x10) && (num11 < 1)))
                    {
                        this.blens = null;
                        this.mode = 9;
                        z.Message = "invalid bit length repeat";
                        r = -3;
                        this.bitb = bitb;
                        this.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        this.write = write;
                        return this.Flush(z, r);
                    }
                    num12 = (num12 == 0x10) ? this.blens[num11 - 1] : 0;
                    do
                    {
                        this.blens[num11++] = num12;
                    }
                    while (--num13 != 0);
                    this.index = num11;
                }
                goto Label_0D6F;
            }
        Label_0EA7:
            this.bitb = bitb;
            this.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            this.write = write;
            if ((r = this.codes.Process(this, z, r)) != 1)
            {
                return this.Flush(z, r);
            }
            r = 0;
            nextIn = z.NextIn;
            availableBytesIn = z.AvailableBytesIn;
            bitb = this.bitb;
            bitk = this.bitk;
            write = this.write;
            num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
            if (this.last == 0)
            {
                this.mode = 0;
                goto Label_1121;
            }
            this.mode = 7;
        Label_0F8B:
            this.write = write;
            r = this.Flush(z, r);
            write = this.write;
            num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
            if (this.read != this.write)
            {
                this.bitb = bitb;
                this.bitk = bitk;
                z.AvailableBytesIn = availableBytesIn;
                z.TotalBytesIn += nextIn - z.NextIn;
                z.NextIn = nextIn;
                this.write = write;
                return this.Flush(z, r);
            }
            this.mode = 8;
        Label_103A:
            r = 1;
            this.bitb = bitb;
            this.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            this.write = write;
            return this.Flush(z, r);
        Label_1121:
            flag = true;
            switch (this.mode)
            {
                case 0:
                    while (bitk < 3)
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            this.write = write;
                            return this.Flush(z, r);
                        }
                        availableBytesIn--;
                        bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    index = bitb & 7;
                    this.last = index & 1;
                    switch (SharedUtils.URShift(index, 1))
                    {
                        case 0:
                            bitb = SharedUtils.URShift(bitb, 3);
                            bitk -= 3;
                            index = bitk & 7;
                            bitb = SharedUtils.URShift(bitb, index);
                            bitk -= index;
                            this.mode = 1;
                            break;

                        case 1:
                        {
                            numArray = new int[1];
                            numArray2 = new int[1];
                            int[][] numArray3 = new int[1][];
                            int[][] numArray4 = new int[1][];
                            InfTree.inflate_trees_fixed(numArray, numArray2, numArray3, numArray4, z);
                            this.codes.Init(numArray[0], numArray2[0], numArray3[0], 0, numArray4[0], 0, z);
                            bitb = SharedUtils.URShift(bitb, 3);
                            bitk -= 3;
                            this.mode = 6;
                            break;
                        }
                        case 2:
                            bitb = SharedUtils.URShift(bitb, 3);
                            bitk -= 3;
                            this.mode = 3;
                            break;

                        case 3:
                            bitb = SharedUtils.URShift(bitb, 3);
                            bitk -= 3;
                            this.mode = 9;
                            z.Message = "invalid block type";
                            r = -3;
                            this.bitb = bitb;
                            this.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            this.write = write;
                            return this.Flush(z, r);
                    }
                    goto Label_1121;

                case 1:
                    while (bitk < 0x20)
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            this.write = write;
                            return this.Flush(z, r);
                        }
                        availableBytesIn--;
                        bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    if ((SharedUtils.URShift(~bitb, 0x10) & 0xffff) != (bitb & 0xffff))
                    {
                        this.mode = 9;
                        z.Message = "invalid stored block lengths";
                        r = -3;
                        this.bitb = bitb;
                        this.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        this.write = write;
                        return this.Flush(z, r);
                    }
                    this.left = bitb & 0xffff;
                    bitb = bitk = 0;
                    this.mode = (this.left != 0) ? 2 : ((this.last != 0) ? 7 : 0);
                    goto Label_1121;

                case 2:
                    if (availableBytesIn != 0)
                    {
                        if (num6 == 0)
                        {
                            if ((write == this.end) && (this.read != 0))
                            {
                                write = 0;
                                num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
                            }
                            if (num6 == 0)
                            {
                                this.write = write;
                                r = this.Flush(z, r);
                                write = this.write;
                                num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
                                if ((write == this.end) && (this.read != 0))
                                {
                                    write = 0;
                                    num6 = (write < this.read) ? ((this.read - write) - 1) : (this.end - write);
                                }
                                if (num6 == 0)
                                {
                                    this.bitb = bitb;
                                    this.bitk = bitk;
                                    z.AvailableBytesIn = availableBytesIn;
                                    z.TotalBytesIn += nextIn - z.NextIn;
                                    z.NextIn = nextIn;
                                    this.write = write;
                                    return this.Flush(z, r);
                                }
                            }
                        }
                        r = 0;
                        index = this.left;
                        if (index > availableBytesIn)
                        {
                            index = availableBytesIn;
                        }
                        if (index > num6)
                        {
                            index = num6;
                        }
                        Array.Copy(z.InputBuffer, nextIn, this.window, write, index);
                        nextIn += index;
                        availableBytesIn -= index;
                        write += index;
                        num6 -= index;
                        this.left -= index;
                        if (this.left == 0)
                        {
                            this.mode = (this.last != 0) ? 7 : 0;
                        }
                        goto Label_1121;
                    }
                    this.bitb = bitb;
                    this.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    this.write = write;
                    return this.Flush(z, r);

                case 3:
                    while (bitk < 14)
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            this.bitb = bitb;
                            this.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            this.write = write;
                            return this.Flush(z, r);
                        }
                        availableBytesIn--;
                        bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    this.table = index = bitb & 0x3fff;
                    if (((index & 0x1f) > 0x1d) || (((index >> 5) & 0x1f) > 0x1d))
                    {
                        this.mode = 9;
                        z.Message = "too many length or distance symbols";
                        r = -3;
                        this.bitb = bitb;
                        this.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        this.write = write;
                        return this.Flush(z, r);
                    }
                    index = (0x102 + (index & 0x1f)) + ((index >> 5) & 0x1f);
                    if ((this.blens == null) || (this.blens.Length < index))
                    {
                        this.blens = new int[index];
                    }
                    else
                    {
                        for (num11 = 0; num11 < index; num11++)
                        {
                            this.blens[num11] = 0;
                        }
                    }
                    bitb = SharedUtils.URShift(bitb, 14);
                    bitk -= 14;
                    this.index = 0;
                    this.mode = 4;
                    goto Label_08EB;

                case 4:
                    goto Label_08EB;

                case 5:
                    goto Label_0D6F;

                case 6:
                    goto Label_0EA7;

                case 7:
                    goto Label_0F8B;

                case 8:
                    goto Label_103A;

                case 9:
                    r = -3;
                    this.bitb = bitb;
                    this.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    this.write = write;
                    return this.Flush(z, r);
            }
            r = -2;
            this.bitb = bitb;
            this.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            this.write = write;
            return this.Flush(z, r);
        }

        internal void Reset(ZlibCodec z, long[] c)
        {
            if (c != null)
            {
                c[0] = this.check;
            }
            if ((this.mode == 4) || (this.mode == 5))
            {
            }
            if (this.mode == 6)
            {
            }
            this.mode = 0;
            this.bitk = 0;
            this.bitb = 0;
            this.read = this.write = 0;
            if (this.checkfn != null)
            {
                z._Adler32 = this.check = Adler.Adler32(0L, null, 0, 0);
            }
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy(d, start, this.window, 0, n);
            this.read = this.write = n;
        }

        internal int SyncPoint()
        {
            return ((this.mode == 1) ? 1 : 0);
        }
    }
}

