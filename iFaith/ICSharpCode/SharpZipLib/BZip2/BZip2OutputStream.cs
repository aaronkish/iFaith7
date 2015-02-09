namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;
    using System.IO;

    public class BZip2OutputStream : Stream
    {
        private int allowableBlockSize;
        private Stream baseStream;
        private byte[] block;
        private uint blockCRC;
        private bool blockRandomised;
        private int blockSize100k;
        private int bsBuff;
        private int bsLive;
        private int bytesOut;
        private static readonly int CLEARMASK = ~SETMASK;
        private bool closed;
        private uint combinedCRC;
        private int currentChar;
        private static readonly int DEPTH_THRESH = 10;
        private bool firstAttempt;
        private int[] ftab;
        private static readonly int GREATER_ICOST = 15;
        private readonly int[] incs;
        private bool[] inUse;
        private int last;
        private static readonly int LESSER_ICOST = 0;
        private IChecksum mCrc;
        private int[] mtfFreq;
        private int nBlocksRandomised;
        private int nInUse;
        private int nMTF;
        private int origPtr;
        private static readonly int QSORT_STACK_SIZE = 0x3e8;
        private int[] quadrant;
        private int runLength;
        private char[] selector;
        private char[] selectorMtf;
        private char[] seqToUnseq;
        private static readonly int SETMASK = 0x200000;
        private static readonly int SMALL_THRESH = 20;
        private short[] szptr;
        private char[] unseqToSeq;
        private int workDone;
        private int workFactor;
        private int workLimit;
        private int[] zptr;

        public BZip2OutputStream(Stream inStream) : this(inStream, 9)
        {
        }

        public BZip2OutputStream(Stream inStream, int inBlockSize)
        {
            this.mCrc = new StrangeCRC();
            this.inUse = new bool[0x100];
            this.seqToUnseq = new char[0x100];
            this.unseqToSeq = new char[0x100];
            this.selector = new char[BZip2Constants.MAX_SELECTORS];
            this.selectorMtf = new char[BZip2Constants.MAX_SELECTORS];
            this.mtfFreq = new int[BZip2Constants.MAX_ALPHA_SIZE];
            this.currentChar = -1;
            this.runLength = 0;
            this.closed = false;
            this.incs = new int[] { 1, 4, 13, 40, 0x79, 0x16c, 0x445, 0xcd0, 0x2671, 0x7354, 0x159fd, 0x40df8, 0xc29e9, 0x247dbc };
            this.block = null;
            this.quadrant = null;
            this.zptr = null;
            this.ftab = null;
            this.BsSetStream(inStream);
            this.workFactor = 50;
            if (inBlockSize > 9)
            {
                inBlockSize = 9;
            }
            if (inBlockSize < 1)
            {
                inBlockSize = 1;
            }
            this.blockSize100k = inBlockSize;
            this.AllocateCompressStructures();
            this.Initialize();
            this.InitBlock();
        }

        private void AllocateCompressStructures()
        {
            int num = BZip2Constants.baseBlockSize * this.blockSize100k;
            this.block = new byte[(num + 1) + BZip2Constants.NUM_OVERSHOOT_BYTES];
            this.quadrant = new int[num + BZip2Constants.NUM_OVERSHOOT_BYTES];
            this.zptr = new int[num];
            this.ftab = new int[0x10001];
            if (((this.block != null) && (this.quadrant != null)) && (this.zptr != null))
            {
                int[] ftab = this.ftab;
            }
            this.szptr = new short[2 * num];
        }

        private void BsFinishedWithStream()
        {
            while (this.bsLive > 0)
            {
                int num = this.bsBuff >> 0x18;
                this.baseStream.WriteByte((byte) num);
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
        }

        private void BsPutint(int u)
        {
            this.BsW(8, (u >> 0x18) & 0xff);
            this.BsW(8, (u >> 0x10) & 0xff);
            this.BsW(8, (u >> 8) & 0xff);
            this.BsW(8, u & 0xff);
        }

        private void BsPutIntVS(int numBits, int c)
        {
            this.BsW(numBits, c);
        }

        private void BsPutUChar(int c)
        {
            this.BsW(8, c);
        }

        private void BsSetStream(Stream f)
        {
            this.baseStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
            this.bytesOut = 0;
        }

        private void BsW(int n, int v)
        {
            while (this.bsLive >= 8)
            {
                int num = this.bsBuff >> 0x18;
                this.baseStream.WriteByte((byte) num);
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
            this.bsBuff |= v << ((0x20 - this.bsLive) - n);
            this.bsLive += n;
        }

        public override void Close()
        {
            if (!this.closed)
            {
                if (this.runLength > 0)
                {
                    this.WriteRun();
                }
                this.currentChar = -1;
                this.EndBlock();
                this.EndCompression();
                this.closed = true;
                this.Flush();
                this.baseStream.Close();
            }
        }

        private void DoReversibleTransformation()
        {
            this.workLimit = this.workFactor * this.last;
            this.workDone = 0;
            this.blockRandomised = false;
            this.firstAttempt = true;
            this.MainSort();
            if ((this.workDone > this.workLimit) && this.firstAttempt)
            {
                this.RandomiseBlock();
                this.workLimit = this.workDone = 0;
                this.blockRandomised = true;
                this.firstAttempt = false;
                this.MainSort();
            }
            this.origPtr = -1;
            for (int i = 0; i <= this.last; i++)
            {
                if (this.zptr[i] == 0)
                {
                    this.origPtr = i;
                    break;
                }
            }
            if (this.origPtr == -1)
            {
                Panic();
            }
        }

        private void EndBlock()
        {
            this.blockCRC = (uint) this.mCrc.Value;
            this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 0x1f);
            this.combinedCRC ^= this.blockCRC;
            this.DoReversibleTransformation();
            this.BsPutUChar(0x31);
            this.BsPutUChar(0x41);
            this.BsPutUChar(0x59);
            this.BsPutUChar(0x26);
            this.BsPutUChar(0x53);
            this.BsPutUChar(0x59);
            this.BsPutint((int) this.blockCRC);
            if (this.blockRandomised)
            {
                this.BsW(1, 1);
                this.nBlocksRandomised++;
            }
            else
            {
                this.BsW(1, 0);
            }
            this.MoveToFrontCodeAndSend();
        }

        private void EndCompression()
        {
            this.BsPutUChar(0x17);
            this.BsPutUChar(0x72);
            this.BsPutUChar(0x45);
            this.BsPutUChar(0x38);
            this.BsPutUChar(80);
            this.BsPutUChar(0x90);
            this.BsPutint((int) this.combinedCRC);
            this.BsFinishedWithStream();
        }

        public void Finalize()
        {
            this.Close();
        }

        public override void Flush()
        {
            this.baseStream.Flush();
        }

        private bool FullGtU(int i1, int i2)
        {
            byte num = this.block[i1 + 1];
            byte num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            num = this.block[i1 + 1];
            num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            num = this.block[i1 + 1];
            num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            num = this.block[i1 + 1];
            num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            num = this.block[i1 + 1];
            num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            num = this.block[i1 + 1];
            num2 = this.block[i2 + 1];
            if (num != num2)
            {
                return (num > num2);
            }
            i1++;
            i2++;
            int num3 = this.last + 1;
            do
            {
                num = this.block[i1 + 1];
                num2 = this.block[i2 + 1];
                if (num != num2)
                {
                    return (num > num2);
                }
                int num4 = this.quadrant[i1];
                int num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num = this.block[i1 + 1];
                num2 = this.block[i2 + 1];
                if (num != num2)
                {
                    return (num > num2);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num = this.block[i1 + 1];
                num2 = this.block[i2 + 1];
                if (num != num2)
                {
                    return (num > num2);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num = this.block[i1 + 1];
                num2 = this.block[i2 + 1];
                if (num != num2)
                {
                    return (num > num2);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                if (i1 > this.last)
                {
                    i1 -= this.last;
                    i1--;
                }
                if (i2 > this.last)
                {
                    i2 -= this.last;
                    i2--;
                }
                num3 -= 4;
                this.workDone++;
            }
            while (num3 >= 0);
            return false;
        }

        private void GenerateMTFValues()
        {
            int num2;
            char[] chArray = new char[0x100];
            this.MakeMaps();
            int index = this.nInUse + 1;
            for (num2 = 0; num2 <= index; num2++)
            {
                this.mtfFreq[num2] = 0;
            }
            int num3 = 0;
            int num4 = 0;
            for (num2 = 0; num2 < this.nInUse; num2++)
            {
                chArray[num2] = (char) num2;
            }
            for (num2 = 0; num2 <= this.last; num2++)
            {
                IntPtr ptr;
                char ch = this.unseqToSeq[this.block[this.zptr[num2]]];
                int num5 = 0;
                char ch2 = chArray[num5];
                while (ch != ch2)
                {
                    num5++;
                    char ch3 = ch2;
                    ch2 = chArray[num5];
                    chArray[num5] = ch3;
                }
                chArray[0] = ch2;
                if (num5 == 0)
                {
                    num4++;
                    continue;
                }
                if (num4 <= 0)
                {
                    goto Label_014B;
                }
                num4--;
            Label_00BE:
                switch ((num4 % 2))
                {
                    case 0:
                        this.szptr[num3] = (short) BZip2Constants.RUNA;
                        num3++;
                        this.mtfFreq[(int) (ptr = (IntPtr) BZip2Constants.RUNA)] = this.mtfFreq[(int) ptr] + 1;
                        break;

                    case 1:
                        this.szptr[num3] = (short) BZip2Constants.RUNB;
                        num3++;
                        this.mtfFreq[(int) (ptr = (IntPtr) BZip2Constants.RUNB)] = this.mtfFreq[(int) ptr] + 1;
                        break;
                }
                if (num4 >= 2)
                {
                    num4 = (num4 - 2) / 2;
                    goto Label_00BE;
                }
                num4 = 0;
            Label_014B:
                this.szptr[num3] = (short) (num5 + 1);
                num3++;
                this.mtfFreq[(int) (ptr = (IntPtr) (num5 + 1))] = this.mtfFreq[(int) ptr] + 1;
            }
            if (num4 <= 0)
            {
                goto Label_021C;
            }
            num4--;
        Label_0192:
            switch ((num4 % 2))
            {
                case 0:
                    this.szptr[num3] = (short) BZip2Constants.RUNA;
                    num3++;
                    this.mtfFreq[BZip2Constants.RUNA]++;
                    break;

                case 1:
                    this.szptr[num3] = (short) BZip2Constants.RUNB;
                    num3++;
                    this.mtfFreq[BZip2Constants.RUNB]++;
                    break;
            }
            if (num4 >= 2)
            {
                num4 = (num4 - 2) / 2;
                goto Label_0192;
            }
        Label_021C:
            this.szptr[num3] = (short) index;
            num3++;
            this.mtfFreq[index]++;
            this.nMTF = num3;
        }

        private void HbAssignCodes(int[] code, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int num = 0;
            for (int i = minLen; i <= maxLen; i++)
            {
                for (int j = 0; j < alphaSize; j++)
                {
                    if (length[j] == i)
                    {
                        code[j] = num;
                        num++;
                    }
                }
                num = num << 1;
            }
        }

        private static void HbMakeCodeLengths(char[] len, int[] freq, int alphaSize, int maxLen)
        {
            int[] numArray = new int[BZip2Constants.MAX_ALPHA_SIZE + 2];
            int[] numArray2 = new int[BZip2Constants.MAX_ALPHA_SIZE * 2];
            int[] numArray3 = new int[BZip2Constants.MAX_ALPHA_SIZE * 2];
            for (int i = 0; i < alphaSize; i++)
            {
                numArray2[i + 1] = ((freq[i] == null) ? 1 : freq[i]) << 8;
            }
            while (true)
            {
                int num14;
                int index = alphaSize;
                int num3 = 0;
                numArray[0] = 0;
                numArray2[0] = 0;
                numArray3[0] = -2;
                for (int j = 1; j <= alphaSize; j++)
                {
                    numArray3[j] = -1;
                    num3++;
                    numArray[num3] = j;
                    int num5 = num3;
                    int num6 = numArray[num5];
                    while (numArray2[num6] < numArray2[numArray[num5 >> 1]])
                    {
                        numArray[num5] = numArray[num5 >> 1];
                        num5 = num5 >> 1;
                    }
                    numArray[num5] = num6;
                }
                if (num3 >= (BZip2Constants.MAX_ALPHA_SIZE + 2))
                {
                    Panic();
                }
                while (num3 > 1)
                {
                    int num7 = numArray[1];
                    numArray[1] = numArray[num3];
                    num3--;
                    int num8 = 1;
                    int num9 = 0;
                    int num10 = numArray[num8];
                Label_00F1:
                    num9 = num8 << 1;
                    if (num9 <= num3)
                    {
                        if ((num9 < num3) && (numArray2[numArray[num9 + 1]] < numArray2[numArray[num9]]))
                        {
                            num9++;
                        }
                        if (numArray2[num10] >= numArray2[numArray[num9]])
                        {
                            numArray[num8] = numArray[num9];
                            num8 = num9;
                            goto Label_00F1;
                        }
                    }
                    numArray[num8] = num10;
                    int num11 = numArray[1];
                    numArray[1] = numArray[num3];
                    num3--;
                    num8 = 1;
                    num9 = 0;
                    num10 = numArray[num8];
                Label_0163:
                    num9 = num8 << 1;
                    if (num9 <= num3)
                    {
                        if ((num9 < num3) && (numArray2[numArray[num9 + 1]] < numArray2[numArray[num9]]))
                        {
                            num9++;
                        }
                        if (numArray2[num10] >= numArray2[numArray[num9]])
                        {
                            numArray[num8] = numArray[num9];
                            num8 = num9;
                            goto Label_0163;
                        }
                    }
                    numArray[num8] = num10;
                    index++;
                    numArray3[num7] = numArray3[num11] = index;
                    numArray2[index] = ((int) ((numArray2[num7] & 0xffffff00L) + (numArray2[num11] & 0xffffff00L))) | (1 + (((numArray2[num7] & 0xff) > (numArray2[num11] & 0xff)) ? (numArray2[num7] & 0xff) : (numArray2[num11] & 0xff)));
                    numArray3[index] = -1;
                    num3++;
                    numArray[num3] = index;
                    num8 = num3;
                    num10 = numArray[num8];
                    while (numArray2[num10] < numArray2[numArray[num8 >> 1]])
                    {
                        numArray[num8] = numArray[num8 >> 1];
                        num8 = num8 >> 1;
                    }
                    numArray[num8] = num10;
                }
                if (index >= (BZip2Constants.MAX_ALPHA_SIZE * 2))
                {
                    Panic();
                }
                bool flag = false;
                for (int k = 1; k <= alphaSize; k++)
                {
                    num14 = 0;
                    int num15 = k;
                    while (numArray3[num15] >= 0)
                    {
                        num15 = numArray3[num15];
                        num14++;
                    }
                    len[k - 1] = (char) num14;
                    if (num14 > maxLen)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return;
                }
                for (int m = 1; m < alphaSize; m++)
                {
                    num14 = numArray2[m] >> 8;
                    num14 = 1 + (num14 / 2);
                    numArray2[m] = num14 << 8;
                }
            }
        }

        private void InitBlock()
        {
            this.mCrc.Reset();
            this.last = -1;
            for (int i = 0; i < 0x100; i++)
            {
                this.inUse[i] = false;
            }
            this.allowableBlockSize = (BZip2Constants.baseBlockSize * this.blockSize100k) - 20;
        }

        private void Initialize()
        {
            this.bytesOut = 0;
            this.nBlocksRandomised = 0;
            this.BsPutUChar(0x68);
            this.BsPutUChar(0x30 + this.blockSize100k);
            this.combinedCRC = 0;
        }

        private void MainSort()
        {
            int num;
            int[] numArray = new int[0x100];
            int[] numArray2 = new int[0x100];
            bool[] flagArray = new bool[0x100];
            for (num = 0; num < BZip2Constants.NUM_OVERSHOOT_BYTES; num++)
            {
                this.block[(this.last + num) + 2] = this.block[(num % (this.last + 1)) + 1];
            }
            for (num = 0; num <= (this.last + BZip2Constants.NUM_OVERSHOOT_BYTES); num++)
            {
                this.quadrant[num] = 0;
            }
            this.block[0] = this.block[this.last + 1];
            if (this.last < 0xfa0)
            {
                for (num = 0; num <= this.last; num++)
                {
                    this.zptr[num] = num;
                }
                this.firstAttempt = false;
                this.workDone = this.workLimit = 0;
                this.SimpleSort(0, this.last, 0);
            }
            else
            {
                int num5;
                IntPtr ptr;
                int num6;
                int num3 = 0;
                for (num = 0; num <= 0xff; num++)
                {
                    flagArray[num] = false;
                }
                for (num = 0; num <= 0x10000; num++)
                {
                    this.ftab[num] = 0;
                }
                int index = this.block[0];
                for (num = 0; num <= this.last; num++)
                {
                    num5 = this.block[num + 1];
                    this.ftab[(int) (ptr = (IntPtr) ((index << 8) + num5))] = this.ftab[(int) ptr] + 1;
                    index = num5;
                }
                for (num = 1; num <= 0x10000; num++)
                {
                    this.ftab[(int) (ptr = (IntPtr) num)] = this.ftab[(int) ptr] + this.ftab[num - 1];
                }
                index = this.block[1];
                for (num = 0; num < this.last; num++)
                {
                    num5 = this.block[num + 2];
                    num6 = (index << 8) + num5;
                    index = num5;
                    this.ftab[(int) (ptr = (IntPtr) num6)] = this.ftab[(int) ptr] - 1;
                    this.zptr[this.ftab[num6]] = num;
                }
                num6 = (this.block[this.last + 1] << 8) + this.block[1];
                this.ftab[(int) (ptr = (IntPtr) num6)] = this.ftab[(int) ptr] - 1;
                this.zptr[this.ftab[num6]] = this.last;
                num = 0;
                while (num <= 0xff)
                {
                    numArray[num] = num;
                    num++;
                }
                int num7 = 1;
                do
                {
                    num7 = (3 * num7) + 1;
                }
                while (num7 <= 0x100);
                do
                {
                    num7 /= 3;
                    num = num7;
                    while (num <= 0xff)
                    {
                        int num8 = numArray[num];
                        num6 = num;
                        while ((this.ftab[(numArray[num6 - num7] + 1) << 8] - this.ftab[numArray[num6 - num7] << 8]) > (this.ftab[(num8 + 1) << 8] - this.ftab[num8 << 8]))
                        {
                            numArray[num6] = numArray[num6 - num7];
                            num6 -= num7;
                            if (num6 <= (num7 - 1))
                            {
                                break;
                            }
                        }
                        numArray[num6] = num8;
                        num++;
                    }
                }
                while (num7 != 1);
                for (num = 0; num <= 0xff; num++)
                {
                    int num9 = numArray[num];
                    num6 = 0;
                    while (num6 <= 0xff)
                    {
                        int num10 = (num9 << 8) + num6;
                        if ((this.ftab[num10] & SETMASK) != SETMASK)
                        {
                            int loSt = this.ftab[num10] & CLEARMASK;
                            int hiSt = (this.ftab[num10 + 1] & CLEARMASK) - 1;
                            if (hiSt > loSt)
                            {
                                this.QSort3(loSt, hiSt, 2);
                                num3 += (hiSt - loSt) + 1;
                                if ((this.workDone > this.workLimit) && this.firstAttempt)
                                {
                                    return;
                                }
                            }
                            this.ftab[(int) (ptr = (IntPtr) num10)] = this.ftab[(int) ptr] | SETMASK;
                        }
                        num6++;
                    }
                    flagArray[num9] = true;
                    if (num < 0xff)
                    {
                        int num13 = this.ftab[num9 << 8] & CLEARMASK;
                        int num14 = (this.ftab[(num9 + 1) << 8] & CLEARMASK) - num13;
                        int num15 = 0;
                        while ((num14 >> num15) > 0xfffe)
                        {
                            num15++;
                        }
                        num6 = 0;
                        while (num6 < num14)
                        {
                            int num16 = this.zptr[num13 + num6];
                            int num17 = num6 >> num15;
                            this.quadrant[num16] = num17;
                            if (num16 < BZip2Constants.NUM_OVERSHOOT_BYTES)
                            {
                                this.quadrant[(num16 + this.last) + 1] = num17;
                            }
                            num6++;
                        }
                        if (((num14 - 1) >> num15) > 0xffff)
                        {
                            Panic();
                        }
                    }
                    num6 = 0;
                    while (num6 <= 0xff)
                    {
                        numArray2[num6] = this.ftab[(num6 << 8) + num9] & CLEARMASK;
                        num6++;
                    }
                    num6 = this.ftab[num9 << 8] & CLEARMASK;
                    while (num6 < (this.ftab[(num9 + 1) << 8] & CLEARMASK))
                    {
                        index = this.block[this.zptr[num6]];
                        if (!flagArray[index])
                        {
                            this.zptr[numArray2[index]] = (this.zptr[num6] == 0) ? this.last : (this.zptr[num6] - 1);
                            numArray2[(int) (ptr = (IntPtr) index)] = numArray2[(int) ptr] + 1;
                        }
                        num6++;
                    }
                    for (num6 = 0; num6 <= 0xff; num6++)
                    {
                        this.ftab[(int) (ptr = (IntPtr) ((num6 << 8) + num9))] = this.ftab[(int) ptr] | SETMASK;
                    }
                }
            }
        }

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int i = 0; i < 0x100; i++)
            {
                if (this.inUse[i])
                {
                    this.seqToUnseq[this.nInUse] = (char) i;
                    this.unseqToSeq[i] = (char) this.nInUse;
                    this.nInUse++;
                }
            }
        }

        private byte Med3(byte a, byte b, byte c)
        {
            byte num;
            if (a > b)
            {
                num = a;
                a = b;
                b = num;
            }
            if (b > c)
            {
                num = b;
                b = c;
                c = num;
            }
            if (a > b)
            {
                b = a;
            }
            return b;
        }

        private void MoveToFrontCodeAndSend()
        {
            this.BsPutIntVS(0x18, this.origPtr);
            this.GenerateMTFValues();
            this.SendMTFValues();
        }

        private static void Panic()
        {
        }

        private void QSort3(int loSt, int hiSt, int dSt)
        {
            StackElem[] elemArray = new StackElem[QSORT_STACK_SIZE];
            for (int i = 0; i < QSORT_STACK_SIZE; i++)
            {
                elemArray[i] = new StackElem();
            }
            int index = 0;
            elemArray[index].ll = loSt;
            elemArray[index].hh = hiSt;
            elemArray[index].dd = dSt;
            index++;
            while (index > 0)
            {
                int num7;
                int num9;
                int num11;
                if (index >= QSORT_STACK_SIZE)
                {
                    Panic();
                }
                index--;
                int ll = elemArray[index].ll;
                int hh = elemArray[index].hh;
                int dd = elemArray[index].dd;
                if (((hh - ll) < SMALL_THRESH) || (dd > DEPTH_THRESH))
                {
                    this.SimpleSort(ll, hh, dd);
                    if ((this.workDone > this.workLimit) && this.firstAttempt)
                    {
                        return;
                    }
                    continue;
                }
                int num6 = this.Med3(this.block[(this.zptr[ll] + dd) + 1], this.block[(this.zptr[hh] + dd) + 1], this.block[(this.zptr[(ll + hh) >> 1] + dd) + 1]);
                int num8 = num7 = ll;
                int num10 = num9 = hh;
            Label_0133:
                if (num8 <= num10)
                {
                    num11 = this.block[(this.zptr[num8] + dd) + 1] - num6;
                    if (num11 == 0)
                    {
                        int num12 = 0;
                        num12 = this.zptr[num8];
                        this.zptr[num8] = this.zptr[num7];
                        this.zptr[num7] = num12;
                        num7++;
                        num8++;
                        goto Label_0133;
                    }
                    if (num11 <= 0)
                    {
                        num8++;
                        goto Label_0133;
                    }
                }
            Label_01A6:
                if (num8 <= num10)
                {
                    num11 = this.block[(this.zptr[num10] + dd) + 1] - num6;
                    if (num11 == 0)
                    {
                        int num13 = 0;
                        num13 = this.zptr[num10];
                        this.zptr[num10] = this.zptr[num9];
                        this.zptr[num9] = num13;
                        num9--;
                        num10--;
                        goto Label_01A6;
                    }
                    if (num11 >= 0)
                    {
                        num10--;
                        goto Label_01A6;
                    }
                }
                if (num8 <= num10)
                {
                    int num14 = this.zptr[num8];
                    this.zptr[num8] = this.zptr[num10];
                    this.zptr[num10] = num14;
                    num8++;
                    num10--;
                    goto Label_0133;
                }
                if (num9 < num7)
                {
                    elemArray[index].ll = ll;
                    elemArray[index].hh = hh;
                    elemArray[index].dd = dd + 1;
                    index++;
                }
                else
                {
                    num11 = ((num7 - ll) < (num8 - num7)) ? (num7 - ll) : (num8 - num7);
                    this.Vswap(ll, num8 - num11, num11);
                    int n = ((hh - num9) < (num9 - num10)) ? (hh - num9) : (num9 - num10);
                    this.Vswap(num8, (hh - n) + 1, n);
                    num11 = ((ll + num8) - num7) - 1;
                    n = (hh - (num9 - num10)) + 1;
                    elemArray[index].ll = ll;
                    elemArray[index].hh = num11;
                    elemArray[index].dd = dd;
                    index++;
                    elemArray[index].ll = num11 + 1;
                    elemArray[index].hh = n - 1;
                    elemArray[index].dd = dd + 1;
                    index++;
                    elemArray[index].ll = n;
                    elemArray[index].hh = hh;
                    elemArray[index].dd = dd;
                    index++;
                }
            }
        }

        private void RandomiseBlock()
        {
            int num3;
            int num = 0;
            int index = 0;
            for (num3 = 0; num3 < 0x100; num3++)
            {
                this.inUse[num3] = false;
            }
            for (num3 = 0; num3 <= this.last; num3++)
            {
                byte[] buffer;
                IntPtr ptr;
                if (num == 0)
                {
                    num = BZip2Constants.rNums[index];
                    index++;
                    if (index == 0x200)
                    {
                        index = 0;
                    }
                }
                num--;
                (buffer = this.block)[(int) (ptr = (IntPtr) (num3 + 1))] = (byte) (buffer[(int) ptr] ^ ((num == 1) ? ((byte) 1) : ((byte) 0)));
                (buffer = this.block)[(int) (ptr = (IntPtr) (num3 + 1))] = (byte) (buffer[(int) ptr] & 0xff);
                this.inUse[this.block[num3 + 1]] = true;
            }
        }

        public override int Read(byte[] b, int off, int len)
        {
            return this.baseStream.Read(b, off, len);
        }

        public override int ReadByte()
        {
            return this.baseStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        private void SendMTFValues()
        {
            int num6;
            int num12;
            char[][] chArray = new char[BZip2Constants.N_GROUPS][];
            for (int i = 0; i < BZip2Constants.N_GROUPS; i++)
            {
                chArray[i] = new char[BZip2Constants.MAX_ALPHA_SIZE];
            }
            int index = 0;
            int alphaSize = this.nInUse + 2;
            for (int j = 0; j < BZip2Constants.N_GROUPS; j++)
            {
                for (int num5 = 0; num5 < alphaSize; num5++)
                {
                    chArray[j][num5] = (char) GREATER_ICOST;
                }
            }
            if (this.nMTF <= 0)
            {
                Panic();
            }
            if (this.nMTF < 200)
            {
                num6 = 2;
            }
            else if (this.nMTF < 600)
            {
                num6 = 3;
            }
            else if (this.nMTF < 0x4b0)
            {
                num6 = 4;
            }
            else if (this.nMTF < 0x960)
            {
                num6 = 5;
            }
            else
            {
                num6 = 6;
            }
            int num7 = num6;
            int nMTF = this.nMTF;
            int num9 = 0;
            while (num7 > 0)
            {
                int num10 = nMTF / num7;
                int num11 = 0;
                num12 = num9 - 1;
                while ((num11 < num10) && (num12 < (alphaSize - 1)))
                {
                    num12++;
                    num11 += this.mtfFreq[num12];
                }
                if (((num12 > num9) && (num7 != num6)) && ((num7 != 1) && (((num6 - num7) % 2) == 1)))
                {
                    num11 -= this.mtfFreq[num12];
                    num12--;
                }
                for (int num13 = 0; num13 < alphaSize; num13++)
                {
                    if ((num13 >= num9) && (num13 <= num12))
                    {
                        chArray[num7 - 1][num13] = (char) LESSER_ICOST;
                    }
                    else
                    {
                        chArray[num7 - 1][num13] = (char) GREATER_ICOST;
                    }
                }
                num7--;
                num9 = num12 + 1;
                nMTF -= num11;
            }
            int[][] numArray = new int[BZip2Constants.N_GROUPS][];
            for (int k = 0; k < BZip2Constants.N_GROUPS; k++)
            {
                numArray[k] = new int[BZip2Constants.MAX_ALPHA_SIZE];
            }
            int[] numArray2 = new int[BZip2Constants.N_GROUPS];
            short[] numArray3 = new short[BZip2Constants.N_GROUPS];
            for (int m = 0; m < BZip2Constants.N_ITERS; m++)
            {
                for (int num16 = 0; num16 < num6; num16++)
                {
                    numArray2[num16] = 0;
                }
                for (int num17 = 0; num17 < num6; num17++)
                {
                    for (int num18 = 0; num18 < alphaSize; num18++)
                    {
                        numArray[num17][num18] = 0;
                    }
                }
                index = 0;
                int num19 = 0;
                num9 = 0;
                while (true)
                {
                    IntPtr ptr;
                    int[] numArray5;
                    if (num9 >= this.nMTF)
                    {
                        break;
                    }
                    num12 = (num9 + BZip2Constants.G_SIZE) - 1;
                    if (num12 >= this.nMTF)
                    {
                        num12 = this.nMTF - 1;
                    }
                    for (int num20 = 0; num20 < num6; num20++)
                    {
                        numArray3[num20] = 0;
                    }
                    if (num6 == 6)
                    {
                        short num21;
                        short num22;
                        short num23;
                        short num24;
                        short num25;
                        short num26 = num25 = num24 = num23 = num22 = (short) (num21 = 0);
                        for (int num27 = num9; num27 <= num12; num27++)
                        {
                            short num28 = this.szptr[num27];
                            num26 = (short) (num26 + ((short) chArray[0][num28]));
                            num25 = (short) (num25 + ((short) chArray[1][num28]));
                            num24 = (short) (num24 + ((short) chArray[2][num28]));
                            num23 = (short) (num23 + ((short) chArray[3][num28]));
                            num22 = (short) (num22 + ((short) chArray[4][num28]));
                            num21 = (short) (num21 + ((short) chArray[5][num28]));
                        }
                        numArray3[0] = num26;
                        numArray3[1] = num25;
                        numArray3[2] = num24;
                        numArray3[3] = num23;
                        numArray3[4] = num22;
                        numArray3[5] = num21;
                    }
                    else
                    {
                        for (int num29 = num9; num29 <= num12; num29++)
                        {
                            short num30 = this.szptr[num29];
                            for (int num31 = 0; num31 < num6; num31++)
                            {
                                short[] numArray4;
                                (numArray4 = numArray3)[(int) (ptr = (IntPtr) num31)] = (short) (numArray4[(int) ptr] + ((short) chArray[num31][num30]));
                            }
                        }
                    }
                    int num32 = 0x3b9ac9ff;
                    int num33 = -1;
                    for (int num34 = 0; num34 < num6; num34++)
                    {
                        if (numArray3[num34] < num32)
                        {
                            num32 = numArray3[num34];
                            num33 = num34;
                        }
                    }
                    num19 += num32;
                    (numArray5 = numArray2)[(int) (ptr = (IntPtr) num33)] = numArray5[(int) ptr] + 1;
                    this.selector[index] = (char) num33;
                    index++;
                    for (int num35 = num9; num35 <= num12; num35++)
                    {
                        (numArray5 = numArray[num33])[(int) (ptr = (IntPtr) this.szptr[num35])] = numArray5[(int) ptr] + 1;
                    }
                    num9 = num12 + 1;
                }
                for (int num36 = 0; num36 < num6; num36++)
                {
                    HbMakeCodeLengths(chArray[num36], numArray[num36], alphaSize, 20);
                }
            }
            numArray = null;
            numArray2 = null;
            numArray3 = null;
            if (num6 >= 8)
            {
                Panic();
            }
            if ((index >= 0x8000) || (index > (2 + (0xdbba0 / BZip2Constants.G_SIZE))))
            {
                Panic();
            }
            char[] chArray2 = new char[BZip2Constants.N_GROUPS];
            for (int n = 0; n < num6; n++)
            {
                chArray2[n] = (char) n;
            }
            for (int num38 = 0; num38 < index; num38++)
            {
                char ch = this.selector[num38];
                int num39 = 0;
                char ch2 = chArray2[num39];
                while (ch != ch2)
                {
                    num39++;
                    char ch3 = ch2;
                    ch2 = chArray2[num39];
                    chArray2[num39] = ch3;
                }
                chArray2[0] = ch2;
                this.selectorMtf[num38] = (char) num39;
            }
            int[][] numArray6 = new int[BZip2Constants.N_GROUPS][];
            for (int num40 = 0; num40 < BZip2Constants.N_GROUPS; num40++)
            {
                numArray6[num40] = new int[BZip2Constants.MAX_ALPHA_SIZE];
            }
            for (int num41 = 0; num41 < num6; num41++)
            {
                int minLen = 0x20;
                int maxLen = 0;
                for (int num44 = 0; num44 < alphaSize; num44++)
                {
                    if (chArray[num41][num44] > maxLen)
                    {
                        maxLen = chArray[num41][num44];
                    }
                    if (chArray[num41][num44] < minLen)
                    {
                        minLen = chArray[num41][num44];
                    }
                }
                if (maxLen > 20)
                {
                    Panic();
                }
                if (minLen < 1)
                {
                    Panic();
                }
                this.HbAssignCodes(numArray6[num41], chArray[num41], minLen, maxLen, alphaSize);
            }
            bool[] flagArray = new bool[0x10];
            for (int num45 = 0; num45 < 0x10; num45++)
            {
                flagArray[num45] = false;
                for (int num46 = 0; num46 < 0x10; num46++)
                {
                    if (this.inUse[(num45 * 0x10) + num46])
                    {
                        flagArray[num45] = true;
                    }
                }
            }
            for (int num47 = 0; num47 < 0x10; num47++)
            {
                if (flagArray[num47])
                {
                    this.BsW(1, 1);
                }
                else
                {
                    this.BsW(1, 0);
                }
            }
            for (int num48 = 0; num48 < 0x10; num48++)
            {
                if (flagArray[num48])
                {
                    for (int num49 = 0; num49 < 0x10; num49++)
                    {
                        if (this.inUse[(num48 * 0x10) + num49])
                        {
                            this.BsW(1, 1);
                        }
                        else
                        {
                            this.BsW(1, 0);
                        }
                    }
                }
            }
            this.BsW(3, num6);
            this.BsW(15, index);
            for (int num50 = 0; num50 < index; num50++)
            {
                for (int num51 = 0; num51 < this.selectorMtf[num50]; num51++)
                {
                    this.BsW(1, 1);
                }
                this.BsW(1, 0);
            }
            for (int num52 = 0; num52 < num6; num52++)
            {
                int v = chArray[num52][0];
                this.BsW(5, v);
                int num54 = 0;
                goto Label_080D;
            Label_07C0:
                this.BsW(2, 2);
                v++;
            Label_07CE:
                if (v < chArray[num52][num54])
                {
                    goto Label_07C0;
                }
                while (v > chArray[num52][num54])
                {
                    this.BsW(2, 3);
                    v--;
                }
                this.BsW(1, 0);
                num54++;
            Label_080D:
                if (num54 < alphaSize)
                {
                    goto Label_07CE;
                }
            }
            int num55 = 0;
            num9 = 0;
            while (true)
            {
                if (num9 >= this.nMTF)
                {
                    break;
                }
                num12 = (num9 + BZip2Constants.G_SIZE) - 1;
                if (num12 >= this.nMTF)
                {
                    num12 = this.nMTF - 1;
                }
                for (int num56 = num9; num56 <= num12; num56++)
                {
                    this.BsW(chArray[this.selector[num55]][this.szptr[num56]], numArray6[this.selector[num55]][this.szptr[num56]]);
                }
                num9 = num12 + 1;
                num55++;
            }
            if (num55 != index)
            {
                Panic();
            }
        }

        public override void SetLength(long val)
        {
            this.baseStream.SetLength(val);
        }

        private void SimpleSort(int lo, int hi, int d)
        {
            int num = (hi - lo) + 1;
            if (num >= 2)
            {
                int index = 0;
                while (this.incs[index] < num)
                {
                    index++;
                }
                index--;
                while (index >= 0)
                {
                    int num3 = this.incs[index];
                    int num4 = lo + num3;
                    do
                    {
                        if (num4 > hi)
                        {
                            goto Label_018C;
                        }
                        int num5 = this.zptr[num4];
                        int num6 = num4;
                        while (this.FullGtU(this.zptr[num6 - num3] + d, num5 + d))
                        {
                            this.zptr[num6] = this.zptr[num6 - num3];
                            num6 -= num3;
                            if (num6 <= ((lo + num3) - 1))
                            {
                                break;
                            }
                        }
                        this.zptr[num6] = num5;
                        num4++;
                        if (num4 > hi)
                        {
                            goto Label_018C;
                        }
                        num5 = this.zptr[num4];
                        num6 = num4;
                        while (this.FullGtU(this.zptr[num6 - num3] + d, num5 + d))
                        {
                            this.zptr[num6] = this.zptr[num6 - num3];
                            num6 -= num3;
                            if (num6 <= ((lo + num3) - 1))
                            {
                                break;
                            }
                        }
                        this.zptr[num6] = num5;
                        num4++;
                        if (num4 > hi)
                        {
                            goto Label_018C;
                        }
                        num5 = this.zptr[num4];
                        num6 = num4;
                        while (this.FullGtU(this.zptr[num6 - num3] + d, num5 + d))
                        {
                            this.zptr[num6] = this.zptr[num6 - num3];
                            num6 -= num3;
                            if (num6 <= ((lo + num3) - 1))
                            {
                                break;
                            }
                        }
                        this.zptr[num6] = num5;
                        num4++;
                    }
                    while ((this.workDone <= this.workLimit) || !this.firstAttempt);
                    return;
                Label_018C:
                    index--;
                }
            }
        }

        private void Vswap(int p1, int p2, int n)
        {
            int num = 0;
            while (n > 0)
            {
                num = this.zptr[p1];
                this.zptr[p1] = this.zptr[p2];
                this.zptr[p2] = num;
                p1++;
                p2++;
                n--;
            }
        }

        public override void Write(byte[] buf, int off, int len)
        {
            for (int i = 0; i < len; i++)
            {
                this.WriteByte(buf[off + i]);
            }
        }

        public override void WriteByte(byte bv)
        {
            int num = (0x100 + bv) % 0x100;
            if (this.currentChar != -1)
            {
                if (this.currentChar != num)
                {
                    this.WriteRun();
                    this.runLength = 1;
                    this.currentChar = num;
                }
                else
                {
                    this.runLength++;
                    if (this.runLength > 0xfe)
                    {
                        this.WriteRun();
                        this.currentChar = -1;
                        this.runLength = 0;
                    }
                }
            }
            else
            {
                this.currentChar = num;
                this.runLength++;
            }
        }

        private void WriteRun()
        {
            if (this.last < this.allowableBlockSize)
            {
                this.inUse[this.currentChar] = true;
                for (int i = 0; i < this.runLength; i++)
                {
                    this.mCrc.Update(this.currentChar);
                }
                switch (this.runLength)
                {
                    case 1:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;

                    case 2:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;

                    case 3:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;
                }
                this.inUse[this.runLength - 4] = true;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) (this.runLength - 4);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.WriteRun();
            }
        }

        public override bool CanRead
        {
            get
            {
                return this.baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.baseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseStream.Position;
            }
            set
            {
                this.baseStream.Position = value;
            }
        }

        private class StackElem
        {
            public int dd;
            public int hh;
            public int ll;
        }
    }
}

