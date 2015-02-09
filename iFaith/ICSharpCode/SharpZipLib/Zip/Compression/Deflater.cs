namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using System;

    public class Deflater
    {
        public static int BEST_COMPRESSION = 9;
        public static int BEST_SPEED = 1;
        private static int BUSY_STATE = 16;
        private static int CLOSED_STATE = 127;
        public static int DEFAULT_COMPRESSION = -1;
        public static int DEFLATED = 8;
        private DeflaterEngine engine;
        private static int FINISHED_STATE = 30;
        private static int FINISHING_STATE = 28;
        private static int FLUSHING_STATE = 20;
        private static int INIT_STATE = 0;
        private static int IS_FINISHING = 8;
        private static int IS_FLUSHING = 4;
        private static int IS_SETDICT = 1;
        private int level;
        public static int NO_COMPRESSION = 0;
        private bool noHeader;
        private DeflaterPending pending;
        private static int SETDICT_STATE = 1;
        private int state;
        private int totalOut;

        public Deflater() : this(DEFAULT_COMPRESSION, false)
        {
        }

        public Deflater(int lvl) : this(lvl, false)
        {
        }

        public Deflater(int lvl, bool nowrap)
        {
            if (lvl == DEFAULT_COMPRESSION)
            {
                lvl = 6;
            }
            else if ((lvl < NO_COMPRESSION) || (lvl > BEST_COMPRESSION))
            {
                throw new ArgumentOutOfRangeException("lvl");
            }
            this.pending = new DeflaterPending();
            this.engine = new DeflaterEngine(this.pending);
            this.noHeader = nowrap;
            this.SetStrategy(DeflateStrategy.Default);
            this.SetLevel(lvl);
            this.Reset();
        }

        public int Deflate(byte[] output)
        {
            return this.Deflate(output, 0, output.Length);
        }

        public int Deflate(byte[] output, int offset, int length)
        {
            int num = length;
            if (this.state == CLOSED_STATE)
            {
                throw new InvalidOperationException("Deflater closed");
            }
            if (this.state < BUSY_STATE)
            {
                int s = (DEFLATED + 112) << 8;
                int num3 = (this.level - 1) >> 1;
                if ((num3 < 0) || (num3 > 3))
                {
                    num3 = 3;
                }
                s |= num3 << 6;
                if ((this.state & IS_SETDICT) != 0)
                {
                    s |= 32;
                }
                s += 31 - (s % 31);
                this.pending.WriteShortMSB(s);
                if ((this.state & IS_SETDICT) != 0)
                {
                    int adler = this.engine.Adler;
                    this.engine.ResetAdler();
                    this.pending.WriteShortMSB(adler >> 16);
                    this.pending.WriteShortMSB(adler & 65535);
                }
                this.state = BUSY_STATE | (this.state & (IS_FLUSHING | IS_FINISHING));
            }
            while (true)
            {
                int num5 = this.pending.Flush(output, offset, length);
                offset += num5;
                this.totalOut += num5;
                length -= num5;
                if ((length == 0) || (this.state == FINISHED_STATE))
                {
                    return (num - length);
                }
                if (!this.engine.Deflate((this.state & IS_FLUSHING) != 0, (this.state & IS_FINISHING) != 0))
                {
                    if (this.state == BUSY_STATE)
                    {
                        return (num - length);
                    }
                    if (this.state == FLUSHING_STATE)
                    {
                        if (this.level != NO_COMPRESSION)
                        {
                            for (int i = 8 + (-this.pending.BitCount & 7); i > 0; i -= 10)
                            {
                                this.pending.WriteBits(2, 10);
                            }
                        }
                        this.state = BUSY_STATE;
                    }
                    else if (this.state == FINISHING_STATE)
                    {
                        this.pending.AlignToByte();
                        if (!this.noHeader)
                        {
                            int num7 = this.engine.Adler;
                            this.pending.WriteShortMSB(num7 >> 16);
                            this.pending.WriteShortMSB(num7 & 65535);
                        }
                        this.state = FINISHED_STATE;
                    }
                }
            }
        }

        public void Finish()
        {
            this.state |= IS_FLUSHING | IS_FINISHING;
        }

        public void Flush()
        {
            this.state |= IS_FLUSHING;
        }

        public void Reset()
        {
            this.state = this.noHeader ? BUSY_STATE : INIT_STATE;
            this.totalOut = 0;
            this.pending.Reset();
            this.engine.Reset();
        }

        public void SetDictionary(byte[] dict)
        {
            this.SetDictionary(dict, 0, dict.Length);
        }

        public void SetDictionary(byte[] dict, int offset, int length)
        {
            if (this.state != INIT_STATE)
            {
                throw new InvalidOperationException();
            }
            this.state = SETDICT_STATE;
            this.engine.SetDictionary(dict, offset, length);
        }

        public void SetInput(byte[] input)
        {
            this.SetInput(input, 0, input.Length);
        }

        public void SetInput(byte[] input, int off, int len)
        {
            if ((this.state & IS_FINISHING) != 0)
            {
                throw new InvalidOperationException("finish()/end() already called");
            }
            this.engine.SetInput(input, off, len);
        }

        public void SetLevel(int lvl)
        {
            if (lvl == DEFAULT_COMPRESSION)
            {
                lvl = 6;
            }
            else if ((lvl < NO_COMPRESSION) || (lvl > BEST_COMPRESSION))
            {
                throw new ArgumentOutOfRangeException("lvl");
            }
            if (this.level != lvl)
            {
                this.level = lvl;
                this.engine.SetLevel(lvl);
            }
        }

        public void SetStrategy(DeflateStrategy stgy)
        {
            this.engine.Strategy = stgy;
        }

        public int Adler
        {
            get
            {
                return this.engine.Adler;
            }
        }

        public bool IsFinished
        {
            get
            {
                return ((this.state == FINISHED_STATE) && this.pending.IsFlushed);
            }
        }

        public bool IsNeedingInput
        {
            get
            {
                return this.engine.NeedsInput();
            }
        }

        public int TotalIn
        {
            get
            {
                return this.engine.TotalIn;
            }
        }

        public int TotalOut
        {
            get
            {
                return this.totalOut;
            }
        }
    }
}

