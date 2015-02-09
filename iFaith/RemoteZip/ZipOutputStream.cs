namespace iFaith.RemoteZip
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.BZip2;
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.IO;

    public class ZipOutputStream : DeflaterOutputStream
    {
        private Stream additionalStream;
        public const int BZIP2 = 12;
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
        private long startPosition;
        public const int STORED = 0;
        private const int ZIP_DEFLATED_VERSION = 20;
        private const int ZIP_STORED_VERSION = 10;
        private byte[] zipComment;

        public ZipOutputStream(Stream baseOutputStream) : base(baseOutputStream, new Deflater(Deflater.DEFAULT_COMPRESSION, true))
        {
            this.entries = new ArrayList();
            this.crc = new Crc32();
            this.curEntry = null;
            this.startPosition = 0L;
            this.additionalStream = null;
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
            int totalOut = 0;
            if (this.curMethod == CompressionMethod.Deflated)
            {
                base.Finish();
                totalOut = base.def.TotalOut;
            }
            else if (this.curMethod == ((CompressionMethod) 12))
            {
                this.additionalStream.Close();
                this.additionalStream = null;
                totalOut = (int) (base.baseOutputStream.Position - this.startPosition);
            }
            else
            {
                totalOut = this.size;
            }
            if (this.curEntry.Size < 0L)
            {
                this.curEntry.Size = this.size;
            }
            else if (this.curEntry.Size != this.size)
            {
                throw new ZipException(Conversions.ToString((double) (((Conversions.ToDouble("size was ") + this.size) + Conversions.ToDouble(", but I expected ")) + this.curEntry.Size)));
            }
            if (this.curEntry.CompressedSize < 0L)
            {
                this.curEntry.CompressedSize = totalOut;
            }
            else if (this.curEntry.CompressedSize != totalOut)
            {
                throw new ZipException(Conversions.ToString((double) (((Conversions.ToDouble("compressed size was ") + totalOut) + Conversions.ToDouble(", but I expected ")) + this.curEntry.CompressedSize)));
            }
            if (this.curEntry.Crc < 0L)
            {
                this.curEntry.Crc = this.crc.Value;
            }
            else if (this.curEntry.Crc != this.crc.Value)
            {
                throw new ZipException(Conversions.ToString((double) (((Conversions.ToDouble("crc was ") + this.crc.Value) + Conversions.ToDouble(", but I expected ")) + this.curEntry.Crc)));
            }
            this.offset += totalOut;
            if ((this.curMethod != CompressionMethod.Stored) && ((this.curEntry.flags & 8) != 0))
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
                IEnumerator enumerator = null;
                if (this.curEntry != null)
                {
                    this.CloseEntry();
                }
                int num = 0;
                int num2 = 0;
                try
                {
                    enumerator = this.entries.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ZipEntry current = (ZipEntry) enumerator.Current;
                        CompressionMethod compressionMethod = current.CompressionMethod;
                        this.WriteLeInt(0x2014b50);
                        this.WriteLeShort((compressionMethod == CompressionMethod.Stored) ? 10 : 20);
                        this.WriteLeShort((compressionMethod == CompressionMethod.Stored) ? 10 : 20);
                        if (current.IsCrypted)
                        {
                            current.flags |= 1;
                        }
                        this.WriteLeShort(current.flags);
                        this.WriteLeShort((short) compressionMethod);
                        this.WriteLeInt((int) current.DosTime);
                        this.WriteLeInt((int) current.Crc);
                        this.WriteLeInt((int) current.CompressedSize);
                        this.WriteLeInt((int) current.Size);
                        byte[] buffer = ZipConstants.ConvertToArray(current.Name);
                        if (buffer.Length > 0xffff)
                        {
                            throw new ZipException("Name too long.");
                        }
                        byte[] extraData = current.ExtraData;
                        if (extraData == null)
                        {
                            extraData = new byte[0];
                        }
                        string comment = current.Comment;
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
                        this.WriteLeInt(current.offset);
                        base.baseOutputStream.Write(buffer, 0, buffer.Length);
                        base.baseOutputStream.Write(extraData, 0, extraData.Length);
                        base.baseOutputStream.Write(buffer3, 0, buffer3.Length);
                        num++;
                        num2 += ((0x2e + buffer.Length) + extraData.Length) + buffer3.Length;
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
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
            if (this.curEntry != null)
            {
                this.CloseEntry();
            }
            CompressionMethod compressionMethod = entry.CompressionMethod;
            int num = 0;
            switch (compressionMethod)
            {
                case CompressionMethod.Stored:
                    if (entry.CompressedSize < 0L)
                    {
                        entry.CompressedSize = entry.Size;
                        break;
                    }
                    if (entry.Size >= 0L)
                    {
                        if (entry.Size == entry.CompressedSize)
                        {
                            break;
                        }
                        throw new ZipException("Method STORED, but compressed size != size");
                    }
                    entry.Size = entry.CompressedSize;
                    break;

                case CompressionMethod.Deflated:
                    if (((entry.CompressedSize < 0L) || (entry.Size < 0L)) || (entry.Crc < 0L))
                    {
                        num |= 8;
                    }
                    goto Label_0174;

                case ((CompressionMethod) 12):
                    this.startPosition = base.baseOutputStream.Position;
                    this.additionalStream = new BZip2OutputStream(new NoCloseSubStream(base.baseOutputStream));
                    if (((entry.CompressedSize < 0L) || (entry.Size < 0L)) || (entry.Crc < 0L))
                    {
                        num |= 8;
                    }
                    goto Label_0174;

                default:
                    goto Label_0174;
            }
            if (entry.Size < 0L)
            {
                throw new ZipException("Method STORED, but size not set");
            }
            if (entry.Crc < 0L)
            {
                throw new ZipException("Method STORED, but crc not set");
            }
        Label_0174:
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
            if (((method != 0) && (method != 8)) && (method != 12))
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

                case ((CompressionMethod) 12):
                    this.additionalStream.Write(b, off, len);
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

