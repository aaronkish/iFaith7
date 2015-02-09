namespace iFaith.RemoteZip
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.BZip2;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    public class RemoteZipFile : IEnumerable
    {
        private string baseUrl;
        private ZipEntry[] entries;
        private int MaxFileOffset;

        private bool FindCentralDirectory(string url, ref int Offset, ref int Size, ref int Entries)
        {
            HttpWebRequest request;
            int num = 256;
            Entries = 0;
            Size = 0;
            Offset = -1;
        Label_0010:
            request = (HttpWebRequest) WebRequest.Create(url);
            request.AddRange(-(num + 22));
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            byte[] bb = new byte[((int) (response.ContentLength - 1L)) + 1];
            int num2 = ReadAll(bb, 0, (int) response.ContentLength, response.GetResponseStream());
            response.Close();
            int index = num2 - 22;
            while (index >= 0)
            {
                if (bb[index] == 80)
                {
                    if (((bb[index + 1] == 75) & (bb[index + 2] == 5)) & (bb[index + 3] == 6))
                    {
                        break;
                    }
                    index -= 4;
                }
                else
                {
                    index--;
                }
            }
            if (index < 0)
            {
                switch (num)
                {
                    case 0x10000:
                        goto Label_0127;

                    case 0x400:
                        num = 0x10000;
                        goto Label_0010;
                }
                if (num != 0x100)
                {
                    goto Label_0127;
                }
                num = 0x400;
                goto Label_0010;
            }
            Size = BitConverter.ToInt32(bb, index + 12);
            Offset = BitConverter.ToInt32(bb, index + 0x10);
            Entries = BitConverter.ToInt16(bb, index + 10);
            return true;
        Label_0127:
            return false;
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
            if (entry.Size == 0L)
            {
                return null;
            }
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            int zipFileIndex = entry.zipFileIndex;
            if (((zipFileIndex < 0) || (zipFileIndex >= this.entries.Length)) || (this.entries[zipFileIndex].Name != entry.Name))
            {
                throw new IndexOutOfRangeException();
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(this.baseUrl);
            int to = (int) (((entry.offset + entry.CompressedSize) + 0x10L) + 0x20000L);
            if (to >= this.MaxFileOffset)
            {
                to = this.MaxFileOffset - 1;
            }
            request.AddRange(entry.offset, to);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            this.SkipLocalHeader(responseStream, this.entries[zipFileIndex]);
            CompressionMethod compressionMethod = this.entries[zipFileIndex].CompressionMethod;
            Stream baseInputStream = new PartialInputStream(responseStream, response, this.entries[zipFileIndex].CompressedSize);
            switch (compressionMethod)
            {
                case CompressionMethod.Stored:
                    return baseInputStream;

                case CompressionMethod.Deflated:
                    return new InflaterInputStream(baseInputStream, new Inflater(true));

                case ((CompressionMethod) 12):
                    return new BZip2InputStream(baseInputStream);
            }
            throw new ZipException(Conversions.ToString((double) (Conversions.ToDouble("Unknown compression method ") + ((double) compressionMethod))));
        }

        public bool Load(string url)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            if (!this.FindCentralDirectory(url, ref num, ref num2, ref num3))
            {
                return false;
            }
            this.MaxFileOffset = num;
            this.baseUrl = url;
            this.entries = new ZipEntry[(num3 - 1) + 1];
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.AddRange(num, num + num2);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            try
            {
                int num4 = num3 - 1;
                for (int i = 0; i <= num4; i++)
                {
                    Application.DoEvents();
                    if (this.ReadLeInt(responseStream) != 33639248)
                    {
                        throw new ZipException("Wrong Central Directory signature");
                    }
                    this.ReadLeInt(responseStream);
                    this.ReadLeShort(responseStream);
                    int num6 = this.ReadLeShort(responseStream);
                    int num7 = this.ReadLeInt(responseStream);
                    int num8 = this.ReadLeInt(responseStream);
                    int num9 = this.ReadLeInt(responseStream);
                    int num10 = this.ReadLeInt(responseStream);
                    int num11 = this.ReadLeShort(responseStream);
                    int sst = this.ReadLeShort(responseStream);
                    int num13 = this.ReadLeShort(responseStream);
                    this.ReadLeInt(responseStream);
                    this.ReadLeInt(responseStream);
                    int num14 = this.ReadLeInt(responseStream);
                    byte[] bb = new byte[(Math.Max(num11, num13) - 1) + 1];
                    ReadAll(bb, 0, num11, responseStream);
                    ZipEntry entry = new ZipEntry(ZipConstants.ConvertToString(bb));
                    entry.CompressionMethod = (CompressionMethod) num6;
                    entry.Crc = num8 & ((long) 0xffffffffL);
                    entry.Size = num10 & ((long) 0xffffffffL);
                    entry.CompressedSize = num9 & ((long) 0xffffffffL);
                    entry.DosTime = (uint) num7;
                    if (sst > 0)
                    {
                        byte[] buffer2 = new byte[(sst - 1) + 1];
                        ReadAll(buffer2, 0, sst, responseStream);
                        entry.ExtraData = buffer2;
                    }
                    if (num13 > 0)
                    {
                        ReadAll(bb, 0, num13, responseStream);
                        entry.Comment = ZipConstants.ConvertToString(bb);
                    }
                    entry.zipFileIndex = i;
                    entry.offset = num14;
                    this.entries[i] = entry;
                    this.OnProgress((int) Math.Round((double) (((double) (i * 100)) / ((double) num3))));
                }
            }
            finally
            {
                responseStream.Close();
                response.Close();
            }
            this.OnProgress(100);
            return true;
        }

        public static int MakeInt(byte[] bb, int pos)
        {
            return BitConverter.ToInt32(bb, 0);
        }

        public static int MakeShort(byte[] bb, int pos)
        {
            return (bb[pos + 0] | ((byte) (bb[pos + 1] << 0)));
        }

        public virtual void OnProgress(int pct)
        {
        }

        private static int ReadAll(byte[] bb, int p, int sst, Stream s)
        {
            int num = 0;
            while (num < sst)
            {
                int num2 = s.Read(bb, p, sst - num);
                if (num2 <= 0)
                {
                    return num;
                }
                num += num2;
                p += num2;
            }
            return num;
        }

        private int ReadLeInt(Stream s)
        {
            return (this.ReadLeShort(s) | (this.ReadLeShort(s) << 0x10));
        }

        private int ReadLeShort(Stream s)
        {
            return (s.ReadByte() | (s.ReadByte() << 8));
        }

        private static void Skip(Stream s, int n)
        {
            int num = n - 1;
            for (int i = 0; i <= num; i++)
            {
                s.ReadByte();
            }
        }

        private void SkipLocalHeader(Stream baseStream, ZipEntry entry)
        {
            Stream stream = baseStream;
            lock (stream)
            {
                if (this.ReadLeInt(baseStream) != 0x4034b50)
                {
                    throw new ZipException("Wrong Local header signature");
                }
                Skip(baseStream, 0x16);
                int num = this.ReadLeShort(baseStream);
                int num2 = this.ReadLeShort(baseStream);
                Skip(baseStream, num + num2);
            }
        }

        public ZipEntry this[int index]
        {
            get
            {
                return this.entries[index];
            }
        }

        public int Size
        {
            get
            {
                if (this.entries != null)
                {
                    return this.entries.Length;
                }
                return 0;
            }
        }

        private class PartialInputStream : InflaterInputStream
        {
            private Stream baseStream;
            private long end;
            private long filepos;
            private HttpWebResponse request;

            public PartialInputStream(Stream baseStream, HttpWebResponse request, long len) : base(baseStream)
            {
                this.baseStream = baseStream;
                this.filepos = 0L;
                this.end = len;
                this.request = request;
            }

            public override void Close()
            {
                this.request.Close();
                this.baseStream.Close();
            }

            public override int Read(byte[] b, int off, int len)
            {
                if (len > (this.end - this.filepos))
                {
                    len = (int) (this.end - this.filepos);
                    if (len == 0)
                    {
                        return 0;
                    }
                }
                Stream baseStream = this.baseStream;
                lock (baseStream)
                {
                    int num = RemoteZipFile.ReadAll(b, off, len, this.baseStream);
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
                Stream baseStream = this.baseStream;
                lock (baseStream)
                {
                    this.filepos += 1L;
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
                int num = (int) (amount - 1L);
                for (int i = 0; i <= num; i++)
                {
                    this.baseStream.ReadByte();
                }
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
                return (Interlocked.Increment(ref this.ptr) < this.array.Length);
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
            //public object System.Collections.IEnumerator.Current
            //{
            //    get
            //    {
            //        return this.array[this.ptr];
            //    }
            //}
        }
    }
}

