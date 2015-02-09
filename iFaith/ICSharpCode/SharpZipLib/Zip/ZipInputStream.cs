namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;

    public class ZipInputStream : InflaterInputStream
    {
        private long avail;
        private Crc32 crc;
        private long csize;
        private ZipEntry entry;
        private int flags;
        private int method;
        private string password;
        private long size;

        public ZipInputStream(Stream baseInputStream) : base(baseInputStream, new Inflater(true))
        {
            this.crc = new Crc32();
            this.entry = null;
            this.password = null;
        }

        public override void Close()
        {
            base.Close();
            this.crc = null;
            this.entry = null;
        }

        public void CloseEntry()
        {
            if (this.crc == null)
            {
                throw new InvalidOperationException("Closed.");
            }
            if (this.entry != null)
            {
                if (this.method == 8)
                {
                    if ((this.flags & 8) != 0)
                    {
                        byte[] buffer = new byte[0x800];
                        while (this.Read(buffer, 0, buffer.Length) > 0)
                        {
                        }
                        return;
                    }
                    this.csize -= base.inf.TotalIn;
                    this.avail = base.inf.RemainingInput;
                }
                if ((this.avail > this.csize) && (this.csize >= 0L))
                {
                    this.avail -= this.csize;
                }
                else
                {
                    this.csize -= this.avail;
                    this.avail = 0L;
                    while (this.csize != 0L)
                    {
                        int num = (int) base.Skip(this.csize & ((long) 0xffffffffL));
                        if (num <= 0)
                        {
                            throw new ZipException("zip archive ends early.");
                        }
                        this.csize -= num;
                    }
                }
                this.size = 0L;
                this.crc.Reset();
                if (this.method == 8)
                {
                    base.inf.Reset();
                }
                this.entry = null;
            }
        }

        private void FillBuf()
        {
            this.avail = base.len = base.baseInputStream.Read(base.buf, 0, base.buf.Length);
        }

        public ZipEntry GetNextEntry()
        {
            if (this.crc == null)
            {
                throw new InvalidOperationException("Closed.");
            }
            if (this.entry != null)
            {
                this.CloseEntry();
            }
            int num = this.ReadLeInt();
            if (num == 0x2014b50)
            {
                this.Close();
                return null;
            }
            if (num != 0x4034b50)
            {
                throw new ZipException("Wrong Local header signature: 0x" + string.Format("{0:X}", num));
            }
            short num2 = (short) this.ReadLeShort();
            this.flags = this.ReadLeShort();
            this.method = this.ReadLeShort();
            uint num3 = (uint) this.ReadLeInt();
            int num4 = this.ReadLeInt();
            this.csize = this.ReadLeInt();
            this.size = this.ReadLeInt();
            int num5 = this.ReadLeShort();
            int num6 = this.ReadLeShort();
            bool flag = (this.flags & 1) == 1;
            if ((this.method == 0) && ((!flag && (this.csize != this.size)) || (flag && ((this.csize - 12L) != this.size))))
            {
                throw new ZipException("Stored, but compressed != uncompressed");
            }
            byte[] outBuf = new byte[num5];
            this.ReadFully(outBuf);
            string name = ZipConstants.ConvertToString(outBuf);
            this.entry = new ZipEntry(name);
            this.entry.IsCrypted = flag;
            this.entry.Version = (ushort) num2;
            if ((this.method != 0) && (this.method != 8))
            {
                throw new ZipException("unknown compression method " + this.method);
            }
            this.entry.CompressionMethod = (CompressionMethod) this.method;
            if ((this.flags & 8) == 0)
            {
                this.entry.Crc = num4 & ((long) 0xffffffffL);
                this.entry.Size = this.size & ((long) 0xffffffffL);
                this.entry.CompressedSize = this.csize & ((long) 0xffffffffL);
            }
            this.entry.DosTime = num3;
            if (num6 > 0)
            {
                byte[] buffer2 = new byte[num6];
                this.ReadFully(buffer2);
                this.entry.ExtraData = buffer2;
            }
            if (flag)
            {
                if (this.password == null)
                {
                    throw new ZipException("No password set.");
                }
                base.InitializePassword(this.password);
                base.cryptbuffer = new byte[12];
                this.ReadFully(base.cryptbuffer);
                for (int i = 0; i < 12; i++)
                {
                    byte[] buffer3;
                    IntPtr ptr;
                    (buffer3 = base.cryptbuffer)[(int) (ptr = (IntPtr) i)] = (byte) (buffer3[(int) ptr] ^ base.DecryptByte());
                    base.UpdateKeys(base.cryptbuffer[i]);
                }
                this.csize -= 12L;
            }
            else
            {
                base.cryptbuffer = null;
            }
            if ((this.method == 8) && (this.avail > 0L))
            {
                Array.Copy(base.buf, base.len - ((int) this.avail), base.buf, 0, (int) this.avail);
                base.len = (int) this.avail;
                this.avail = 0L;
                if (flag)
                {
                    base.DecryptBlock(base.buf, 0, base.len);
                }
                base.inf.SetInput(base.buf, 0, base.len);
            }
            return this.entry;
        }

        public override int Read(byte[] b, int off, int len)
        {
            if (this.crc == null)
            {
                throw new InvalidOperationException("Closed.");
            }
            if (this.entry == null)
            {
                return -1;
            }
            bool flag = false;
            switch (this.method)
            {
                case 0:
                    if ((len > this.csize) && (this.csize >= 0L))
                    {
                        len = (int) this.csize;
                    }
                    len = this.ReadBuf(b, off, len);
                    if (len > 0)
                    {
                        this.csize -= len;
                        this.size -= len;
                    }
                    if (this.csize == 0L)
                    {
                        flag = true;
                    }
                    else if (len < 0)
                    {
                        throw new ZipException("EOF in stored block");
                    }
                    if (base.cryptbuffer != null)
                    {
                        base.DecryptBlock(b, off, len);
                    }
                    break;

                case 8:
                    len = base.Read(b, off, len);
                    if (len < 0)
                    {
                        if (!base.inf.IsFinished)
                        {
                            throw new ZipException("Inflater not finished!?");
                        }
                        this.avail = base.inf.RemainingInput;
                        if ((base.inf.TotalIn != this.csize) || (base.inf.TotalOut != this.size))
                        {
                            throw new ZipException(string.Concat(new object[] { "size mismatch: ", this.csize, ";", this.size, " <-> ", base.inf.TotalIn, ";", base.inf.TotalOut }));
                        }
                        base.inf.Reset();
                        flag = true;
                    }
                    break;
            }
            if (len > 0)
            {
                this.crc.Update(b, off, len);
            }
            if (flag)
            {
                if ((this.flags & 8) != 0)
                {
                    this.ReadDataDescr();
                }
                Console.WriteLine("{0:x}", this.crc.Value & ((long) 0xffffffffL));
                Console.WriteLine(this.entry.Crc);
                if (((ulong)(((ulong) this.crc.Value) & 0xffffffffL) != (ulong)this.entry.Crc) && (this.entry.Crc != -1L))
                {
                    throw new ZipException("CRC mismatch");
                }
                this.crc.Reset();
                this.entry = null;
            }
            return len;
        }

        private int ReadBuf(byte[] outBuf, int offset, int length)
        {
            if (this.avail <= 0L)
            {
                this.FillBuf();
                if (this.avail <= 0L)
                {
                    return -1;
                }
            }
            if (length > this.avail)
            {
                length = (int) this.avail;
            }
            Array.Copy(base.buf, base.len - ((int) this.avail), outBuf, offset, length);
            this.avail -= length;
            return length;
        }

        public override int ReadByte()
        {
            byte[] buffer = new byte[1];
            if (this.Read(buffer, 0, 1) <= 0)
            {
                return -1;
            }
            return (buffer[0] & 0xff);
        }

        private void ReadDataDescr()
        {
            if (this.ReadLeInt() != 0x8074b50)
            {
                throw new ZipException("Data descriptor signature not found");
            }
            this.entry.Crc = this.ReadLeInt() & ((long) 0xffffffffL);
            this.csize = this.ReadLeInt();
            this.size = this.ReadLeInt();
            this.entry.Size = this.size & ((long) 0xffffffffL);
            this.entry.CompressedSize = this.csize & ((long) 0xffffffffL);
        }

        private void ReadFully(byte[] outBuf)
        {
            int num3;
            int offset = 0;
            for (int i = outBuf.Length; i > 0; i -= num3)
            {
                num3 = this.ReadBuf(outBuf, offset, i);
                if (num3 == -1)
                {
                    throw new Exception();
                }
                offset += num3;
            }
        }

        private int ReadLeByte()
        {
            long num;
            if (this.avail <= 0L)
            {
                this.FillBuf();
                if (this.avail <= 0L)
                {
                    throw new ZipException("EOF in header");
                }
            }
            this.avail = (num = this.avail) - 1L;
            return (base.buf[(int) ((IntPtr) (base.len - num))] & 0xff);
        }

        private int ReadLeInt()
        {
            return (this.ReadLeShort() | (this.ReadLeShort() << 0x10));
        }

        private long ReadLeLong()
        {
            return (long) (this.ReadLeInt() | this.ReadLeInt());
        }

        private int ReadLeShort()
        {
            return (this.ReadLeByte() | (this.ReadLeByte() << 8));
        }

        public override int Available
        {
            get
            {
                if (this.entry == null)
                {
                    return 0;
                }
                return 1;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }
    }
}

