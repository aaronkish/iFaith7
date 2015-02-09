namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;

    public class InflaterInputStream : Stream
    {
        protected Stream baseInputStream;
        protected byte[] buf;
        protected byte[] cryptbuffer;
        protected Inflater inf;
        private uint[] keys;
        protected int len;
        private byte[] onebytebuffer;

        public InflaterInputStream(Stream baseInputStream) : this(baseInputStream, new Inflater(), 4096)
        {
        }

        public InflaterInputStream(Stream baseInputStream, Inflater inf) : this(baseInputStream, inf, 4096)
        {
        }

        public InflaterInputStream(Stream baseInputStream, Inflater inf, int size)
        {
            this.onebytebuffer = new byte[1];
            this.cryptbuffer = null;
            this.keys = null;
            this.baseInputStream = baseInputStream;
            this.inf = inf;
            try
            {
                this.len = (int) baseInputStream.Length;
            }
            catch (Exception)
            {
                this.len = 0;
            }
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size <= 0");
            }
            this.buf = new byte[size];
        }

        public override void Close()
        {
            this.baseInputStream.Close();
        }

        private uint ComputeCrc32(uint oldCrc, byte bval)
        {
            return (Crc32.CrcTable[(int)((IntPtr)((oldCrc ^ bval) & 255))] ^ (oldCrc >> 8));
        }

        protected void DecryptBlock(byte[] buf, int off, int len)
        {
            for (int i = off; i < (off + len); i++)
            {
                byte[] buffer;
                IntPtr ptr;
                (buffer = buf)[(int) (ptr = (IntPtr) i)] = (byte) (buffer[(int) ptr] ^ this.DecryptByte());
                this.UpdateKeys(buf[i]);
            }
        }

        protected byte DecryptByte()
        {
            uint num = (this.keys[2] & 65535) | 2;
            return (byte) ((num * (num ^ 1)) >> 8);
        }

        protected void Fill()
        {
            this.len = this.baseInputStream.Read(this.buf, 0, this.buf.Length);
            if (this.cryptbuffer != null)
            {
                this.DecryptBlock(this.buf, 0, this.buf.Length);
            }
            if (this.len <= 0)
            {
                throw new ApplicationException("Deflated stream ends early.");
            }
            this.inf.SetInput(this.buf, 0, this.len);
        }
        

        public override void Flush()
        {
            this.baseInputStream.Flush();
        }

        protected void InitializePassword(string password)
        {
            this.keys = new uint[] { 305419896, 591751049, 878082192 };
            for (int i = 0; i < password.Length; i++)
            {
                this.UpdateKeys((byte) password[i]);
            }
        }

        public override int Read(byte[] b, int off, int len)
        {
            while (true)
            {
                int num;
                try
                {
                    if (b.Length<=len)
                    {
                        b = new byte[2 * len];
                    }
                    //LA MATRIZ DE ORIGEN NO ES LO SUFICIENTEMENTE LARGA.
                    num = this.inf.Inflate(b, off, len);
                }
                catch (Exception exception)
                {
                    throw new ZipException(exception.ToString());
                }
                if (num > 0)
                {
                    return num;
                }
                if (this.inf.IsNeedingDictionary)
                {
                    throw new ZipException("Need a dictionary");
                }
                if (this.inf.IsFinished)
                {
                    return 0;
                }
                if (!this.inf.IsNeedingInput)
                {
                    throw new InvalidOperationException("Don't know what to do");
                }
                this.Fill();
            }
        }

        public override int ReadByte()
        {
            if (this.Read(this.onebytebuffer, 0, 1) > 0)
            {
                return (this.onebytebuffer[0] & 255);
            }
            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseInputStream.Seek(offset, origin);
        }

        public override void SetLength(long val)
        {
            this.baseInputStream.SetLength(val);
        }

        public long Skip(long n)
        {
            if (n < 0L)
            {
                throw new ArgumentOutOfRangeException("n");
            }
            int num = 2048;
            if (n < num)
            {
                num = (int) n;
            }
            byte[] buffer = new byte[num];
            return (long) this.baseInputStream.Read(buffer, 0, buffer.Length);
        }

        protected void UpdateKeys(byte ch)
        {
            this.keys[0] = this.ComputeCrc32(this.keys[0], ch);
            this.keys[1] += (byte) this.keys[0];
            this.keys[1] = (this.keys[1] * 134775813) + 1;
            this.keys[2] = this.ComputeCrc32(this.keys[2], (byte)(this.keys[1] >> 24));
        }

        public override void Write(byte[] array, int offset, int count)
        {
            this.baseInputStream.Write(array, offset, count);
        }

        public override void WriteByte(byte val)
        {
            this.baseInputStream.WriteByte(val);
        }

        public virtual int Available
        {
            get
            {
                if (!this.inf.IsFinished)
                {
                    return 1;
                }
                return 0;
            }
        }

        public override bool CanRead
        {
            get
            {
                return this.baseInputStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.baseInputStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseInputStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return (long) this.len;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseInputStream.Position;
            }
            set
            {
                this.baseInputStream.Position = value;
            }
        }
    }
}

