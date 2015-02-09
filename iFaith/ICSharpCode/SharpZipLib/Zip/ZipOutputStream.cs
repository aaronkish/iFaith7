namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.Collections;
    using System.IO;

    public class ZipOutputStream : DeflaterOutputStream
    {
        private Crc32 crc;
        private ZipEntry curEntry;
        private CompressionMethod curMethod;
        private int defaultMethod;
        public const int DEFLATED = 8;
        private ArrayList entries;
        private int offset;
        private long seekPos;
        private bool shouldWriteBack;
        private int size;
        public const int STORED = 0;
        private const int ZIP_DEFLATED_VERSION = 20;
        private const int ZIP_STORED_VERSION = 10;
        private byte[] zipComment;

        public ZipOutputStream(Stream baseOutputStream) : base(baseOutputStream, new Deflater(Deflater.DEFAULT_COMPRESSION, true))
        {
            this.entries = new ArrayList();
            this.crc = new Crc32();
            this.curEntry = null;
            this.offset = 0;
            this.zipComment = new byte[0];
            this.defaultMethod = 8;
            this.shouldWriteBack = false;
            this.seekPos = -1L;
        }

        public void CloseEntry()
        {
            if (this.curEntry == null)
            {
                throw new InvalidOperationException("No open entry");
            }
            if (this.curMethod == CompressionMethod.Deflated)
            {
                base.Finish();
            }
            int num = (this.curMethod == CompressionMethod.Deflated) ? base.def.TotalOut : this.size;
            if (this.curEntry.Size < 0L)
            {
                this.curEntry.Size = this.size;
            }
            else if (this.curEntry.Size != this.size)
            {
                throw new ZipException(string.Concat(new object[] { "size was ", this.size, ", but I expected ", this.curEntry.Size }));
            }
            if (this.curEntry.CompressedSize < 0L)
            {
                this.curEntry.CompressedSize = num;
            }
            else if (this.curEntry.CompressedSize != num)
            {
                throw new ZipException(string.Concat(new object[] { "compressed size was ", num, ", but I expected ", this.curEntry.CompressedSize }));
            }
            if (this.curEntry.Crc < 0L)
            {
                this.curEntry.Crc = this.crc.Value;
            }
            else if (this.curEntry.Crc != this.crc.Value)
            {
                throw new ZipException(string.Concat(new object[] { "crc was ", this.crc.Value, ", but I expected ", this.curEntry.Crc }));
            }
            this.offset += num;
            if ((this.curMethod == CompressionMethod.Deflated) && ((this.curEntry.flags & 8) != 0))
            {
                if (this.shouldWriteBack)
                {
                    this.curEntry.flags &= -9;
                    long position = base.baseOutputStream.Position;
                    base.baseOutputStream.Seek(this.seekPos, SeekOrigin.Begin);
                    this.WriteLeInt((int) this.curEntry.Crc);
                    this.WriteLeInt((int) this.curEntry.CompressedSize);
                    this.WriteLeInt((int) this.curEntry.Size);
                    base.baseOutputStream.Seek(position, SeekOrigin.Begin);
                    this.shouldWriteBack = false;
                }
                else
                {
                    this.WriteLeInt(0x8074b50);
                    this.WriteLeInt((int) this.curEntry.Crc);
                    this.WriteLeInt((int) this.curEntry.CompressedSize);
                    this.WriteLeInt((int) this.curEntry.Size);
                    this.offset += 0x10;
                }
            }
            this.entries.Add(this.curEntry);
            this.curEntry = null;
        }

        public override void Finish()
        {
            if (this.entries != null)
            {
                if (this.curEntry != null)
                {
                    this.CloseEntry();
                }
                int num = 0;
                int num2 = 0;
                foreach (ZipEntry entry in this.entries)
                {
                    CompressionMethod compressionMethod = entry.CompressionMethod;
                    this.WriteLeInt(0x2014b50);
                    this.WriteLeShort((compressionMethod == CompressionMethod.Stored) ? 10 : 20);
                    this.WriteLeShort((compressionMethod == CompressionMethod.Stored) ? 10 : 20);
                    if (entry.IsCrypted)
                    {
                        entry.flags |= 1;
                    }
                    this.WriteLeShort(entry.flags);
                    this.WriteLeShort((short) compressionMethod);
                    this.WriteLeInt((int) entry.DosTime);
                    this.WriteLeInt((int) entry.Crc);
                    this.WriteLeInt((int) entry.CompressedSize);
                    this.WriteLeInt((int) entry.Size);
                    byte[] buffer = ZipConstants.ConvertToArray(entry.Name);
                    if (buffer.Length > 0xffff)
                    {
                        throw new ZipException("Name too long.");
                    }
                    byte[] extraData = entry.ExtraData;
                    if (extraData == null)
                    {
                        extraData = new byte[0];
                    }
                    string comment = entry.Comment;
                    byte[] buffer3 = (comment != null) ? ZipConstants.ConvertToArray(comment) : new byte[0];
                    if (buffer3.Length > 0xffff)
                    {
                        throw new ZipException("Comment too long.");
                    }
                    this.WriteLeShort(buffer.Length);
                    this.WriteLeShort(extraData.Length);
                    this.WriteLeShort(buffer3.Length);
                    this.WriteLeShort(0);
                    this.WriteLeShort(0);
                    this.WriteLeInt(0);
                    this.WriteLeInt(entry.offset);
                    base.baseOutputStream.Write(buffer, 0, buffer.Length);
                    base.baseOutputStream.Write(extraData, 0, extraData.Length);
                    base.baseOutputStream.Write(buffer3, 0, buffer3.Length);
                    num++;
                    num2 += ((0x2e + buffer.Length) + extraData.Length) + buffer3.Length;
                }
                this.WriteLeInt(0x6054b50);
                this.WriteLeShort(0);
                this.WriteLeShort(0);
                this.WriteLeShort(num);
                this.WriteLeShort(num);
                this.WriteLeInt(num2);
                this.WriteLeInt(this.offset);
                this.WriteLeShort(this.zipComment.Length);
                base.baseOutputStream.Write(this.zipComment, 0, this.zipComment.Length);
                base.baseOutputStream.Flush();
                this.entries = null;
            }
        }

        public void PutNextEntry(ZipEntry entry)
        {
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipOutputStream was finished");
            }
            CompressionMethod compressionMethod = entry.CompressionMethod;
            int num = 0;
            CompressionMethod method2 = compressionMethod;
            if (method2 != CompressionMethod.Stored)
            {
                if ((method2 == CompressionMethod.Deflated) && (((entry.CompressedSize < 0L) || (entry.Size < 0L)) || (entry.Crc < 0L)))
                {
                    num |= 8;
                }
            }
            else
            {
                if (entry.CompressedSize >= 0L)
                {
                    if (entry.Size >= 0L)
                    {
                        if (entry.Size != entry.CompressedSize)
                        {
                            throw new ZipException("Method STORED, but compressed size != size");
                        }
                    }
                    else
                    {
                        entry.Size = entry.CompressedSize;
                    }
                }
                else
                {
                    entry.CompressedSize = entry.Size;
                }
                if (entry.Size < 0L)
                {
                    throw new ZipException("Method STORED, but size not set");
                }
                if (entry.Crc < 0L)
                {
                    throw new ZipException("Method STORED, but crc not set");
                }
            }
            if (this.curEntry != null)
            {
                this.CloseEntry();
            }
            entry.flags = num;
            entry.offset = this.offset;
            entry.CompressionMethod = compressionMethod;
            this.curMethod = compressionMethod;
            this.WriteLeInt(0x4034b50);
            this.WriteLeShort((compressionMethod == CompressionMethod.Stored) ? 10 : 20);
            if ((num & 8) == 0)
            {
                this.WriteLeShort(num);
                this.WriteLeShort((byte) compressionMethod);
                this.WriteLeInt((int) entry.DosTime);
                this.WriteLeInt((int) entry.Crc);
                this.WriteLeInt((int) entry.CompressedSize);
                this.WriteLeInt((int) entry.Size);
            }
            else
            {
                if (base.baseOutputStream.CanSeek)
                {
                    this.shouldWriteBack = true;
                    this.WriteLeShort((short) (num & -9));
                }
                else
                {
                    this.shouldWriteBack = false;
                    this.WriteLeShort(num);
                }
                this.WriteLeShort((byte) compressionMethod);
                this.WriteLeInt((int) entry.DosTime);
                this.seekPos = base.baseOutputStream.Position;
                this.WriteLeInt(0);
                this.WriteLeInt(0);
                this.WriteLeInt(0);
            }
            byte[] buffer = ZipConstants.ConvertToArray(entry.Name);
            if (buffer.Length > 0xffff)
            {
                throw new ZipException("Name too long.");
            }
            byte[] extraData = entry.ExtraData;
            if (extraData == null)
            {
                extraData = new byte[0];
            }
            if (extraData.Length > 0xffff)
            {
                throw new ZipException("Extra data too long.");
            }
            this.WriteLeShort(buffer.Length);
            this.WriteLeShort(extraData.Length);
            base.baseOutputStream.Write(buffer, 0, buffer.Length);
            base.baseOutputStream.Write(extraData, 0, extraData.Length);
            this.offset += (30 + buffer.Length) + extraData.Length;
            this.curEntry = entry;
            this.crc.Reset();
            if (compressionMethod == CompressionMethod.Deflated)
            {
                base.def.Reset();
            }
            this.size = 0;
        }

        public void SetComment(string comment)
        {
            byte[] buffer = ZipConstants.ConvertToArray(comment);
            if (buffer.Length > 0xffff)
            {
                throw new ArgumentException("Comment too long.");
            }
            this.zipComment = buffer;
        }

        public void SetLevel(int level)
        {
            base.def.SetLevel(level);
        }

        public void SetMethod(int method)
        {
            if ((method != 0) && (method != 8))
            {
                throw new ArgumentException("Method not supported.");
            }
            this.defaultMethod = method;
        }

        public override void Write(byte[] b, int off, int len)
        {
            if (this.curEntry == null)
            {
                throw new InvalidOperationException("No open entry.");
            }
            switch (this.curMethod)
            {
                case CompressionMethod.Stored:
                    base.baseOutputStream.Write(b, off, len);
                    break;

                case CompressionMethod.Deflated:
                    base.Write(b, off, len);
                    break;
            }
            this.crc.Update(b, off, len);
            this.size += len;
        }

        private void WriteLeInt(int value)
        {
            this.WriteLeShort(value);
            this.WriteLeShort(value >> 0x10);
        }

        private void WriteLeLong(long value)
        {
            this.WriteLeInt((int) value);
            this.WriteLeInt((int) (value >> 0x20));
        }

        private void WriteLeShort(int value)
        {
            base.baseOutputStream.WriteByte((byte) value);
            base.baseOutputStream.WriteByte((byte) (value >> 8));
        }
    }
}

