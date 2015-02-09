namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;
    using System.IO;

    public class BZip2InputStream : Stream
    {
        private int[][] baseArray = new int[BZip2Constants.N_GROUPS][];
        private Stream baseStream;
        private bool blockRandomised;
        private int blockSize100k;
        private int bsBuff;
        private int bsLive;
        private int ch2;
        private int chPrev;
        private int computedBlockCRC;
        private uint computedCombinedCRC;
        private int count;
        private int currentChar = -1;
        private int currentState = 1;
        private int i2;
        private bool[] inUse = new bool[0x100];
        private int j2;
        private int last;
        private int[][] limit = new int[BZip2Constants.N_GROUPS][];
        private byte[] ll8;
        private IChecksum mCrc = new StrangeCRC();
        private int[] minLens = new int[BZip2Constants.N_GROUPS];
        private int nInUse;
        private const int NO_RAND_PART_A_STATE = 5;
        private const int NO_RAND_PART_B_STATE = 6;
        private const int NO_RAND_PART_C_STATE = 7;
        private int origPtr;
        private int[][] perm = new int[BZip2Constants.N_GROUPS][];
        private const int RAND_PART_A_STATE = 2;
        private const int RAND_PART_B_STATE = 3;
        private const int RAND_PART_C_STATE = 4;
        private int rNToGo = 0;
        private int rTPos = 0;
        private byte[] selector = new byte[BZip2Constants.MAX_SELECTORS];
        private byte[] selectorMtf = new byte[BZip2Constants.MAX_SELECTORS];
        private byte[] seqToUnseq = new byte[0x100];
        private const int START_BLOCK_STATE = 1;
        private int storedBlockCRC;
        private int storedCombinedCRC;
        private bool streamEnd = false;
        private int tPos;
        private int[] tt;
        private byte[] unseqToSeq = new byte[0x100];
        private int[] unzftab = new int[0x100];
        private byte z;

        public BZip2InputStream(Stream zStream)
        {
            for (int i = 0; i < BZip2Constants.N_GROUPS; i++)
            {
                this.limit[i] = new int[BZip2Constants.MAX_ALPHA_SIZE];
                this.baseArray[i] = new int[BZip2Constants.MAX_ALPHA_SIZE];
                this.perm[i] = new int[BZip2Constants.MAX_ALPHA_SIZE];
            }
            this.ll8 = null;
            this.tt = null;
            this.BsSetStream(zStream);
            this.Initialize();
            this.InitBlock();
            this.SetupBlock();
        }

        private static void BadBGLengths()
        {
        }

        private static void BadBlockHeader()
        {
        }

        private static void bitStreamEOF()
        {
        }

        private static void BlockOverrun()
        {
        }

        private int BsGetint()
        {
            int num = 0;
            num = (num << 8) | this.BsR(8);
            num = (num << 8) | this.BsR(8);
            num = (num << 8) | this.BsR(8);
            return ((num << 8) | this.BsR(8));
        }

        private int BsGetInt32()
        {
            return this.BsGetint();
        }

        private int BsGetIntVS(int numBits)
        {
            return this.BsR(numBits);
        }

        private char BsGetUChar()
        {
            return (char) this.BsR(8);
        }

        private int BsR(int n)
        {
            while (this.bsLive < n)
            {
                this.FillBuffer();
            }
            int num = (this.bsBuff >> (this.bsLive - n)) & ((((int) 1) << n) - 1);
            this.bsLive -= n;
            return num;
        }

        private void BsSetStream(Stream f)
        {
            this.baseStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
        }

        private static void Cadvise()
        {
        }

        public override void Close()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Close();
            }
        }

        private void Complete()
        {
            this.storedCombinedCRC = this.BsGetInt32();
            if (this.storedCombinedCRC != this.computedCombinedCRC)
            {
                CrcError();
            }
            this.streamEnd = true;
        }

        private static void CompressedStreamEOF()
        {
        }

        private static void CrcError()
        {
        }

        private void EndBlock()
        {
            this.computedBlockCRC = (int) this.mCrc.Value;
            if (this.storedBlockCRC != this.computedBlockCRC)
            {
                CrcError();
            }
            this.computedCombinedCRC = ((this.computedCombinedCRC << 1) & uint.MaxValue) | (this.computedCombinedCRC >> 0x1f);
            this.computedCombinedCRC ^= (uint) this.computedBlockCRC;
        }

        private void FillBuffer()
        {
            int num = 0;
            try
            {
                num = this.baseStream.ReadByte();
            }
            catch (Exception)
            {
                CompressedStreamEOF();
            }
            if (num == -1)
            {
                CompressedStreamEOF();
            }
            this.bsBuff = (this.bsBuff << 8) | (num & 0xff);
            this.bsLive += 8;
        }

        public override void Flush()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Flush();
            }
        }

        private void GetAndMoveToFrontDecode()
        {
            int num10;
            byte[] buffer = new byte[0x100];
            int num = BZip2Constants.baseBlockSize * this.blockSize100k;
            this.origPtr = this.BsGetIntVS(0x18);
            this.RecvDecodingTables();
            int num2 = this.nInUse + 1;
            int index = -1;
            int num4 = 0;
            for (int i = 0; i <= 0xff; i++)
            {
                this.unzftab[i] = 0;
            }
            for (int j = 0; j <= 0xff; j++)
            {
                buffer[j] = (byte) j;
            }
            this.last = -1;
            if (num4 == 0)
            {
                index++;
                num4 = BZip2Constants.G_SIZE;
            }
            num4--;
            int num7 = this.selector[index];
            int n = this.minLens[num7];
            int num9 = this.BsR(n);
            while (num9 > this.limit[num7][n])
            {
                n++;
                while (this.bsLive < 1)
                {
                    this.FillBuffer();
                }
                num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                this.bsLive--;
                num9 = (num9 << 1) | num10;
            }
            int num11 = this.perm[num7][num9 - this.baseArray[num7][n]];
            while (num11 != num2)
            {
                IntPtr ptr;
                if ((num11 == BZip2Constants.RUNA) || (num11 == BZip2Constants.RUNB))
                {
                    int num12 = -1;
                    int num13 = 1;
                    do
                    {
                        if (num11 == BZip2Constants.RUNA)
                        {
                            num12 += 1 * num13;
                        }
                        else if (num11 == BZip2Constants.RUNB)
                        {
                            num12 += 2 * num13;
                        }
                        num13 = num13 << 1;
                        if (num4 == 0)
                        {
                            index++;
                            num4 = BZip2Constants.G_SIZE;
                        }
                        num4--;
                        num7 = this.selector[index];
                        n = this.minLens[num7];
                        num9 = this.BsR(n);
                        while (num9 > this.limit[num7][n])
                        {
                            n++;
                            while (this.bsLive < 1)
                            {
                                this.FillBuffer();
                            }
                            num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                            this.bsLive--;
                            num9 = (num9 << 1) | num10;
                        }
                        num11 = this.perm[num7][num9 - this.baseArray[num7][n]];
                    }
                    while ((num11 == BZip2Constants.RUNA) || (num11 == BZip2Constants.RUNB));
                    num12++;
                    byte num14 = this.seqToUnseq[buffer[0]];
                    this.unzftab[(int) (ptr = (IntPtr) num14)] = this.unzftab[(int) ptr] + num12;
                    while (num12 > 0)
                    {
                        this.last++;
                        this.ll8[this.last] = num14;
                        num12--;
                    }
                    if (this.last >= num)
                    {
                        BlockOverrun();
                    }
                }
                else
                {
                    this.last++;
                    if (this.last >= num)
                    {
                        BlockOverrun();
                    }
                    byte num15 = buffer[num11 - 1];
                    this.unzftab[(int) (ptr = (IntPtr) this.seqToUnseq[num15])] = this.unzftab[(int) ptr] + 1;
                    this.ll8[this.last] = this.seqToUnseq[num15];
                    for (int k = num11 - 1; k > 0; k--)
                    {
                        buffer[k] = buffer[k - 1];
                    }
                    buffer[0] = num15;
                    if (num4 == 0)
                    {
                        index++;
                        num4 = BZip2Constants.G_SIZE;
                    }
                    num4--;
                    num7 = this.selector[index];
                    n = this.minLens[num7];
                    num9 = this.BsR(n);
                    while (num9 > this.limit[num7][n])
                    {
                        n++;
                        while (this.bsLive < 1)
                        {
                            this.FillBuffer();
                        }
                        num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                        this.bsLive--;
                        num9 = (num9 << 1) | num10;
                    }
                    num11 = this.perm[num7][num9 - this.baseArray[num7][n]];
                }
            }
        }

        private void HbCreateDecodeTables(int[] limit, int[] baseArray, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int index = 0;
            for (int i = minLen; i <= maxLen; i++)
            {
                for (int num3 = 0; num3 < alphaSize; num3++)
                {
                    if (length[num3] == i)
                    {
                        perm[index] = num3;
                        index++;
                    }
                }
            }
            for (int j = 0; j < BZip2Constants.MAX_CODE_LEN; j++)
            {
                baseArray[j] = 0;
            }
            for (int k = 0; k < alphaSize; k++)
            {
                IntPtr ptr;
                baseArray[(int) (ptr = (IntPtr) (length[k] + '\x0001'))] = baseArray[(int) ptr] + 1;
            }
            for (int m = 1; m < BZip2Constants.MAX_CODE_LEN; m++)
            {
                baseArray[m] += baseArray[m - 1];
            }
            for (int n = 0; n < BZip2Constants.MAX_CODE_LEN; n++)
            {
                limit[n] = 0;
            }
            int num8 = 0;
            for (int num9 = minLen; num9 <= maxLen; num9++)
            {
                num8 += baseArray[num9 + 1] - baseArray[num9];
                limit[num9] = num8 - 1;
                num8 = num8 << 1;
            }
            for (int num10 = minLen + 1; num10 <= maxLen; num10++)
            {
                baseArray[num10] = ((limit[num10 - 1] + 1) << 1) - baseArray[num10];
            }
        }

        private void InitBlock()
        {
            char ch = this.BsGetUChar();
            char ch2 = this.BsGetUChar();
            char ch3 = this.BsGetUChar();
            char ch4 = this.BsGetUChar();
            char ch5 = this.BsGetUChar();
            char ch6 = this.BsGetUChar();
            if ((((ch == '\x0017') && (ch2 == 'r')) && ((ch3 == 'E') && (ch4 == '8'))) && ((ch5 == 'P') && (ch6 == '\x0090')))
            {
                this.Complete();
            }
            else if ((((ch != '1') || (ch2 != 'A')) || ((ch3 != 'Y') || (ch4 != '&'))) || ((ch5 != 'S') || (ch6 != 'Y')))
            {
                BadBlockHeader();
                this.streamEnd = true;
            }
            else
            {
                this.storedBlockCRC = this.BsGetInt32();
                this.blockRandomised = this.BsR(1) == 1;
                this.GetAndMoveToFrontDecode();
                this.mCrc.Reset();
                this.currentState = 1;
            }
        }

        private void Initialize()
        {
            char ch = this.BsGetUChar();
            char ch2 = this.BsGetUChar();
            if (((ch != 'h') || (ch2 < '1')) || (ch2 > '9'))
            {
                this.streamEnd = true;
            }
            else
            {
                this.SetDecompressStructureSizes(ch2 - '0');
                this.computedCombinedCRC = 0;
            }
        }

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int i = 0; i < 0x100; i++)
            {
                if (this.inUse[i])
                {
                    this.seqToUnseq[this.nInUse] = (byte) i;
                    this.unseqToSeq[i] = (byte) this.nInUse;
                    this.nInUse++;
                }
            }
        }

        public override int Read(byte[] b, int off, int len)
        {
            for (int i = 0; i < len; i++)
            {
                int num2 = this.ReadByte();
                if (num2 == -1)
                {
                    return i;
                }
                b[off + i] = (byte) num2;
            }
            return len;
        }

        public override int ReadByte()
        {
            if (this.streamEnd)
            {
                return -1;
            }
            int currentChar = this.currentChar;
            switch (this.currentState)
            {
                case 1:
                case 2:
                case 5:
                    return currentChar;

                case 3:
                    this.SetupRandPartB();
                    return currentChar;

                case 4:
                    this.SetupRandPartC();
                    return currentChar;

                case 6:
                    this.SetupNoRandPartB();
                    return currentChar;

                case 7:
                    this.SetupNoRandPartC();
                    return currentChar;
            }
            return currentChar;
        }

        private void RecvDecodingTables()
        {
            char[][] chArray = new char[BZip2Constants.N_GROUPS][];
            for (int i = 0; i < BZip2Constants.N_GROUPS; i++)
            {
                chArray[i] = new char[BZip2Constants.MAX_ALPHA_SIZE];
            }
            bool[] flagArray = new bool[0x10];
            for (int j = 0; j < 0x10; j++)
            {
                flagArray[j] = this.BsR(1) == 1;
            }
            for (int k = 0; k < 0x10; k++)
            {
                if (flagArray[k])
                {
                    for (int num4 = 0; num4 < 0x10; num4++)
                    {
                        this.inUse[(k * 0x10) + num4] = this.BsR(1) == 1;
                    }
                }
            }
            this.MakeMaps();
            int alphaSize = this.nInUse + 2;
            int num6 = this.BsR(3);
            int num7 = this.BsR(15);
            for (int m = 0; m < num7; m++)
            {
                int num9 = 0;
                while (this.BsR(1) == 1)
                {
                    num9++;
                }
                this.selectorMtf[m] = (byte) num9;
            }
            byte[] buffer = new byte[BZip2Constants.N_GROUPS];
            for (int n = 0; n < num6; n++)
            {
                buffer[n] = (byte) n;
            }
            for (int num11 = 0; num11 < num7; num11++)
            {
                int index = this.selectorMtf[num11];
                byte num13 = buffer[index];
                while (index > 0)
                {
                    buffer[index] = buffer[index - 1];
                    index--;
                }
                buffer[0] = num13;
                this.selector[num11] = num13;
            }
            for (int num14 = 0; num14 < num6; num14++)
            {
                int num15 = this.BsR(5);
                int num16 = 0;
                goto Label_01CB;
            Label_0190:
                if (this.BsR(1) == 0)
                {
                    num15++;
                }
                else
                {
                    num15--;
                }
            Label_01AD:
                if (this.BsR(1) == 1)
                {
                    goto Label_0190;
                }
                chArray[num14][num16] = (char) num15;
                num16++;
            Label_01CB:
                if (num16 < alphaSize)
                {
                    goto Label_01AD;
                }
            }
            for (int num17 = 0; num17 < num6; num17++)
            {
                int num18 = 0x20;
                int num19 = 0;
                for (int num20 = 0; num20 < alphaSize; num20++)
                {
                    num19 = Math.Max(num19, chArray[num17][num20]);
                    num18 = Math.Min(num18, chArray[num17][num20]);
                }
                this.HbCreateDecodeTables(this.limit[num17], this.baseArray[num17], this.perm[num17], chArray[num17], num18, num19, alphaSize);
                this.minLens[num17] = num18;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        private void SetDecompressStructureSizes(int newSize100k)
        {
            if (((0 > newSize100k) || (newSize100k > 9)) || ((0 > this.blockSize100k) || (this.blockSize100k > 9)))
            {
                throw new ApplicationException("Invalid block size");
            }
            this.blockSize100k = newSize100k;
            if (newSize100k != 0)
            {
                int num = BZip2Constants.baseBlockSize * newSize100k;
                this.ll8 = new byte[num];
                this.tt = new int[num];
            }
        }

        public override void SetLength(long val)
        {
            this.baseStream.SetLength(val);
        }

        private void SetupBlock()
        {
            IntPtr ptr;
            int[] destinationArray = new int[0x101];
            destinationArray[0] = 0;
            Array.Copy(this.unzftab, 0, destinationArray, 1, 0x100);
            for (int i = 1; i <= 0x100; i++)
            {
                destinationArray[(int) (ptr = (IntPtr) i)] = destinationArray[(int) ptr] + destinationArray[i - 1];
            }
            for (int j = 0; j <= this.last; j++)
            {
                byte index = this.ll8[j];
                this.tt[destinationArray[index]] = j;
                destinationArray[(int) (ptr = (IntPtr) index)] = destinationArray[(int) ptr] + 1;
            }
            destinationArray = null;
            this.tPos = this.tt[this.origPtr];
            this.count = 0;
            this.i2 = 0;
            this.ch2 = 0x100;
            if (this.blockRandomised)
            {
                this.rNToGo = 0;
                this.rTPos = 0;
                this.SetupRandPartA();
            }
            else
            {
                this.SetupNoRandPartA();
            }
        }

        private void SetupNoRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 6;
                this.mCrc.Update(this.ch2);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupNoRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 5;
                this.count = 1;
                this.SetupNoRandPartA();
            }
            else
            {
                this.count++;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    this.currentState = 7;
                    this.j2 = 0;
                    this.SetupNoRandPartC();
                }
                else
                {
                    this.currentState = 5;
                    this.SetupNoRandPartA();
                }
            }
        }

        private void SetupNoRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.Update(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 5;
                this.i2++;
                this.count = 0;
                this.SetupNoRandPartA();
            }
        }

        private void SetupRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                if (this.rNToGo == 0)
                {
                    this.rNToGo = BZip2Constants.rNums[this.rTPos];
                    this.rTPos++;
                    if (this.rTPos == 0x200)
                    {
                        this.rTPos = 0;
                    }
                }
                this.rNToGo--;
                this.ch2 ^= (this.rNToGo == 1) ? 1 : 0;
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 3;
                this.mCrc.Update(this.ch2);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 2;
                this.count = 1;
                this.SetupRandPartA();
            }
            else
            {
                this.count++;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    if (this.rNToGo == 0)
                    {
                        this.rNToGo = BZip2Constants.rNums[this.rTPos];
                        this.rTPos++;
                        if (this.rTPos == 0x200)
                        {
                            this.rTPos = 0;
                        }
                    }
                    this.rNToGo--;
                    this.z = (byte) (this.z ^ ((this.rNToGo == 1) ? ((byte) 1) : ((byte) 0)));
                    this.j2 = 0;
                    this.currentState = 4;
                    this.SetupRandPartC();
                }
                else
                {
                    this.currentState = 2;
                    this.SetupRandPartA();
                }
            }
        }

        private void SetupRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.Update(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 2;
                this.i2++;
                this.count = 0;
                this.SetupRandPartA();
            }
        }

        public override void Write(byte[] array, int offset, int count)
        {
            this.baseStream.Write(array, offset, count);
        }

        public override void WriteByte(byte val)
        {
            this.baseStream.WriteByte(val);
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
    }
}

