namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;

    public class DeflaterEngine : DeflaterConstants
    {
        private Adler32 adler;
        private int blockStart;
        private int comprFunc;
        private int goodLength;
        private short[] head;
        private DeflaterHuffman huffman;
        private byte[] inputBuf;
        private int inputEnd;
        private int inputOff;
        private int ins_h;
        private int lookahead;
        private int matchLen;
        private int matchStart;
        private int max_chain;
        private int max_lazy;
        private int niceLength;
        private DeflaterPending pending;
        private short[] prev;
        private bool prevAvailable;
        private DeflateStrategy strategy;
        private int strstart;
        private static int TOO_FAR = 4096;
        private int totalIn;
        private byte[] window;

        public DeflaterEngine(DeflaterPending pending)
        {
            this.pending = pending;
            this.huffman = new DeflaterHuffman(pending);
            this.adler = new Adler32();
            this.window = new byte[65536];
            this.head = new short[32768];
            this.prev = new short[32768];
            this.blockStart = this.strstart = 1;
        }

        public bool Deflate(bool flush, bool finish)
        {
            bool flag2;
            do
            {
                this.FillWindow();
                bool flag = flush && (this.inputOff == this.inputEnd);
                switch (this.comprFunc)
                {
                    case 0:
                        flag2 = this.DeflateStored(flag, finish);
                        break;

                    case 1:
                        flag2 = this.DeflateFast(flag, finish);
                        break;

                    case 2:
                        flag2 = this.DeflateSlow(flag, finish);
                        break;

                    default:
                        throw new InvalidOperationException("unknown comprFunc");
                }
            }
            while (this.pending.IsFlushed && flag2);
            return flag2;
        }

        private bool DeflateFast(bool flush, bool finish)
        {
            if ((this.lookahead >= 262) || flush)
            {
                while ((this.lookahead >= 262) || flush)
                {
                    int num;
                    if (this.lookahead == 0)
                    {
                        this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
                        this.blockStart = this.strstart;
                        return false;
                    }
                    if (this.strstart > 65274)
                    {
                        this.SlideWindow();
                    }
                    if ((((this.lookahead >= 3) && ((num = this.InsertString()) != 0)) && ((this.strategy != DeflateStrategy.HuffmanOnly) && ((this.strstart - num) <= 32506))) && this.FindLongestMatch(num))
                    {
                        this.huffman.TallyDist(this.strstart - this.matchStart, this.matchLen);
                        this.lookahead -= this.matchLen;
                        if ((this.matchLen <= this.max_lazy) && (this.lookahead >= 3))
                        {
                            while (--this.matchLen > 0)
                            {
                                this.strstart++;
                                this.InsertString();
                            }
                            this.strstart++;
                        }
                        else
                        {
                            this.strstart += this.matchLen;
                            if (this.lookahead >= 2)
                            {
                                this.UpdateHash();
                            }
                        }
                        this.matchLen = 2;
                    }
                    else
                    {
                        this.huffman.TallyLit(this.window[this.strstart] & 255);
                        this.strstart++;
                        this.lookahead--;
                        if (this.huffman.IsFull())
                        {
                            bool lastBlock = finish && (this.lookahead == 0);
                            this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, lastBlock);
                            this.blockStart = this.strstart;
                            return !lastBlock;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool DeflateSlow(bool flush, bool finish)
        {
            if ((this.lookahead >= 262) || flush)
            {
                while ((this.lookahead >= 262) || flush)
                {
                    if (this.lookahead == 0)
                    {
                        if (this.prevAvailable)
                        {
                            this.huffman.TallyLit(this.window[this.strstart - 1] & 255);
                        }
                        this.prevAvailable = false;
                        this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
                        this.blockStart = this.strstart;
                        return false;
                    }
                    if (this.strstart >= 65274)
                    {
                        this.SlideWindow();
                    }
                    int matchStart = this.matchStart;
                    int matchLen = this.matchLen;
                    if (this.lookahead >= 3)
                    {
                        int curMatch = this.InsertString();
                        if ((((this.strategy != DeflateStrategy.HuffmanOnly) && (curMatch != 0)) && (((this.strstart - curMatch) <= 32506) && this.FindLongestMatch(curMatch))) && ((this.matchLen <= 5) && ((this.strategy == DeflateStrategy.Filtered) || ((this.matchLen == 3) && ((this.strstart - this.matchStart) > TOO_FAR)))))
                        {
                            this.matchLen = 2;
                        }
                    }
                    if ((matchLen >= 3) && (this.matchLen <= matchLen))
                    {
                        this.huffman.TallyDist((this.strstart - 1) - matchStart, matchLen);
                        matchLen -= 2;
                        do
                        {
                            this.strstart++;
                            this.lookahead--;
                            if (this.lookahead >= 3)
                            {
                                this.InsertString();
                            }
                        }
                        while (--matchLen > 0);
                        this.strstart++;
                        this.lookahead--;
                        this.prevAvailable = false;
                        this.matchLen = 2;
                    }
                    else
                    {
                        if (this.prevAvailable)
                        {
                            this.huffman.TallyLit(this.window[this.strstart - 1] & 255);
                        }
                        this.prevAvailable = true;
                        this.strstart++;
                        this.lookahead--;
                    }
                    if (this.huffman.IsFull())
                    {
                        int num4 = this.strstart - this.blockStart;
                        if (this.prevAvailable)
                        {
                            num4--;
                        }
                        bool lastBlock = (finish && (this.lookahead == 0)) && !this.prevAvailable;
                        this.huffman.FlushBlock(this.window, this.blockStart, num4, lastBlock);
                        this.blockStart += num4;
                        return !lastBlock;
                    }
                }
                return true;
            }
            return false;
        }

        private bool DeflateStored(bool flush, bool finish)
        {
            if (!flush && (this.lookahead == 0))
            {
                return false;
            }
            this.strstart += this.lookahead;
            this.lookahead = 0;
            int num = this.strstart - this.blockStart;
            if (((num < DeflaterConstants.MAX_BLOCK_SIZE) && ((this.blockStart >= 32768) || (num < 32506))) && !flush)
            {
                return true;
            }
            bool lastBlock = finish;
            if (num > DeflaterConstants.MAX_BLOCK_SIZE)
            {
                num = DeflaterConstants.MAX_BLOCK_SIZE;
                lastBlock = false;
            }
            this.huffman.FlushStoredBlock(this.window, this.blockStart, num, lastBlock);
            this.blockStart += num;
            return !lastBlock;
        }

        public void FillWindow()
        {
            if (this.strstart >= 65274)
            {
                this.SlideWindow();
            }
            while ((this.lookahead < 262) && (this.inputOff < this.inputEnd))
            {
                int length = (65536 - this.lookahead) - this.strstart;
                if (length > (this.inputEnd - this.inputOff))
                {
                    length = this.inputEnd - this.inputOff;
                }
                Array.Copy(this.inputBuf, this.inputOff, this.window, this.strstart + this.lookahead, length);
                this.adler.Update(this.inputBuf, this.inputOff, length);
                this.inputOff += length;
                this.totalIn += length;
                this.lookahead += length;
            }
            if (this.lookahead >= 3)
            {
                this.UpdateHash();
            }
        }

        private bool FindLongestMatch(int curMatch)
        {
            int num = this.max_chain;
            int niceLength = this.niceLength;
            short[] prev = this.prev;
            int strstart = this.strstart;
            int index = this.strstart + this.matchLen;
            int num5 = Math.Max(this.matchLen, 2);
            int num6 = Math.Max(this.strstart - 32506, 0);
            int num7 = (this.strstart + 258) - 1;
            byte num8 = this.window[index - 1];
            byte num9 = this.window[index];
            if (num5 >= this.goodLength)
            {
                num = num >> 2;
            }
            if (niceLength > this.lookahead)
            {
                niceLength = this.lookahead;
            }
            do
            {
                if (((this.window[curMatch + num5] == num9) && (this.window[(curMatch + num5) - 1] == num8)) && ((this.window[curMatch] == this.window[strstart]) && (this.window[curMatch + 1] == this.window[strstart + 1])))
                {
                    int num10 = curMatch + 2;
                    strstart += 2;
                    while ((((this.window[++strstart] == this.window[++num10]) && (this.window[++strstart] == this.window[++num10])) && ((this.window[++strstart] == this.window[++num10]) && (this.window[++strstart] == this.window[++num10]))) && (((this.window[++strstart] == this.window[++num10]) && (this.window[++strstart] == this.window[++num10])) && (((this.window[++strstart] == this.window[++num10]) && (this.window[++strstart] == this.window[++num10])) && (strstart < num7))))
                    {
                    }
                    if (strstart > index)
                    {
                        this.matchStart = curMatch;
                        index = strstart;
                        num5 = strstart - this.strstart;
                        if (num5 >= niceLength)
                        {
                            break;
                        }
                        num8 = this.window[index - 1];
                        num9 = this.window[index];
                    }
                    strstart = this.strstart;
                }
            }
            while (((curMatch = prev[curMatch & 32767] & 65535) > num6) && (--num != 0));
            this.matchLen = Math.Min(num5, this.lookahead);
            return (this.matchLen >= 3);
        }

        private int InsertString()
        {
            short num2;
            int index = ((this.ins_h << 5) ^ this.window[this.strstart + 2]) & 32767;
            this.prev[this.strstart & 32767] = num2 = this.head[index];
            this.head[index] = (short) this.strstart;
            this.ins_h = index;
            return (num2 & 65535);
        }

        public bool NeedsInput()
        {
            return (this.inputEnd == this.inputOff);
        }

        public void Reset()
        {
            this.huffman.Reset();
            this.adler.Reset();
            this.blockStart = this.strstart = 1;
            this.lookahead = 0;
            this.totalIn = 0;
            this.prevAvailable = false;
            this.matchLen = 2;
            for (int i = 0; i < 32768; i++)
            {
                this.head[i] = 0;
            }
            for (int j = 0; j < 32768; j++)
            {
                this.prev[j] = 0;
            }
        }

        public void ResetAdler()
        {
            this.adler.Reset();
        }

        public void SetDictionary(byte[] buffer, int offset, int length)
        {
            this.adler.Update(buffer, offset, length);
            if (length >= 3)
            {
                if (length > 32506)
                {
                    offset += length - 32506;
                    length = 32506;
                }
                Array.Copy(buffer, offset, this.window, this.strstart, length);
                this.UpdateHash();
                length--;
                while (--length > 0)
                {
                    this.InsertString();
                    this.strstart++;
                }
                this.strstart += 2;
                this.blockStart = this.strstart;
            }
        }

        public void SetInput(byte[] buf, int off, int len)
        {
            if (this.inputOff < this.inputEnd)
            {
                throw new InvalidOperationException("Old input was not completely processed");
            }
            int num = off + len;
            if (((0 > off) || (off > num)) || (num > buf.Length))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.inputBuf = buf;
            this.inputOff = off;
            this.inputEnd = num;
        }

        public void SetLevel(int lvl)
        {
            this.goodLength = DeflaterConstants.GOOD_LENGTH[lvl];
            this.max_lazy = DeflaterConstants.MAX_LAZY[lvl];
            this.niceLength = DeflaterConstants.NICE_LENGTH[lvl];
            this.max_chain = DeflaterConstants.MAX_CHAIN[lvl];
            if (DeflaterConstants.COMPR_FUNC[lvl] != this.comprFunc)
            {
                switch (this.comprFunc)
                {
                    case 0:
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushStoredBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        this.UpdateHash();
                        break;

                    case 1:
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        break;

                    case 2:
                        if (this.prevAvailable)
                        {
                            this.huffman.TallyLit(this.window[this.strstart - 1] & 255);
                        }
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        this.prevAvailable = false;
                        this.matchLen = 2;
                        break;
                }
                this.comprFunc = DeflaterConstants.COMPR_FUNC[lvl];
            }
        }

        private void SlideWindow()
        {
            Array.Copy(this.window, 32768, this.window, 0, 32768);
            this.matchStart -= 32768;
            this.strstart -= 32768;
            this.blockStart -= 32768;
            for (int i = 0; i < 32768; i++)
            {
                int num2 = this.head[i] & 65535;
                this.head[i] = (num2 >= 32768) ? ((short)(num2 - 32768)) : ((short)0);
            }
            for (int j = 0; j < 32768; j++)
            {
                int num4 = this.prev[j] & 65535;
                this.prev[j] = (num4 >= 32768) ? ((short)(num4 - 32768)) : ((short)0);
            }
        }

        private void UpdateHash()
        {
            this.ins_h = (this.window[this.strstart] << 5) ^ this.window[this.strstart + 1];
        }

        public int Adler
        {
            get
            {
                return (int) this.adler.Value;
            }
        }

        public DeflateStrategy Strategy
        {
            get
            {
                return this.strategy;
            }
            set
            {
                this.strategy = value;
            }
        }

        public int TotalIn
        {
            get
            {
                return this.totalIn;
            }
        }
    }
}

