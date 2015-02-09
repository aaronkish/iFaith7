namespace Ionic.Zlib
{
    using System;

    internal sealed class InflateCodes
    {
        private const int BADCODE = 9;
        private const int COPY = 5;
        internal byte dbits;
        internal int dist;
        private const int DIST = 3;
        private const int DISTEXT = 4;
        internal int[] dtree;
        internal int dtree_index;
        private const int END = 8;
        internal int get_Renamed;
        private static readonly int[] inflate_mask = new int[] { 
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
            0xffff
         };
        internal byte lbits;
        internal int len;
        private const int LEN = 1;
        private const int LENEXT = 2;
        internal int lit;
        private const int LIT = 6;
        internal int[] ltree;
        internal int ltree_index;
        internal int mode;
        internal int need;
        private const int START = 0;
        internal int[] tree;
        internal int tree_index = 0;
        private const int WASH = 7;

        internal InflateCodes()
        {
        }

        internal int InflateFast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InflateBlocks s, ZlibCodec z)
        {
            int num13;
            int nextIn = z.NextIn;
            int availableBytesIn = z.AvailableBytesIn;
            int bitb = s.bitb;
            int bitk = s.bitk;
            int write = s.write;
            int num6 = (write < s.read) ? ((s.read - write) - 1) : (s.end - write);
            int num7 = inflate_mask[bl];
            int num8 = inflate_mask[bd];
            do
            {
                while (bitk < 20)
                {
                    availableBytesIn--;
                    bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                    bitk += 8;
                }
                int num9 = bitb & num7;
                int[] numArray = tl;
                int num10 = tl_index;
                int index = (num10 + num9) * 3;
                int num12 = numArray[index];
                if (num12 == 0)
                {
                    bitb = bitb >> numArray[index + 1];
                    bitk -= numArray[index + 1];
                    s.window[write++] = (byte) numArray[index + 2];
                    num6--;
                }
                else
                {
                    while (true)
                    {
                        bool flag = false;
                        bitb = bitb >> numArray[index + 1];
                        bitk -= numArray[index + 1];
                        if ((num12 & 0x10) != 0)
                        {
                            num12 &= 15;
                            num13 = numArray[index + 2] + (bitb & inflate_mask[num12]);
                            bitb = bitb >> num12;
                            bitk -= num12;
                            while (bitk < 15)
                            {
                                availableBytesIn--;
                                bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                                bitk += 8;
                            }
                            num9 = bitb & num8;
                            numArray = td;
                            num10 = td_index;
                            index = (num10 + num9) * 3;
                            num12 = numArray[index];
                            while (true)
                            {
                                bitb = bitb >> numArray[index + 1];
                                bitk -= numArray[index + 1];
                                if ((num12 & 0x10) != 0)
                                {
                                    int num15;
                                    num12 &= 15;
                                    while (bitk < num12)
                                    {
                                        availableBytesIn--;
                                        bitb |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                                        bitk += 8;
                                    }
                                    int num14 = numArray[index + 2] + (bitb & inflate_mask[num12]);
                                    bitb = bitb >> num12;
                                    bitk -= num12;
                                    num6 -= num13;
                                    if (write >= num14)
                                    {
                                        num15 = write - num14;
                                        if (((write - num15) > 0) && (2 > (write - num15)))
                                        {
                                            s.window[write++] = s.window[num15++];
                                            s.window[write++] = s.window[num15++];
                                            num13 -= 2;
                                        }
                                        else
                                        {
                                            Array.Copy(s.window, num15, s.window, write, 2);
                                            write += 2;
                                            num15 += 2;
                                            num13 -= 2;
                                        }
                                    }
                                    else
                                    {
                                        num15 = write - num14;
                                        do
                                        {
                                            num15 += s.end;
                                        }
                                        while (num15 < 0);
                                        num12 = s.end - num15;
                                        if (num13 > num12)
                                        {
                                            num13 -= num12;
                                            if (((write - num15) > 0) && (num12 > (write - num15)))
                                            {
                                                do
                                                {
                                                    s.window[write++] = s.window[num15++];
                                                }
                                                while (--num12 != 0);
                                            }
                                            else
                                            {
                                                Array.Copy(s.window, num15, s.window, write, num12);
                                                write += num12;
                                                num15 += num12;
                                                num12 = 0;
                                            }
                                            num15 = 0;
                                        }
                                    }
                                    if (((write - num15) > 0) && (num13 > (write - num15)))
                                    {
                                        do
                                        {
                                            s.window[write++] = s.window[num15++];
                                        }
                                        while (--num13 != 0);
                                    }
                                    else
                                    {
                                        Array.Copy(s.window, num15, s.window, write, num13);
                                        write += num13;
                                        num15 += num13;
                                        num13 = 0;
                                    }
                                    break;
                                }
                                if ((num12 & 0x40) == 0)
                                {
                                    num9 += numArray[index + 2];
                                    num9 += bitb & inflate_mask[num12];
                                    index = (num10 + num9) * 3;
                                    num12 = numArray[index];
                                }
                                else
                                {
                                    z.Message = "invalid distance code";
                                    num13 = z.AvailableBytesIn - availableBytesIn;
                                    num13 = ((bitk >> 3) < num13) ? (bitk >> 3) : num13;
                                    availableBytesIn += num13;
                                    nextIn -= num13;
                                    bitk -= num13 << 3;
                                    s.bitb = bitb;
                                    s.bitk = bitk;
                                    z.AvailableBytesIn = availableBytesIn;
                                    z.TotalBytesIn += nextIn - z.NextIn;
                                    z.NextIn = nextIn;
                                    s.write = write;
                                    return -3;
                                }
                                flag = true;
                            }
                        }
                        if ((num12 & 0x40) == 0)
                        {
                            num9 += numArray[index + 2];
                            num9 += bitb & inflate_mask[num12];
                            index = (num10 + num9) * 3;
                            num12 = numArray[index];
                            if (num12 == 0)
                            {
                                bitb = bitb >> numArray[index + 1];
                                bitk -= numArray[index + 1];
                                s.window[write++] = (byte) numArray[index + 2];
                                num6--;
                                break;
                            }
                        }
                        else
                        {
                            if ((num12 & 0x20) != 0)
                            {
                                num13 = z.AvailableBytesIn - availableBytesIn;
                                num13 = ((bitk >> 3) < num13) ? (bitk >> 3) : num13;
                                availableBytesIn += num13;
                                nextIn -= num13;
                                bitk -= num13 << 3;
                                s.bitb = bitb;
                                s.bitk = bitk;
                                z.AvailableBytesIn = availableBytesIn;
                                z.TotalBytesIn += nextIn - z.NextIn;
                                z.NextIn = nextIn;
                                s.write = write;
                                return 1;
                            }
                            z.Message = "invalid literal/length code";
                            num13 = z.AvailableBytesIn - availableBytesIn;
                            num13 = ((bitk >> 3) < num13) ? (bitk >> 3) : num13;
                            availableBytesIn += num13;
                            nextIn -= num13;
                            bitk -= num13 << 3;
                            s.bitb = bitb;
                            s.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            s.write = write;
                            return -3;
                        }
                        flag = true;
                    }
                }
            }
            while ((num6 >= 0x102) && (availableBytesIn >= 10));
            num13 = z.AvailableBytesIn - availableBytesIn;
            num13 = ((bitk >> 3) < num13) ? (bitk >> 3) : num13;
            availableBytesIn += num13;
            nextIn -= num13;
            bitk -= num13 << 3;
            s.bitb = bitb;
            s.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            s.write = write;
            return 0;
        }

        internal void Init(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZlibCodec z)
        {
            this.mode = 0;
            this.lbits = (byte) bl;
            this.dbits = (byte) bd;
            this.ltree = tl;
            this.ltree_index = tl_index;
            this.dtree = td;
            this.dtree_index = td_index;
            this.tree = null;
        }

        internal int Process(InflateBlocks blocks, ZlibCodec z, int r)
        {
            bool flag;
            int num8;
            int num12;
            int number = 0;
            int bitk = 0;
            int nextIn = 0;
            nextIn = z.NextIn;
            int availableBytesIn = z.AvailableBytesIn;
            number = blocks.bitb;
            bitk = blocks.bitk;
            int write = blocks.write;
            int num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
            goto Label_0C74;
        Label_0193:
            this.need = this.lbits;
            this.tree = this.ltree;
            this.tree_index = this.ltree_index;
            this.mode = 1;
        Label_01C3:
            num8 = this.need;
            while (bitk < num8)
            {
                if (availableBytesIn != 0)
                {
                    r = 0;
                }
                else
                {
                    blocks.bitb = number;
                    blocks.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    blocks.write = write;
                    return blocks.Flush(z, r);
                }
                availableBytesIn--;
                number |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                bitk += 8;
            }
            int index = (this.tree_index + (number & inflate_mask[num8])) * 3;
            number = SharedUtils.URShift(number, this.tree[index + 1]);
            bitk -= this.tree[index + 1];
            int num11 = this.tree[index];
            if (num11 == 0)
            {
                this.lit = this.tree[index + 2];
                this.mode = 6;
                goto Label_0C74;
            }
            if ((num11 & 0x10) != 0)
            {
                this.get_Renamed = num11 & 15;
                this.len = this.tree[index + 2];
                this.mode = 2;
                goto Label_0C74;
            }
            if ((num11 & 0x40) == 0)
            {
                this.need = num11;
                this.tree_index = (index / 3) + this.tree[index + 2];
                goto Label_0C74;
            }
            if ((num11 & 0x20) != 0)
            {
                this.mode = 7;
                goto Label_0C74;
            }
            this.mode = 9;
            z.Message = "invalid literal/length code";
            r = -3;
            blocks.bitb = number;
            blocks.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            blocks.write = write;
            return blocks.Flush(z, r);
        Label_04B2:
            num8 = this.need;
            while (bitk < num8)
            {
                if (availableBytesIn != 0)
                {
                    r = 0;
                }
                else
                {
                    blocks.bitb = number;
                    blocks.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    blocks.write = write;
                    return blocks.Flush(z, r);
                }
                availableBytesIn--;
                number |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                bitk += 8;
            }
            index = (this.tree_index + (number & inflate_mask[num8])) * 3;
            number = number >> this.tree[index + 1];
            bitk -= this.tree[index + 1];
            num11 = this.tree[index];
            if ((num11 & 0x10) != 0)
            {
                this.get_Renamed = num11 & 15;
                this.dist = this.tree[index + 2];
                this.mode = 4;
                goto Label_0C74;
            }
            if ((num11 & 0x40) == 0)
            {
                this.need = num11;
                this.tree_index = (index / 3) + this.tree[index + 2];
                goto Label_0C74;
            }
            this.mode = 9;
            z.Message = "invalid distance code";
            r = -3;
            blocks.bitb = number;
            blocks.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            blocks.write = write;
            return blocks.Flush(z, r);
        Label_072F:
            num12 = write - this.dist;
            while (num12 < 0)
            {
                num12 += blocks.end;
            }
            while (this.len != 0)
            {
                if (num6 == 0)
                {
                    if ((write == blocks.end) && (blocks.read != 0))
                    {
                        write = 0;
                        num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                    }
                    if (num6 == 0)
                    {
                        blocks.write = write;
                        r = blocks.Flush(z, r);
                        write = blocks.write;
                        num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                        if ((write == blocks.end) && (blocks.read != 0))
                        {
                            write = 0;
                            num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                        }
                        if (num6 == 0)
                        {
                            blocks.bitb = number;
                            blocks.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            blocks.write = write;
                            return blocks.Flush(z, r);
                        }
                    }
                }
                blocks.window[write++] = blocks.window[num12++];
                num6--;
                if (num12 == blocks.end)
                {
                    num12 = 0;
                }
                this.len--;
            }
            this.mode = 0;
            goto Label_0C74;
        Label_0B8D:
            r = 1;
            blocks.bitb = number;
            blocks.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            blocks.write = write;
            return blocks.Flush(z, r);
        Label_0C74:
            flag = true;
            switch (this.mode)
            {
                case 0:
                    if ((num6 < 0x102) || (availableBytesIn < 10))
                    {
                        goto Label_0193;
                    }
                    blocks.bitb = number;
                    blocks.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    blocks.write = write;
                    r = this.InflateFast(this.lbits, this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, blocks, z);
                    nextIn = z.NextIn;
                    availableBytesIn = z.AvailableBytesIn;
                    number = blocks.bitb;
                    bitk = blocks.bitk;
                    write = blocks.write;
                    num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                    if (r == 0)
                    {
                        goto Label_0193;
                    }
                    this.mode = (r == 1) ? 7 : 9;
                    goto Label_0C74;

                case 1:
                    goto Label_01C3;

                case 2:
                    num8 = this.get_Renamed;
                    while (bitk < num8)
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            blocks.bitb = number;
                            blocks.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            blocks.write = write;
                            return blocks.Flush(z, r);
                        }
                        availableBytesIn--;
                        number |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    this.len += number & inflate_mask[num8];
                    number = number >> num8;
                    bitk -= num8;
                    this.need = this.dbits;
                    this.tree = this.dtree;
                    this.tree_index = this.dtree_index;
                    this.mode = 3;
                    goto Label_04B2;

                case 3:
                    goto Label_04B2;

                case 4:
                    num8 = this.get_Renamed;
                    while (bitk < num8)
                    {
                        if (availableBytesIn != 0)
                        {
                            r = 0;
                        }
                        else
                        {
                            blocks.bitb = number;
                            blocks.bitk = bitk;
                            z.AvailableBytesIn = availableBytesIn;
                            z.TotalBytesIn += nextIn - z.NextIn;
                            z.NextIn = nextIn;
                            blocks.write = write;
                            return blocks.Flush(z, r);
                        }
                        availableBytesIn--;
                        number |= (z.InputBuffer[nextIn++] & 0xff) << bitk;
                        bitk += 8;
                    }
                    this.dist += number & inflate_mask[num8];
                    number = number >> num8;
                    bitk -= num8;
                    this.mode = 5;
                    goto Label_072F;

                case 5:
                    goto Label_072F;

                case 6:
                    if (num6 == 0)
                    {
                        if ((write == blocks.end) && (blocks.read != 0))
                        {
                            write = 0;
                            num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                        }
                        if (num6 == 0)
                        {
                            blocks.write = write;
                            r = blocks.Flush(z, r);
                            write = blocks.write;
                            num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                            if ((write == blocks.end) && (blocks.read != 0))
                            {
                                write = 0;
                                num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                            }
                            if (num6 == 0)
                            {
                                blocks.bitb = number;
                                blocks.bitk = bitk;
                                z.AvailableBytesIn = availableBytesIn;
                                z.TotalBytesIn += nextIn - z.NextIn;
                                z.NextIn = nextIn;
                                blocks.write = write;
                                return blocks.Flush(z, r);
                            }
                        }
                    }
                    r = 0;
                    blocks.window[write++] = (byte) this.lit;
                    num6--;
                    this.mode = 0;
                    goto Label_0C74;

                case 7:
                    if (bitk > 7)
                    {
                        bitk -= 8;
                        availableBytesIn++;
                        nextIn--;
                    }
                    blocks.write = write;
                    r = blocks.Flush(z, r);
                    write = blocks.write;
                    num6 = (write < blocks.read) ? ((blocks.read - write) - 1) : (blocks.end - write);
                    if (blocks.read != blocks.write)
                    {
                        blocks.bitb = number;
                        blocks.bitk = bitk;
                        z.AvailableBytesIn = availableBytesIn;
                        z.TotalBytesIn += nextIn - z.NextIn;
                        z.NextIn = nextIn;
                        blocks.write = write;
                        return blocks.Flush(z, r);
                    }
                    this.mode = 8;
                    goto Label_0B8D;

                case 8:
                    goto Label_0B8D;

                case 9:
                    r = -3;
                    blocks.bitb = number;
                    blocks.bitk = bitk;
                    z.AvailableBytesIn = availableBytesIn;
                    z.TotalBytesIn += nextIn - z.NextIn;
                    z.NextIn = nextIn;
                    blocks.write = write;
                    return blocks.Flush(z, r);
            }
            r = -2;
            blocks.bitb = number;
            blocks.bitk = bitk;
            z.AvailableBytesIn = availableBytesIn;
            z.TotalBytesIn += nextIn - z.NextIn;
            z.NextIn = nextIn;
            blocks.write = write;
            return blocks.Flush(z, r);
        }
    }
}

