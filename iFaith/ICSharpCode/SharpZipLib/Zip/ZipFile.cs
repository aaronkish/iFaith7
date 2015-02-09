namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.Collections;
    using System.IO;

    public class ZipFile : IEnumerable
    {
        private Stream baseStream;
        private string comment;
        private ZipEntry[] entries;
        private string name;

        public ZipFile(FileStream file)
        {
            this.baseStream = file;
            this.name = file.Name;
            this.ReadEntries();
        }

        public ZipFile(Stream baseStream)
        {
            this.baseStream = baseStream;
            this.name = null;
            this.ReadEntries();
        }

        public ZipFile(string name) : this(File.OpenRead(name))
        {
        }

        private long CheckLocalHeader(ZipEntry entry)
        {
            lock (this.baseStream)
            {
                this.baseStream.Seek((long) entry.offset, SeekOrigin.Begin);
                if (this.ReadLeInt() != 0x4034b50)
                {
                    throw new ZipException("Wrong Local header signature");
                }
                long position = this.baseStream.Position;
                this.baseStream.Position += 4L;
                if ((this.baseStream.Position - position) != 4L)
                {
                    throw new EndOfStreamException();
                }
                if ((int)entry.CompressionMethod != this.ReadLeShort()) //MODIFICADO POR MI< VERIFICAR FUNCIONAMIENTO
                {
                    throw new ZipException("Compression method mismatch");
                }
                position = this.baseStream.Position;
                this.baseStream.Position += 0x10L;
                if ((this.baseStream.Position - position) != 0x10L)
                {
                    throw new EndOfStreamException();
                }
                if (entry.Name.Length != this.ReadLeShort())
                {
                    throw new ZipException("file name length mismatch");
                }
                int num2 = entry.Name.Length + this.ReadLeShort();
                return ((entry.offset + 30) + num2);
            }
        }

        public void Close()
        {
            this.entries = null;
            lock (this.baseStream)
            {
                this.baseStream.Close();
            }
        }

        public ZipEntry GetEntry(string name)
        {
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            int entryIndex = this.GetEntryIndex(name);
            if (entryIndex < 0)
            {
                return null;
            }
            return (ZipEntry) this.entries[entryIndex].Clone();
        }

        private int GetEntryIndex(string name)
        {
            for (int i = 0; i < this.entries.Length; i++)
            {
                if (name.Equals(this.entries[i].Name))
                {
                    return i;
                }
            }
            return -1;
        }

        public IEnumerator GetEnumerator()
        {
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            return new ZipEntryEnumeration(this.entries);
        }

        public Stream GetInputStream(ZipEntry entry)
        {
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            int zipFileIndex = entry.zipFileIndex;
            if (((zipFileIndex < 0) || (zipFileIndex >= this.entries.Length)) || (this.entries[zipFileIndex].Name != entry.Name))
            {
                zipFileIndex = this.GetEntryIndex(entry.Name);
                if (zipFileIndex < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            long start = this.CheckLocalHeader(this.entries[zipFileIndex]);
            CompressionMethod compressionMethod = this.entries[zipFileIndex].CompressionMethod;
            Stream baseInputStream = new PartialInputStream(this.baseStream, start, this.entries[zipFileIndex].CompressedSize);
            CompressionMethod method2 = compressionMethod;
            if (method2 != CompressionMethod.Stored)
            {
                if (method2 != CompressionMethod.Deflated)
                {
                    throw new ZipException("Unknown compression method " + compressionMethod);
                }
                return new InflaterInputStream(baseInputStream, new Inflater(true));
            }
            return baseInputStream;
        }

        private void ReadEntries()
        {
            long offset = this.baseStream.Length - 0x16L;
            do
            {
                if (offset < 0L)
                {
                    throw new ZipException("central directory not found, probably not a zip file");
                }
                offset -= 1L;
                this.baseStream.Seek(offset, SeekOrigin.Begin);
            }
            while (this.ReadLeInt() != 0x6054b50);
            long position = this.baseStream.Position;
            this.baseStream.Position += 6L;
            if ((this.baseStream.Position - position) != 6L)
            {
                throw new EndOfStreamException();
            }
            int num3 = this.ReadLeShort();
            position = this.baseStream.Position;
            this.baseStream.Position += 4L;
            if ((this.baseStream.Position - position) != 4L)
            {
                throw new EndOfStreamException();
            }
            int num4 = this.ReadLeInt();
            byte[] buffer = new byte[this.ReadLeShort()];
            this.baseStream.Read(buffer, 0, buffer.Length);
            this.comment = ZipConstants.ConvertToString(buffer);
            this.entries = new ZipEntry[num3];
            this.baseStream.Seek((long) num4, SeekOrigin.Begin);
            for (int i = 0; i < num3; i++)
            {
                if (this.ReadLeInt() != 0x2014b50)
                {
                    throw new ZipException("Wrong Central Directory signature");
                }
                position = this.baseStream.Position;
                this.baseStream.Position += 6L;
                if ((this.baseStream.Position - position) != 6L)
                {
                    throw new EndOfStreamException();
                }
                int num7 = this.ReadLeShort();
                int num8 = this.ReadLeInt();
                int num9 = this.ReadLeInt();
                int num10 = this.ReadLeInt();
                int num11 = this.ReadLeInt();
                int num12 = this.ReadLeShort();
                int count = this.ReadLeShort();
                int num14 = this.ReadLeShort();
                position = this.baseStream.Position;
                this.baseStream.Position += 8L;
                if ((this.baseStream.Position - position) != 8L)
                {
                    throw new EndOfStreamException();
                }
                int num15 = this.ReadLeInt();
                byte[] buffer2 = new byte[Math.Max(num12, num14)];
                this.baseStream.Read(buffer2, 0, num12);
                ZipEntry entry = new ZipEntry(ZipConstants.ConvertToString(buffer2));
                entry.CompressionMethod = (CompressionMethod) num7;
                entry.Crc = num9 & ((long) 0xffffffffL);
                entry.Size = num11 & ((long) 0xffffffffL);
                entry.CompressedSize = num10 & ((long) 0xffffffffL);
                entry.DosTime = (uint) num8;
                if (count > 0)
                {
                    byte[] buffer3 = new byte[count];
                    this.baseStream.Read(buffer3, 0, count);
                    entry.ExtraData = buffer3;
                }
                if (num14 > 0)
                {
                    this.baseStream.Read(buffer2, 0, num14);
                    entry.Comment = ZipConstants.ConvertToString(buffer2);
                }
                entry.zipFileIndex = i;
                entry.offset = num15;
                this.entries[i] = entry;
            }
        }

        private int ReadLeInt()
        {
            return (this.ReadLeShort() | (this.ReadLeShort() << 0x10));
        }

        private int ReadLeShort()
        {
            return (this.baseStream.ReadByte() | (this.baseStream.ReadByte() << 8));
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Size
        {
            get
            {
                int length;
                try
                {
                    length = this.entries.Length;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("ZipFile has closed");
                }
                return length;
            }
        }

        public string ZipFileComment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
            }
        }

        private class PartialInputStream : InflaterInputStream
        {
            private Stream baseStream;
            private long end;
            private long filepos;

            public PartialInputStream(Stream baseStream, long start, long len) : base(baseStream)
            {
                this.baseStream = baseStream;
                this.filepos = start;
                this.end = start + len;
            }

            public override int Read(byte[] b, int off, int len)
            {
                if (len > (this.end - this.filepos))
                {
                    len = (int) (this.end - this.filepos);
                    if (len == 0)
                    {
                        return -1;
                    }
                }
                lock (this.baseStream)
                {
                    this.baseStream.Seek(this.filepos, SeekOrigin.Begin);
                    int num = this.baseStream.Read(b, off, len);
                    if (num > 0)
                    {
                        this.filepos += len;
                    }
                    return num;
                }
            }

            public override int ReadByte()
            {
                if (this.filepos == this.end)
                {
                    return -1;
                }
                lock (this.baseStream)
                {
                    long num;
                    this.filepos = (num = this.filepos) + 1L;
                    this.baseStream.Seek(num, SeekOrigin.Begin);
                    return this.baseStream.ReadByte();
                }
            }

            public long SkipBytes(long amount)
            {
                if (amount < 0L)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (amount > (this.end - this.filepos))
                {
                    amount = this.end - this.filepos;
                }
                this.filepos += amount;
                return amount;
            }

            public override int Available
            {
                get
                {
                    long num = this.end - this.filepos;
                    if (num > 0x7fffffffL)
                    {
                        return 0x7fffffff;
                    }
                    return (int) num;
                }
            }
        }

        private class ZipEntryEnumeration : IEnumerator
        {
            private ZipEntry[] array;
            private int ptr = -1;

            public ZipEntryEnumeration(ZipEntry[] arr)
            {
                this.array = arr;
            }

            public bool MoveNext()
            {
                return (++this.ptr < this.array.Length);
            }

            public void Reset()
            {
                this.ptr = -1;
            }

            public object Current
            {
                get
                {
                    return this.array[this.ptr];
                }
            }
        }
    }
}

