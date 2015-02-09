namespace Ionic.Zip
{
    using Ionic.Zlib;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ZipEntry
    {
        internal long __FileDataPosition = -1L;
        private Encoding _actualEncoding = null;
        internal WinZipAesCrypto _aesCrypto;
        internal Stream _archiveStream;
        internal short _BitField;
        internal string _Comment;
        private byte[] _CommentBytes;
        private short _commentLength;
        internal long _CompressedFileDataSize;
        internal long _CompressedSize;
        internal short _CompressionMethod;
        internal int _Crc32;
        private bool _crcCalculated = false;
        internal EncryptionAlgorithm _Encryption = EncryptionAlgorithm.None;
        private byte[] _EntryHeader;
        private int _ExternalFileAttrs;
        internal byte[] _Extra;
        private short _extraFieldLength;
        private string _FileNameInArchive;
        private short _filenameLength;
        private bool _ForceNoCompression;
        private short _InternalFileAttrs;
        private bool _ioOperationCanceled;
        private bool _IsDirectory;
        private bool _IsZip64Format;
        internal short _KeyStrengthInBits;
        internal DateTime _LastModified;
        private int _LengthOfDirEntry;
        internal int _LengthOfHeader;
        internal int _LengthOfTrailer;
        internal string _LocalFileName;
        private bool _metadataChanged;
        private bool _OverwriteOnExtract;
        internal string _Password;
        private bool _presumeZip64;
        private Encoding _provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
        internal long _RelativeOffsetOfLocalHeader;
        internal EntrySource _Source = EntrySource.None;
        private Stream _sourceStream;
        internal int _TimeBlob;
        private long _TotalEntrySize;
        private bool _TrimVolumeFromFullyQualifiedPaths = true;
        internal long _UncompressedSize;
        private short _VersionMadeBy;
        internal short _VersionNeeded;
        internal byte[] _WeakEncryptionHeader;
        private short _WinZipAesMethod;
        internal ZipCrypto _zipCrypto;
        internal ZipFile _zipfile;
        private static Encoding ibm437 = Encoding.GetEncoding("IBM437");
        private object LOCK = new object();
        private const int Rfc2898KeygenIterations = 0x3e8;
        private const int WORKING_BUFFER_SIZE = 0x4400;

        internal ZipEntry()
        {
        }

        private void _CheckRead(int nbytes)
        {
            if (nbytes == 0)
            {
                throw new BadReadException(string.Format("bad read of entry {0} from compressed archive.", this.FileName));
            }
        }

        private void _EmitOne(Stream outstream)
        {
            this._WriteSecurityMetadata(outstream);
            this._WriteFileData(outstream);
            this._TotalEntrySize = this._LengthOfHeader + this._CompressedSize;
        }

        private int _ExtractOne(Stream output)
        {
            Stream archiveStream = this.ArchiveStream;
            archiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
            int num = 0;
            byte[] buffer = new byte[0x4400];
            long num2 = (this.CompressionMethod == 8) ? this.UncompressedSize : this._CompressedFileDataSize;
            Stream stream = null;
            if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
            {
                stream = new ZipCipherStream(archiveStream, this._zipCrypto, CryptoMode.Decrypt);
            }
            else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
            {
                stream = new WinZipAesCipherStream(archiveStream, this._aesCrypto, this._CompressedFileDataSize, CryptoMode.Decrypt);
            }
            else
            {
                stream = new CrcCalculatorStream(archiveStream, this._CompressedFileDataSize);
            }
            Stream stream3 = (this.CompressionMethod == 8) ? new DeflateStream(stream, CompressionMode.Decompress, true) : stream;
            int bytesWritten = 0;
            using (CrcCalculatorStream stream4 = new CrcCalculatorStream(stream3))
            {
                while (num2 > 0L)
                {
                    int count = (num2 > buffer.Length) ? buffer.Length : ((int)num2);
                    int nbytes = stream4.Read(buffer, 0, count);
                    this._CheckRead(nbytes);
                    output.Write(buffer, 0, nbytes);
                    num2 -= nbytes;
                    bytesWritten += nbytes;
                    this.OnExtractProgress(bytesWritten, this.UncompressedSize);
                    if (this._ioOperationCanceled)
                    {
                        break;
                    }
                }
                num = stream4.Crc32;
                if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    WinZipAesCipherStream stream5 = stream as WinZipAesCipherStream;
                    this._aesCrypto.CalculatedMac = stream5.FinalAuthentication;
                }
            }
            return num;
        }

        private void _WriteFileData(Stream s)
        {
            Stream stream = null;
            CrcCalculatorStream stream2 = null;
            CountingStream stream3 = null;
            try
            {
                this.__FileDataPosition = s.Position;
            }
            catch
            {
            }
            try
            {
                if (this._sourceStream != null)
                {
                    this._sourceStream.Position = 0L;
                    stream = this._sourceStream;
                }
                else
                {
                    stream = File.OpenRead(this.LocalFileName);
                }
                long totalBytesToXfer = 0L;
                if (this._sourceStream == null)
                {
                    FileInfo info = new FileInfo(this.LocalFileName);
                    totalBytesToXfer = info.Length;
                }
                stream2 = new CrcCalculatorStream(stream);
                stream3 = new CountingStream(s);
                Stream stream4 = stream3;
                if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
                {
                    stream4 = new ZipCipherStream(stream3, this._zipCrypto, CryptoMode.Encrypt);
                }
                else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    stream4 = new WinZipAesCipherStream(stream3, this._aesCrypto, CryptoMode.Encrypt);
                }
                Stream stream5 = null;
                bool flag2 = false;
                if (this.CompressionMethod == 8)
                {
                    stream5 = new DeflateStream(stream4, CompressionMode.Compress, this._zipfile.CompressionLevel, true);
                    flag2 = true;
                }
                else
                {
                    stream5 = stream4;
                }
                byte[] buffer = new byte[0x4400];
                for (int i = stream2.Read(buffer, 0, 0x4400); i > 0; i = stream2.Read(buffer, 0, 0x4400))
                {
                    stream5.Write(buffer, 0, i);
                    this.OnWriteBlock(stream2.TotalBytesSlurped, totalBytesToXfer);
                    if (this._ioOperationCanceled)
                    {
                        break;
                    }
                }
                if (flag2)
                {
                    stream5.Close();
                }
                stream4.Flush();
                stream4.Close();
                WinZipAesCipherStream stream6 = stream4 as WinZipAesCipherStream;
                if (stream6 != null)
                {
                    s.Write(stream6.FinalAuthentication, 0, 10);
                }
            }
            finally
            {
                if ((this._sourceStream == null) && (stream != null))
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            if (!this._ioOperationCanceled)
            {
                short num7;
                this._UncompressedSize = stream2.TotalBytesSlurped;
                this._CompressedSize = stream3.BytesWritten;
                this._Crc32 = stream2.Crc32;
                if (this._Password != null)
                {
                    if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
                    {
                        this._CompressedSize += 12L;
                    }
                    else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                    {
                        this._CompressedSize += this._aesCrypto.SizeOfEncryptionMetadata;
                    }
                }
                int destinationIndex = 8;
                this._EntryHeader[destinationIndex++] = (byte)(this.CompressionMethod & 0xff);
                this._EntryHeader[destinationIndex++] = (byte)((this.CompressionMethod & 0xff00) >> 8);
                destinationIndex = 14;
                this._EntryHeader[destinationIndex++] = (byte)(this._Crc32 & 0xff);
                this._EntryHeader[destinationIndex++] = (byte)((this._Crc32 & 0xff00) >> 8);
                this._EntryHeader[destinationIndex++] = (byte)((this._Crc32 & 0xff0000) >> 0x10);
                this._EntryHeader[destinationIndex++] = (byte)((this._Crc32 & 0xff000000L) >> 0x18);
                if ((this._zipfile._zip64 == Zip64Option.Default) && ((((uint)this._CompressedSize) >= uint.MaxValue) || (((uint)this._UncompressedSize) >= uint.MaxValue)))
                {
                    throw new ZipException("Compressed or Uncompressed size exceeds maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
                }
                bool flag4 = this._IsZip64Format = (((this._zipfile._zip64 == Zip64Option.Always) || (((uint)this._CompressedSize) >= uint.MaxValue)) || (((uint)this._UncompressedSize) >= uint.MaxValue)) || (((uint)this._RelativeOffsetOfLocalHeader) >= uint.MaxValue);
                short num4 = (short)(this._EntryHeader[0x1a] + (this._EntryHeader[0x1b] * 0x100));
                short num5 = (short)(this._EntryHeader[0x1c] + (this._EntryHeader[0x1d] * 0x100));
                if (flag4)
                {
                    this._EntryHeader[4] = 0x2d;
                    this._EntryHeader[5] = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        this._EntryHeader[destinationIndex++] = 0xff;
                    }
                    destinationIndex = 30 + num4;
                    this._EntryHeader[destinationIndex++] = 1;
                    this._EntryHeader[destinationIndex++] = 0;
                    destinationIndex += 2;
                    Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, this._EntryHeader, destinationIndex, 8);
                    destinationIndex += 8;
                    Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, this._EntryHeader, destinationIndex, 8);
                    destinationIndex += 8;
                    Array.Copy(BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, this._EntryHeader, destinationIndex, 8);
                }
                else
                {
                    this._EntryHeader[4] = 20;
                    this._EntryHeader[5] = 0;
                    destinationIndex = 0x12;
                    this._EntryHeader[destinationIndex++] = (byte)(this._CompressedSize & 0xffL);
                    this._EntryHeader[destinationIndex++] = (byte)((this._CompressedSize & 0xff00L) >> 8);
                    this._EntryHeader[destinationIndex++] = (byte)((this._CompressedSize & 0xff0000L) >> 0x10);
                    this._EntryHeader[destinationIndex++] = (byte)((this._CompressedSize & 0xff000000L) >> 0x18);
                    this._EntryHeader[destinationIndex++] = (byte)(this._UncompressedSize & 0xffL);
                    this._EntryHeader[destinationIndex++] = (byte)((this._UncompressedSize & 0xff00L) >> 8);
                    this._EntryHeader[destinationIndex++] = (byte)((this._UncompressedSize & 0xff0000L) >> 0x10);
                    this._EntryHeader[destinationIndex++] = (byte)((this._UncompressedSize & 0xff000000L) >> 0x18);
                    if (num5 != 0)
                    {
                        destinationIndex = 30 + num4;
                        num7 = (short)(this._EntryHeader[destinationIndex + 2] + (this._EntryHeader[destinationIndex + 3] * 0x100));
                        if (num7 == 0x1c)
                        {
                            this._EntryHeader[destinationIndex++] = 0x99;
                            this._EntryHeader[destinationIndex++] = 0x99;
                        }
                    }
                }
                if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    destinationIndex = 8;
                    this._EntryHeader[destinationIndex++] = 0x63;
                    this._EntryHeader[destinationIndex++] = 0;
                    destinationIndex = 30 + num4;
                    do
                    {
                        ushort num8 = (ushort)(this._EntryHeader[destinationIndex] + (this._EntryHeader[destinationIndex + 1] * 0x100));
                        num7 = (short)(this._EntryHeader[destinationIndex + 2] + (this._EntryHeader[destinationIndex + 3] * 0x100));
                        if (num8 != 0x9901)
                        {
                            destinationIndex += num7 + 4;
                        }
                        else
                        {
                            destinationIndex += 9;
                            this._EntryHeader[destinationIndex++] = (byte)(this._CompressionMethod & 0xff);
                            this._EntryHeader[destinationIndex++] = (byte)(this._CompressionMethod & 0xff00);
                        }
                    }
                    while (destinationIndex < ((num5 - 30) - num4));
                }
                if (s.CanSeek)
                {
                    s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
                    s.Write(this._EntryHeader, 0, this._EntryHeader.Length);
                    CountingStream stream7 = s as CountingStream;
                    if (stream7 != null)
                    {
                        stream7.Adjust((long)this._EntryHeader.Length);
                    }
                    s.Seek(this._CompressedSize, SeekOrigin.Current);
                }
                else
                {
                    if ((this._BitField & 8) != 8)
                    {
                        throw new ZipException("Logic error.");
                    }
                    byte[] destinationArray = null;
                    if ((this._zipfile._zip64 == Zip64Option.Always) || (this._zipfile._zip64 == Zip64Option.AsNecessary))
                    {
                        destinationArray = new byte[0x18];
                        destinationIndex = 0;
                        Array.Copy(BitConverter.GetBytes(0x8074b50), 0, destinationArray, destinationIndex, 4);
                        destinationIndex += 4;
                        Array.Copy(BitConverter.GetBytes(this._Crc32), 0, destinationArray, destinationIndex, 4);
                        destinationIndex += 4;
                        Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, destinationArray, destinationIndex, 8);
                        destinationIndex += 8;
                        Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, destinationArray, destinationIndex, 8);
                        destinationIndex += 8;
                    }
                    else
                    {
                        destinationArray = new byte[0x10];
                        destinationIndex = 0;
                        int num9 = 0x8074b50;
                        destinationArray[destinationIndex++] = (byte)(num9 & 0xff);
                        destinationArray[destinationIndex++] = (byte)((num9 & 0xff00) >> 8);
                        destinationArray[destinationIndex++] = (byte)((num9 & 0xff0000) >> 0x10);
                        destinationArray[destinationIndex++] = (byte)((num9 & 0xff000000L) >> 0x18);
                        destinationArray[destinationIndex++] = (byte)(this._Crc32 & 0xff);
                        destinationArray[destinationIndex++] = (byte)((this._Crc32 & 0xff00) >> 8);
                        destinationArray[destinationIndex++] = (byte)((this._Crc32 & 0xff0000) >> 0x10);
                        destinationArray[destinationIndex++] = (byte)((this._Crc32 & 0xff000000L) >> 0x18);
                        destinationArray[destinationIndex++] = (byte)(this._CompressedSize & 0xffL);
                        destinationArray[destinationIndex++] = (byte)((this._CompressedSize & 0xff00L) >> 8);
                        destinationArray[destinationIndex++] = (byte)((this._CompressedSize & 0xff0000L) >> 0x10);
                        destinationArray[destinationIndex++] = (byte)((this._CompressedSize & 0xff000000L) >> 0x18);
                        destinationArray[destinationIndex++] = (byte)(this._UncompressedSize & 0xffL);
                        destinationArray[destinationIndex++] = (byte)((this._UncompressedSize & 0xff00L) >> 8);
                        destinationArray[destinationIndex++] = (byte)((this._UncompressedSize & 0xff0000L) >> 0x10);
                        destinationArray[destinationIndex++] = (byte)((this._UncompressedSize & 0xff000000L) >> 0x18);
                    }
                    s.Write(destinationArray, 0, destinationArray.Length);
                }
            }
        }

        private void _WriteSecurityMetadata(Stream outstream)
        {
            if (this._Password != null)
            {
                if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
                {
                    this._zipCrypto = ZipCrypto.ForWrite(this._Password);
                    Random random = new Random();
                    byte[] buffer = new byte[12];
                    random.NextBytes(buffer);
                    this.FigureCrc32();
                    buffer[11] = (byte)((this._Crc32 >> 0x18) & 0xff);
                    byte[] buffer2 = this._zipCrypto.EncryptMessage(buffer, buffer.Length);
                    outstream.Write(buffer2, 0, buffer2.Length);
                }
                else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    this._aesCrypto = WinZipAesCrypto.Generate(this._Password, this._KeyStrengthInBits);
                    outstream.Write(this._aesCrypto.Salt, 0, this._aesCrypto._Salt.Length);
                    outstream.Write(this._aesCrypto.GeneratedPV, 0, this._aesCrypto.GeneratedPV.Length);
                }
            }
        }

        private byte[] ConsExtraField()
        {
            byte[] destinationArray = null;
            byte[] sourceArray = null;
            int num;
            if (this._zipfile._zip64 != Zip64Option.Default)
            {
                destinationArray = new byte[0x20];
                num = 0;
                if (this._presumeZip64)
                {
                    destinationArray[num++] = 1;
                    destinationArray[num++] = 0;
                }
                else
                {
                    destinationArray[num++] = 0x99;
                    destinationArray[num++] = 0x99;
                }
                destinationArray[num++] = 0x1c;
                destinationArray[num++] = 0;
                Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, destinationArray, num, 8);
                num += 8;
                Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, destinationArray, num, 8);
                num += 8;
                Array.Copy(BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, destinationArray, num, 8);
                num += 8;
                Array.Copy(BitConverter.GetBytes(0), 0, destinationArray, num, 4);
            }
            if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
            {
                sourceArray = new byte[11];
                num = 0;
                sourceArray[num++] = 1;
                sourceArray[num++] = 0x99;
                sourceArray[num++] = 7;
                sourceArray[num++] = 0;
                sourceArray[num++] = 1;
                sourceArray[num++] = 0;
                sourceArray[num++] = 0x41;
                sourceArray[num++] = 0x45;
                sourceArray[num] = 0xff;
                if (this._KeyStrengthInBits == 0x80)
                {
                    sourceArray[num] = 1;
                }
                if (this._KeyStrengthInBits == 0x100)
                {
                    sourceArray[num] = 3;
                }
                num++;
                sourceArray[num++] = (byte)(this._CompressionMethod & 0xff);
                sourceArray[num++] = (byte)(this._CompressionMethod & 0xff00);
            }
            byte[] buffer3 = null;
            int num2 = 0;
            if (destinationArray != null)
            {
                num2 += destinationArray.Length;
            }
            if (sourceArray != null)
            {
                num2 += sourceArray.Length;
            }
            if (num2 > 0)
            {
                buffer3 = new byte[num2];
                int destinationIndex = 0;
                if (destinationArray != null)
                {
                    Array.Copy(destinationArray, 0, buffer3, destinationIndex, destinationArray.Length);
                    destinationIndex += destinationArray.Length;
                }
                if (sourceArray != null)
                {
                    Array.Copy(sourceArray, 0, buffer3, destinationIndex, sourceArray.Length);
                    destinationIndex += sourceArray.Length;
                }
                this._LengthOfHeader = num2;
            }
            return buffer3;
        }

        internal void CopyMetaData(ZipEntry source)
        {
            this.__FileDataPosition = source.__FileDataPosition;
            this.CompressionMethod = source.CompressionMethod;
            this._CompressedFileDataSize = source._CompressedFileDataSize;
            this._UncompressedSize = source._UncompressedSize;
            this._BitField = source._BitField;
            this._LastModified = source._LastModified;
        }

        private void CopyThroughOneEntry(Stream outstream)
        {
            long num3;
            int num4;
            int num5;
            byte[] buffer = new byte[0x4400];
            Stream archiveStream = this.ArchiveStream;
            if ((this._metadataChanged || (this._IsZip64Format && (this._zipfile.UseZip64WhenSaving == Zip64Option.Default))) || (!this._IsZip64Format && (this._zipfile.UseZip64WhenSaving == Zip64Option.Always)))
            {
                long num = this._RelativeOffsetOfLocalHeader;
                if (this.LengthOfHeader == 0)
                {
                    throw new ZipException("Bad header length.");
                }
                int lengthOfHeader = this.LengthOfHeader;
                this.WriteHeader(outstream, 0);
                if (lengthOfHeader != 0)
                {
                    archiveStream.Seek(num + lengthOfHeader, SeekOrigin.Begin);
                    for (num3 = this._CompressedSize; num3 > 0L; num3 -= num5)
                    {
                        num4 = (num3 > buffer.Length) ? buffer.Length : ((int)num3);
                        num5 = archiveStream.Read(buffer, 0, num4);
                        this._CheckRead(num5);
                        outstream.Write(buffer, 0, num5);
                    }
                }
                this._TotalEntrySize = this._LengthOfHeader + this._CompressedSize;
            }
            else
            {
                if (this.LengthOfHeader == 0)
                {
                    throw new ZipException("Bad header length.");
                }
                archiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
                this._EntryHeader = new byte[this._LengthOfHeader];
                num5 = archiveStream.Read(this._EntryHeader, 0, this._EntryHeader.Length);
                this._CheckRead(num5);
                archiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
                if (this._TotalEntrySize == 0L)
                {
                    this._TotalEntrySize = this._LengthOfHeader + this._CompressedSize;
                }
                CountingStream stream2 = outstream as CountingStream;
                this._RelativeOffsetOfLocalHeader = (stream2 != null) ? ((int)stream2.BytesWritten) : ((int)outstream.Position);
                for (num3 = this._TotalEntrySize; num3 > 0L; num3 -= num5)
                {
                    num4 = (num3 > buffer.Length) ? buffer.Length : ((int)num3);
                    num5 = archiveStream.Read(buffer, 0, num4);
                    this._CheckRead(num5);
                    outstream.Write(buffer, 0, num5);
                }
            }
        }

        internal static ZipEntry Create(string filename, string nameInArchive)
        {
            return Create(filename, nameInArchive, null);
        }

        internal static ZipEntry Create(string filename, string nameInArchive, Stream stream)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ZipException("The entry name must be non-null and non-empty.");
            }
            ZipEntry entry = new ZipEntry();
            if (stream != null)
            {
                entry._sourceStream = stream;
                entry._LastModified = DateTime.Now;
            }
            else
            {
                entry._LastModified = (File.Exists(filename) || Directory.Exists(filename)) ? SharedUtilities.RoundToEvenSecond(File.GetLastWriteTime(filename)) : DateTime.Now;
                if (!(entry._LastModified.IsDaylightSavingTime() || !DateTime.Now.IsDaylightSavingTime()))
                {
                    entry._LastModified += new TimeSpan(1, 0, 0);
                }
                if (!(!entry._LastModified.IsDaylightSavingTime() || DateTime.Now.IsDaylightSavingTime()))
                {
                    entry._LastModified -= new TimeSpan(1, 0, 0);
                }
            }
            entry._LocalFileName = filename;
            entry._FileNameInArchive = nameInArchive.Replace('\\', '/');
            return entry;
        }

        private bool DefaultWantCompression()
        {
            if (this._LocalFileName != null)
            {
                return this.SeemsCompressible(this._LocalFileName);
            }
            if (this._FileNameInArchive != null)
            {
                return this.SeemsCompressible(this._FileNameInArchive);
            }
            return true;
        }

        public void Extract()
        {
            this.InternalExtract(".", null, null);
        }

        public void Extract(bool overwrite)
        {
            this.OverwriteOnExtract = overwrite;
            this.InternalExtract(".", null, null);
        }

        public void Extract(Stream stream)
        {
            this.InternalExtract(null, stream, null);
        }

        public void Extract(string baseDirectory)
        {
            this.InternalExtract(baseDirectory, null, null);
        }

        public void Extract(string baseDirectory, bool overwrite)
        {
            this.OverwriteOnExtract = overwrite;
            this.InternalExtract(baseDirectory, null, null);
        }

        public void ExtractWithPassword(string password)
        {
            this.InternalExtract(".", null, password);
        }

        public void ExtractWithPassword(bool overwrite, string password)
        {
            this.OverwriteOnExtract = overwrite;
            this.InternalExtract(".", null, password);
        }

        public void ExtractWithPassword(Stream stream, string password)
        {
            this.InternalExtract(null, stream, password);
        }

        public void ExtractWithPassword(string baseDirectory, string password)
        {
            this.InternalExtract(baseDirectory, null, password);
        }

        public void ExtractWithPassword(string baseDirectory, bool overwrite, string password)
        {
            this.OverwriteOnExtract = overwrite;
            this.InternalExtract(baseDirectory, null, password);
        }

        private void FigureCompressionMethodForWriting(int cycle)
        {
            if (cycle > 1)
            {
                this._CompressionMethod = 0;
            }
            else if (this.IsDirectory)
            {
                this._CompressionMethod = 0;
            }
            else if (this.__FileDataPosition == -1L)
            {
                long length = 0L;
                if (this._sourceStream != null)
                {
                    length = this._sourceStream.Length;
                }
                else
                {
                    FileInfo info = new FileInfo(this.LocalFileName);
                    length = info.Length;
                }
                if (length == 0L)
                {
                    this._CompressionMethod = 0;
                }
                else if (this._ForceNoCompression)
                {
                    this._CompressionMethod = 0;
                }
                else if (this.WantCompression != null)
                {
                    this._CompressionMethod = this.WantCompression(this.LocalFileName, this._FileNameInArchive) ? ((short)8) : ((short)0);
                }
                else
                {
                    this._CompressionMethod = this.DefaultWantCompression() ? ((short)8) : ((short)0);
                }
            }
        }

        private int FigureCrc32()
        {
            if (!this._crcCalculated)
            {
                Stream input = null;
                if (this._sourceStream != null)
                {
                    this._sourceStream.Position = 0L;
                    input = this._sourceStream;
                }
                else
                {
                    input = File.OpenRead(this.LocalFileName);
                }
                this._Crc32 = new CRC32().GetCrc32(input);
                if (this._sourceStream == null)
                {
                    input.Close();
                    input.Dispose();
                }
                this._crcCalculated = true;
            }
            return this._Crc32;
        }

        private Encoding GenerateCommentBytes()
        {
            this._CommentBytes = ibm437.GetBytes(this._Comment);
            if (ibm437.GetString(this._CommentBytes, 0, this._CommentBytes.Length) == this._Comment)
            {
                return ibm437;
            }
            this._CommentBytes = this._provisionalAlternateEncoding.GetBytes(this._Comment);
            return this._provisionalAlternateEncoding;
        }

        private void GetEncodedBytes(out byte[] result)
        {
            string str = this.FileName.Replace(@"\", "/");
            string s = null;
            if (((this._TrimVolumeFromFullyQualifiedPaths && (this.FileName.Length >= 3)) && (this.FileName[1] == ':')) && (str[2] == '/'))
            {
                s = str.Substring(3);
            }
            else if ((this.FileName.Length >= 4) && ((str[0] == '/') && (str[1] == '/')))
            {
                int index = str.IndexOf('/', 2);
                if (index == -1)
                {
                    throw new ArgumentException("The path for that entry appears to be badly formatted");
                }
                s = str.Substring(index + 1);
            }
            else if ((this.FileName.Length >= 3) && ((str[0] == '.') && (str[1] == '/')))
            {
                s = str.Substring(2);
            }
            else
            {
                s = str;
            }
            result = ibm437.GetBytes(s);
            string str3 = ibm437.GetString(result, 0, result.Length);
            this._CommentBytes = null;
            if (str3 == s)
            {
                if ((this._Comment == null) || (this._Comment.Length == 0))
                {
                    this._actualEncoding = ibm437;
                }
                else
                {
                    Encoding encoding = this.GenerateCommentBytes();
                    if (encoding.CodePage == 0x1b5)
                    {
                        this._actualEncoding = ibm437;
                    }
                    else
                    {
                        this._actualEncoding = encoding;
                        result = encoding.GetBytes(s);
                    }
                }
            }
            else
            {
                result = this._provisionalAlternateEncoding.GetBytes(s);
                if ((this._Comment != null) && (this._Comment.Length != 0))
                {
                    this._CommentBytes = this._provisionalAlternateEncoding.GetBytes(this._Comment);
                }
                this._actualEncoding = this._provisionalAlternateEncoding;
            }
        }

        internal static void HandlePK00Prefix(Stream s)
        {
            if (SharedUtilities.ReadInt(s) != 0x30304b50)
            {
                s.Seek(-4L, SeekOrigin.Current);
            }
        }

        private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
        {
            Stream archiveStream = entry.ArchiveStream;
            if (((ulong)SharedUtilities.ReadInt(archiveStream)) == (ulong)entry._Crc32)
            {
                if (SharedUtilities.ReadInt(archiveStream) == entry._CompressedSize)
                {
                    if (SharedUtilities.ReadInt(archiveStream) != entry._UncompressedSize)
                    {
                        archiveStream.Seek(-12L, SeekOrigin.Current);
                    }
                }
                else
                {
                    archiveStream.Seek(-8L, SeekOrigin.Current);
                }
            }
            else
            {
                archiveStream.Seek(-4L, SeekOrigin.Current);
            }
        }

        private void InternalExtract(string baseDir, Stream outstream, string password)
        {
            this.OnBeforeExtract(baseDir);
            this._ioOperationCanceled = false;
            string outputFile = null;
            Stream output = null;
            bool flag = false;
            try
            {
                this.ValidateCompression();
                this.ValidateEncryption();
                if (!this.ValidateOutput(baseDir, outstream, out outputFile))
                {
                    if (password == null)
                    {
                        password = this._Password;
                    }
                    this.SetupCrypto(password);
                    if (outputFile != null)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                        }
                        if (File.Exists(outputFile))
                        {
                            flag = true;
                            if (!this.OverwriteOnExtract)
                            {
                                throw new ZipException("The file already exists.");
                            }
                            File.Delete(outputFile);
                        }
                        output = new FileStream(outputFile, FileMode.CreateNew);
                    }
                    else
                    {
                        output = outstream;
                    }
                    if (this._ioOperationCanceled)
                    {
                        try
                        {
                            if (outputFile != null)
                            {
                                if (output != null)
                                {
                                    output.Close();
                                }
                                if (File.Exists(outputFile))
                                {
                                    File.Delete(outputFile);
                                }
                            }
                        }
                        finally
                        {
                        }
                    }
                    int num = this._ExtractOne(output);
                    if (this._ioOperationCanceled)
                    {
                        try
                        {
                            if (outputFile != null)
                            {
                                if (output != null)
                                {
                                    output.Close();
                                }
                                if (File.Exists(outputFile))
                                {
                                    File.Delete(outputFile);
                                }
                            }
                        }
                        finally
                        {
                        }
                    }
                    if ((num != this._Crc32) && (((this.Encryption != EncryptionAlgorithm.WinZipAes128) && (this.Encryption != EncryptionAlgorithm.WinZipAes256)) || (this._WinZipAesMethod != 2)))
                    {
                        throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + string.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", this._Crc32, num));
                    }
                    if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                    {
                        this._aesCrypto.ReadAndVerifyMac(this.ArchiveStream);
                    }
                    if (outputFile != null)
                    {
                        output.Close();
                        output = null;
                        DateTime lastModified = this.LastModified;
                        if (!(!DateTime.Now.IsDaylightSavingTime() || this.LastModified.IsDaylightSavingTime()))
                        {
                            lastModified = this.LastModified - new TimeSpan(1, 0, 0);
                        }
                        File.SetLastWriteTime(outputFile, lastModified);
                    }
                    this.OnAfterExtract(baseDir);
                }
            }
            catch
            {
                try
                {
                    if (outputFile != null)
                    {
                        if (output != null)
                        {
                            output.Close();
                        }
                        if (File.Exists(outputFile) && !(flag && !this.OverwriteOnExtract))
                        {
                            File.Delete(outputFile);
                        }
                    }
                }
                finally
                {
                }
                throw;
            }
        }

        private CrcCalculatorStream InternalOpenReader(string password)
        {
            this.ValidateCompression();
            this.ValidateEncryption();
            this.SetupCrypto(password);
            Stream archiveStream = this.ArchiveStream;
            this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
            Stream stream = archiveStream;
            if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
            {
                stream = new ZipCipherStream(archiveStream, this._zipCrypto, CryptoMode.Decrypt);
            }
            else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
            {
                stream = new WinZipAesCipherStream(archiveStream, this._aesCrypto, this._CompressedFileDataSize, CryptoMode.Decrypt);
            }
            return new CrcCalculatorStream((this.CompressionMethod == 8) ? new DeflateStream(stream, CompressionMode.Decompress, true) : stream, this._UncompressedSize);
        }

        private static bool IsNotValidSig(int signature)
        {
            return (signature != 0x4034b50);
        }

        internal static bool IsNotValidZipDirEntrySig(int signature)
        {
            return (signature != 0x2014b50);
        }

        internal static bool IsStrong(EncryptionAlgorithm e)
        {
            return ((e != EncryptionAlgorithm.None) && (e != EncryptionAlgorithm.PkzipWeak));
        }

        internal void MarkAsDirectory()
        {
            this._IsDirectory = true;
            if (!this._FileNameInArchive.EndsWith("/"))
            {
                this._FileNameInArchive = this._FileNameInArchive + "/";
            }
        }

        internal static string NameInArchive(string filename, string directoryPathInArchive)
        {
            string pathName = null;
            if (directoryPathInArchive == null)
            {
                pathName = filename;
            }
            else if (string.IsNullOrEmpty(directoryPathInArchive))
            {
                pathName = Path.GetFileName(filename);
            }
            else
            {
                pathName = Path.Combine(directoryPathInArchive, Path.GetFileName(filename));
            }
            return SharedUtilities.TrimVolumeAndSwapSlashes(pathName);
        }

        private void OnAfterExtract(string path)
        {
            if (!this._zipfile._inExtractAll)
            {
                this._zipfile.OnSingleEntryExtract(this, path, false, this.OverwriteOnExtract);
            }
        }

        private void OnBeforeExtract(string path)
        {
            if (!this._zipfile._inExtractAll)
            {
                this._ioOperationCanceled = this._zipfile.OnSingleEntryExtract(this, path, true, this.OverwriteOnExtract);
            }
        }

        private void OnExtractProgress(int bytesWritten, long totalBytesToWrite)
        {
            this._ioOperationCanceled = this._zipfile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
        }

        private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
        {
            this._ioOperationCanceled = this._zipfile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
        }

        public CrcCalculatorStream OpenReader()
        {
            return this.InternalOpenReader(this._Password);
        }

        public CrcCalculatorStream OpenReader(string password)
        {
            return this.InternalOpenReader(password);
        }

        internal int ProcessExtraField(short extraFieldLength)
        {
            int num = 0;
            Stream archiveStream = this.ArchiveStream;
            if (extraFieldLength > 0)
            {
                int num3;
                short num5;
                byte[] buffer = this._Extra = new byte[extraFieldLength];
                num = archiveStream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i = (num3 + num5) + 4)
                {
                    num3 = i;
                    ushort num4 = (ushort)(buffer[i] + (buffer[i + 1] * 0x100));
                    num5 = (short)(buffer[i + 2] + (buffer[i + 3] * 0x100));
                    i += 4;
                    switch (num4)
                    {
                        case 1:
                            if (num5 > 0x1c)
                            {
                                throw new BadReadException(string.Format("  Inconsistent ZIP64 datasize (0x{0:X4}) at position 0x{1:X16}", num5, archiveStream.Position - num));
                            }
                            if (this._UncompressedSize == 0xffffffffL)
                            {
                                this._UncompressedSize = BitConverter.ToInt64(buffer, i);
                                i += 8;
                            }
                            if (this._CompressedSize == 0xffffffffL)
                            {
                                this._CompressedSize = BitConverter.ToInt64(buffer, i);
                                i += 8;
                            }
                            if (this._RelativeOffsetOfLocalHeader == 0xffffffffL)
                            {
                                this._RelativeOffsetOfLocalHeader = BitConverter.ToInt64(buffer, i);
                                i += 8;
                            }
                            break;

                        case 0x9901:
                            if (this._CompressionMethod == 0x63)
                            {
                                if (num5 != 7)
                                {
                                    throw new BadReadException(string.Format("  Inconsistent WinZip AES datasize (0x{0:X4}) at position 0x{1:X16}", num5, archiveStream.Position - num));
                                }
                                this._WinZipAesMethod = BitConverter.ToInt16(buffer, i);
                                i += 2;
                                if ((this._WinZipAesMethod != 1) && (this._WinZipAesMethod != 2))
                                {
                                    throw new BadReadException(string.Format("  Unexpected vendor version number (0x{0:X4}) for WinZip AES metadata at position 0x{1:X16}", this._WinZipAesMethod, archiveStream.Position - num));
                                }
                                short num7 = BitConverter.ToInt16(buffer, i);
                                i += 2;
                                if (num7 != 0x4541)
                                {
                                    throw new BadReadException(string.Format("  Unexpected vendor ID (0x{0:X4}) for WinZip AES metadata at position 0x{1:X16}", num7, archiveStream.Position - num));
                                }
                                this._KeyStrengthInBits = -1;
                                if (buffer[i] == 1)
                                {
                                    this._KeyStrengthInBits = 0x80;
                                }
                                if (buffer[i] == 3)
                                {
                                    this._KeyStrengthInBits = 0x100;
                                }
                                if (this._KeyStrengthInBits < 0)
                                {
                                    throw new Exception(string.Format("Invalid key strength ({0})", this._KeyStrengthInBits));
                                }
                                this.Encryption = (this._KeyStrengthInBits == 0x80) ? EncryptionAlgorithm.WinZipAes128 : EncryptionAlgorithm.WinZipAes256;
                                i++;
                                this._CompressionMethod = BitConverter.ToInt16(buffer, i);
                                i += 2;
                            }
                            break;
                    }
                }
            }
            return num;
        }

        internal static ZipEntry Read(ZipFile zf, bool first)
        {
            Stream readStream = zf.ReadStream;
            Encoding provisionalAlternateEncoding = zf.ProvisionalAlternateEncoding;
            ZipEntry ze = new ZipEntry();
            ze._Source = EntrySource.Zipfile;
            ze._zipfile = zf;
            ze._archiveStream = readStream;
            zf.OnReadEntry(true, null);
            if (first)
            {
                HandlePK00Prefix(readStream);
            }
            if (!ReadHeader(ze, provisionalAlternateEncoding))
            {
                return null;
            }
            ze.__FileDataPosition = ze.ArchiveStream.Position;
            readStream.Seek(ze._CompressedFileDataSize, SeekOrigin.Current);
            if (!(((ze._BitField & 8) != 8) || ze.FileName.EndsWith("/")))
            {
                int num = ze._IsZip64Format ? 0x18 : 0x10;
                readStream.Seek((long)num, SeekOrigin.Current);
            }
            HandleUnexpectedDataDescriptor(ze);
            zf.OnReadBytes(ze);
            zf.OnReadEntry(false, ze);
            return ze;
        }

        public static ZipEntry ReadDirEntry(Stream s, Encoding expectedEncoding)
        {
            int signature = SharedUtilities.ReadSignature(s);
            if (IsNotValidZipDirEntrySig(signature))
            {
                s.Seek(-4L, SeekOrigin.Current);
                if ((signature != 0x6054b50L) && (signature != 0x6064b50L))
                {
                    throw new BadReadException(string.Format("  ZipEntry::ReadDirEntry(): Bad signature (0x{0:X8}) at position 0x{1:X8}", signature, s.Position));
                }
                return null;
            }
            int num2 = 0x2e;
            byte[] buffer = new byte[0x2a];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                return null;
            }
            int num4 = 0;
            ZipEntry entry2 = new ZipEntry();
            entry2._archiveStream = s;
            entry2._VersionMadeBy = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._VersionNeeded = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._BitField = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._CompressionMethod = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._TimeBlob = ((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100);
            entry2._LastModified = SharedUtilities.PackedToDateTime(entry2._TimeBlob);
            entry2._Crc32 = ((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100);
            entry2._CompressedSize = (long)((ulong)(((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100)));
            entry2._UncompressedSize = (long)((ulong)(((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100)));
            entry2._filenameLength = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._extraFieldLength = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._commentLength = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            num4 += 2;
            entry2._InternalFileAttrs = (short)(buffer[num4++] + (buffer[num4++] * 0x100));
            entry2._ExternalFileAttrs = ((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100);
            entry2._RelativeOffsetOfLocalHeader = (long)((ulong)(((buffer[num4++] + (buffer[num4++] * 0x100)) + ((buffer[num4++] * 0x100) * 0x100)) + (((buffer[num4++] * 0x100) * 0x100) * 0x100)));
            buffer = new byte[entry2._filenameLength];
            int num3 = s.Read(buffer, 0, buffer.Length);
            num2 += num3;
            if ((entry2._BitField & 0x800) == 0x800)
            {
                entry2._LocalFileName = SharedUtilities.Utf8StringFromBuffer(buffer, buffer.Length);
            }
            else
            {
                entry2._LocalFileName = SharedUtilities.StringFromBuffer(buffer, buffer.Length, expectedEncoding);
            }
            entry2._FileNameInArchive = entry2._LocalFileName;
            if (entry2.AttributesIndicateDirectory)
            {
                entry2.MarkAsDirectory();
            }
            if (entry2._LocalFileName.EndsWith("/"))
            {
                entry2.MarkAsDirectory();
            }
            entry2._CompressedFileDataSize = entry2._CompressedSize;
            if ((entry2._BitField & 1) == 1)
            {
                entry2._Encryption = EncryptionAlgorithm.PkzipWeak;
            }
            if (entry2._extraFieldLength > 0)
            {
                bool flag2 = ((((uint)entry2._CompressedSize) == uint.MaxValue) || (((uint)entry2._UncompressedSize) == uint.MaxValue)) || (((uint)entry2._RelativeOffsetOfLocalHeader) == uint.MaxValue);
                num2 += entry2.ProcessExtraField(entry2._extraFieldLength);
                entry2._CompressedFileDataSize = entry2._CompressedSize;
            }
            if (entry2._Encryption == EncryptionAlgorithm.PkzipWeak)
            {
                entry2._CompressedFileDataSize -= 12L;
            }
            else if ((entry2.Encryption == EncryptionAlgorithm.WinZipAes128) || (entry2.Encryption == EncryptionAlgorithm.WinZipAes256))
            {
                entry2._CompressedFileDataSize = entry2.CompressedSize - ((((entry2._KeyStrengthInBits / 8) / 2) + 10) + 2);
                entry2._LengthOfTrailer = 10;
            }
            if (entry2._commentLength > 0)
            {
                buffer = new byte[entry2._commentLength];
                num3 = s.Read(buffer, 0, buffer.Length);
                num2 += num3;
                if ((entry2._BitField & 0x800) == 0x800)
                {
                    entry2._Comment = SharedUtilities.Utf8StringFromBuffer(buffer, buffer.Length);
                }
                else
                {
                    entry2._Comment = SharedUtilities.StringFromBuffer(buffer, buffer.Length, expectedEncoding);
                }
            }
            entry2._LengthOfDirEntry = num2;
            return entry2;
        }

        private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
        {
            int num = 0;
            ze._RelativeOffsetOfLocalHeader = (int)ze.ArchiveStream.Position;
            int signature = SharedUtilities.ReadSignature(ze.ArchiveStream);
            num += 4;
            if (IsNotValidSig(signature))
            {
                ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
                if (IsNotValidZipDirEntrySig(signature) && (signature != 0x6054b50L))
                {
                    throw new BadReadException(string.Format("  ZipEntry::ReadHeader(): Bad signature (0x{0:X8}) at position  0x{1:X8}", signature, ze.ArchiveStream.Position));
                }
                return false;
            }
            byte[] buffer = new byte[0x1a];
            int num3 = ze.ArchiveStream.Read(buffer, 0, buffer.Length);
            if (num3 != buffer.Length)
            {
                return false;
            }
            num += num3;
            int startIndex = 0;
            ze._VersionNeeded = (short)(buffer[startIndex++] + (buffer[startIndex++] * 0x100));
            ze._BitField = (short)(buffer[startIndex++] + (buffer[startIndex++] * 0x100));
            ze._CompressionMethod = (short)(buffer[startIndex++] + (buffer[startIndex++] * 0x100));
            ze._TimeBlob = ((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100);
            ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
            ze._Crc32 = ((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100);
            ze._CompressedSize = (long)((ulong)(((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100)));
            ze._UncompressedSize = (long)((ulong)(((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100)));
            if ((((uint)ze._CompressedSize) == uint.MaxValue) || (((uint)ze._UncompressedSize) == uint.MaxValue))
            {
                ze._IsZip64Format = true;
            }
            short num5 = (short)(buffer[startIndex++] + (buffer[startIndex++] * 0x100));
            short extraFieldLength = (short)(buffer[startIndex++] + (buffer[startIndex++] * 0x100));
            buffer = new byte[num5];
            num3 = ze.ArchiveStream.Read(buffer, 0, buffer.Length);
            num += num3;
            ze._actualEncoding = ((ze._BitField & 0x800) == 0x800) ? Encoding.UTF8 : defaultEncoding;
            ze._FileNameInArchive = ze._actualEncoding.GetString(buffer, 0, buffer.Length);
            ze._LocalFileName = ze._FileNameInArchive;
            if (ze._LocalFileName.EndsWith("/"))
            {
                ze.MarkAsDirectory();
            }
            num += ze.ProcessExtraField(extraFieldLength);
            if (!ze._LocalFileName.EndsWith("/") && ((ze._BitField & 8) == 8))
            {
                long position = ze.ArchiveStream.Position;
                bool flag3 = true;
                long num8 = 0L;
                int num9 = 0;
                while (flag3)
                {
                    num9++;
                    ze._zipfile.OnReadBytes(ze);
                    long num10 = SharedUtilities.FindSignature(ze.ArchiveStream, 0x8074b50);
                    if (num10 == -1L)
                    {
                        return false;
                    }
                    num8 += num10;
                    if (ze._IsZip64Format)
                    {
                        buffer = new byte[20];
                        if (ze.ArchiveStream.Read(buffer, 0, buffer.Length) != 20)
                        {
                            return false;
                        }
                        startIndex = 0;
                        ze._Crc32 = ((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100);
                        ze._CompressedSize = BitConverter.ToInt64(buffer, startIndex);
                        startIndex += 8;
                        ze._UncompressedSize = BitConverter.ToInt64(buffer, startIndex);
                        startIndex += 8;
                    }
                    else
                    {
                        buffer = new byte[12];
                        if (ze.ArchiveStream.Read(buffer, 0, buffer.Length) != 12)
                        {
                            return false;
                        }
                        startIndex = 0;
                        ze._Crc32 = ((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100);
                        ze._CompressedSize = (long)((ulong)(((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100)));
                        ze._UncompressedSize = (long)((ulong)(((buffer[startIndex++] + (buffer[startIndex++] * 0x100)) + ((buffer[startIndex++] * 0x100) * 0x100)) + (((buffer[startIndex++] * 0x100) * 0x100) * 0x100)));
                    }
                    if (num8 != ze._CompressedSize)
                    {
                        ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
                        num8 += 4L;
                    }
                }
                ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
            }
            ze._CompressedFileDataSize = ze._CompressedSize;
            if ((ze._BitField & 1) == 1)
            {
                if ((ze.Encryption == EncryptionAlgorithm.WinZipAes128) || (ze.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    ze._aesCrypto = WinZipAesCrypto.ReadFromStream(null, ze._KeyStrengthInBits, ze.ArchiveStream);
                    num += ze._aesCrypto.SizeOfEncryptionMetadata - 10;
                    ze._CompressedFileDataSize = ze.CompressedSize - ze._aesCrypto.SizeOfEncryptionMetadata;
                    ze._LengthOfTrailer = 10;
                }
                else
                {
                    ze._WeakEncryptionHeader = new byte[12];
                    num += ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
                    ze._CompressedFileDataSize -= 12L;
                }
            }
            ze._LengthOfHeader = num;
            ze._TotalEntrySize = (ze._LengthOfHeader + ze._CompressedFileDataSize) + ze._LengthOfTrailer;
            return true;
        }

        internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
        {
            int num = s.Read(buffer, 0, 12);
            if (num != 12)
            {
                throw new ZipException(string.Format("Unexpected end of data at position 0x{0:X8}", s.Position));
            }
            return num;
        }

        internal void ResetDirEntry()
        {
            this.__FileDataPosition = -1L;
            this._LengthOfHeader = 0;
        }

        private bool SeemsCompressible(string filename)
        {
            string pattern = @"(?i)^(.+)\.(mp3|png|docx|xlsx|jpg|zip)$";
            if (Regex.IsMatch(filename, pattern))
            {
                return false;
            }
            return true;
        }

        private void SetFdpLoh()
        {
            long position = this.ArchiveStream.Position;
            this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
            byte[] buffer = new byte[30];
            this.ArchiveStream.Read(buffer, 0, buffer.Length);
            short num2 = (short)(buffer[0x1a] + (buffer[0x1b] * 0x100));
            short num3 = (short)(buffer[0x1c] + (buffer[0x1d] * 0x100));
            this.ArchiveStream.Seek((long)(num2 + num3), SeekOrigin.Current);
            this._LengthOfHeader = (30 + num3) + num2;
            this.__FileDataPosition = ((this._RelativeOffsetOfLocalHeader + 30L) + num2) + num3;
            if (this._Encryption == EncryptionAlgorithm.PkzipWeak)
            {
                this.__FileDataPosition += 12L;
            }
            else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
            {
                this.__FileDataPosition += ((this._KeyStrengthInBits / 8) / 2) + 2;
            }
            this.ArchiveStream.Seek(position, SeekOrigin.Begin);
        }

        private void SetupCrypto(string password)
        {
            if (password != null)
            {
                if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
                {
                    this.ArchiveStream.Seek(this.FileDataPosition - 12L, SeekOrigin.Begin);
                    this._zipCrypto = ZipCrypto.ForRead(password, this);
                }
                else if ((this.Encryption == EncryptionAlgorithm.WinZipAes128) || (this.Encryption == EncryptionAlgorithm.WinZipAes256))
                {
                    if (this._aesCrypto != null)
                    {
                        this._aesCrypto.Password = password;
                    }
                    else
                    {
                        int num = ((this._KeyStrengthInBits / 8) / 2) + 2;
                        this.ArchiveStream.Seek(this.FileDataPosition - num, SeekOrigin.Begin);
                        this._aesCrypto = WinZipAesCrypto.ReadFromStream(password, this._KeyStrengthInBits, this.ArchiveStream);
                    }
                }
            }
        }

        private void ValidateCompression()
        {
            if ((this.CompressionMethod != 0) && (this.CompressionMethod != 8))
            {
                throw new ArgumentException(string.Format("Unsupported Compression method (0x{0:X2})", this.CompressionMethod));
            }
        }

        private void ValidateEncryption()
        {
            if ((((this.Encryption != EncryptionAlgorithm.PkzipWeak) && (this.Encryption != EncryptionAlgorithm.WinZipAes128)) && (this.Encryption != EncryptionAlgorithm.WinZipAes256)) && (this.Encryption != EncryptionAlgorithm.None))
            {
                throw new ArgumentException(string.Format("Unsupported Encryption algorithm ({0:X2})", this.Encryption));
            }
        }

        private bool ValidateOutput(string basedir, Stream outstream, out string OutputFile)
        {
            if (basedir != null)
            {
                OutputFile = this.FileName.StartsWith("/") ? Path.Combine(basedir, this.FileName.Substring(1)) : Path.Combine(basedir, this.FileName);
                if (this.IsDirectory || this.FileName.EndsWith("/"))
                {
                    if (!Directory.Exists(OutputFile))
                    {
                        Directory.CreateDirectory(OutputFile);
                    }
                    return true;
                }
                return false;
            }
            if (outstream == null)
            {
                throw new ZipException("Cannot extract.", new ArgumentException("Invalid input.", "outstream | basedir"));
            }
            OutputFile = null;
            return (this.IsDirectory || this.FileName.EndsWith("/"));
        }

        private bool WantReadAgain()
        {
            if (this._CompressionMethod == 0)
            {
                return false;
            }
            if (this._CompressedSize < this._UncompressedSize)
            {
                return false;
            }
            if (this.ForceNoCompression)
            {
                return false;
            }
            if (this.WillReadTwiceOnInflation != null)
            {
                return this.WillReadTwiceOnInflation(this._UncompressedSize, this._CompressedSize, this.FileName);
            }
            return true;
        }

        internal void Write(Stream outstream)
        {
            if (this._Source == EntrySource.Zipfile)
            {
                this.CopyThroughOneEntry(outstream);
            }
            else
            {
                bool flag2 = true;
                int cycle = 0;
                do
                {
                    cycle++;
                    this.WriteHeader(outstream, cycle);
                    if (this.IsDirectory)
                    {
                        break;
                    }
                    this._EmitOne(outstream);
                    if (flag2)
                    {
                        if (cycle > 1)
                        {
                            flag2 = false;
                        }
                        else if (!outstream.CanSeek)
                        {
                            flag2 = false;
                        }
                        else if ((this._aesCrypto != null) && ((this.CompressedSize - this._aesCrypto.SizeOfEncryptionMetadata) <= this.UncompressedSize))
                        {
                            flag2 = false;
                        }
                        else if ((this._zipCrypto != null) && ((this.CompressedSize - 12L) <= this.UncompressedSize))
                        {
                            flag2 = false;
                        }
                        else
                        {
                            flag2 = this.WantReadAgain();
                        }
                    }
                    if (flag2)
                    {
                        outstream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
                        outstream.SetLength(outstream.Position);
                        CountingStream stream = outstream as CountingStream;
                        if (stream != null)
                        {
                            stream.Adjust(this._TotalEntrySize);
                        }
                    }
                }
                while (flag2);
            }
        }

        internal void WriteCentralDirectoryEntry(Stream s)
        {
            byte[] buffer = new byte[0x1000];
            int count = 0;
            buffer[count++] = 80;
            buffer[count++] = 0x4b;
            buffer[count++] = 1;
            buffer[count++] = 2;
            buffer[count++] = this._EntryHeader[4];
            buffer[count++] = this._EntryHeader[5];
            int index = 0;
            index = 0;
            while (index < 0x1a)
            {
                buffer[count + index] = this._EntryHeader[4 + index];
                index++;
            }
            count += index;
            int num3 = (this._CommentBytes == null) ? 0 : this._CommentBytes.Length;
            if ((num3 + count) > buffer.Length)
            {
                num3 = buffer.Length - count;
            }
            buffer[count++] = (byte)(num3 & 0xff);
            buffer[count++] = (byte)((num3 & 0xff00) >> 8);
            buffer[count++] = 0;
            buffer[count++] = 0;
            buffer[count++] = 0;
            buffer[count++] = 0;
            buffer[count++] = this.IsDirectory ? ((byte)0x10) : ((byte)0x20);
            buffer[count++] = 0;
            buffer[count++] = 0xb6;
            buffer[count++] = 0x81;
            if (this._IsZip64Format)
            {
                for (index = 0; index < 4; index++)
                {
                    buffer[count++] = 0xff;
                }
            }
            else
            {
                buffer[count++] = (byte)(this._RelativeOffsetOfLocalHeader & 0xffL);
                buffer[count++] = (byte)((this._RelativeOffsetOfLocalHeader & 0xff00L) >> 8);
                buffer[count++] = (byte)((this._RelativeOffsetOfLocalHeader & 0xff0000L) >> 0x10);
                buffer[count++] = (byte)((this._RelativeOffsetOfLocalHeader & 0xff000000L) >> 0x18);
            }
            short num4 = (short)(this._EntryHeader[0x1a] + (this._EntryHeader[0x1b] * 0x100));
            index = 0;
            while (index < num4)
            {
                buffer[count + index] = this._EntryHeader[30 + index];
                index++;
            }
            count += index;
            short num5 = (short)(this._EntryHeader[0x1c] + (this._EntryHeader[0x1d] * 0x100));
            if (this._Extra != null)
            {
                index = 0;
                while (index < num5)
                {
                    buffer[count + index] = this._EntryHeader[(30 + num4) + index];
                    index++;
                }
                count += index;
            }
            if (num3 != 0)
            {
                index = 0;
                while ((index < num3) && ((count + index) < buffer.Length))
                {
                    buffer[count + index] = this._CommentBytes[index];
                    index++;
                }
                count += index;
            }
            s.Write(buffer, 0, count);
        }

        private void WriteHeader(Stream s, int cycle)
        {
            byte[] buffer2;
            int index = 0;
            CountingStream stream = s as CountingStream;
            this._RelativeOffsetOfLocalHeader = (stream != null) ? stream.BytesWritten : s.Position;
            byte[] buffer = new byte[0x200];
            int count = 0;
            buffer[count++] = 80;
            buffer[count++] = 0x4b;
            buffer[count++] = 3;
            buffer[count++] = 4;
            if ((this._zipfile._zip64 == Zip64Option.Default) && (((uint)this._RelativeOffsetOfLocalHeader) >= uint.MaxValue))
            {
                throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
            }
            this._presumeZip64 = (this._zipfile._zip64 == Zip64Option.Always) || ((this._zipfile._zip64 == Zip64Option.AsNecessary) && !s.CanSeek);
            short num3 = this._presumeZip64 ? ((short)0x2d) : ((short)20);
            buffer[count++] = (byte)(num3 & 0xff);
            buffer[count++] = (byte)((num3 & 0xff00) >> 8);
            this.GetEncodedBytes(out buffer2);
            short length = (short)buffer2.Length;
            bool flag2 = this.ActualEncoding.CodePage == Encoding.UTF8.CodePage;
            this._BitField = this.UsesEncryption ? ((short)1) : ((short)0);
            if (this.UsesEncryption && IsStrong(this.Encryption))
            {
                this._BitField = (short)(this._BitField | 0x20);
            }
            if (flag2)
            {
                this._BitField = (short)(this._BitField | 0x800);
            }
            if (!s.CanSeek)
            {
                this._BitField = (short)(this._BitField | 8);
            }
            buffer[count++] = (byte)(this._BitField & 0xff);
            buffer[count++] = (byte)((this._BitField & 0xff00) >> 8);
            if (this.__FileDataPosition == -1L)
            {
                this._UncompressedSize = 0L;
                this._CompressedSize = 0L;
                this._Crc32 = 0;
                this._crcCalculated = false;
            }
            this.FigureCompressionMethodForWriting(cycle);
            buffer[count++] = (byte)(this.CompressionMethod & 0xff);
            buffer[count++] = (byte)((this.CompressionMethod & 0xff00) >> 8);
            this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
            buffer[count++] = (byte)(this._TimeBlob & 0xff);
            buffer[count++] = (byte)((this._TimeBlob & 0xff00) >> 8);
            buffer[count++] = (byte)((this._TimeBlob & 0xff0000) >> 0x10);
            buffer[count++] = (byte)((this._TimeBlob & 0xff000000L) >> 0x18);
            buffer[count++] = (byte)(this._Crc32 & 0xff);
            buffer[count++] = (byte)((this._Crc32 & 0xff00) >> 8);
            buffer[count++] = (byte)((this._Crc32 & 0xff0000) >> 0x10);
            buffer[count++] = (byte)((this._Crc32 & 0xff000000L) >> 0x18);
            if (this._presumeZip64)
            {
                for (index = 0; index < 8; index++)
                {
                    buffer[count++] = 0xff;
                }
            }
            else
            {
                buffer[count++] = (byte)(this._CompressedSize & 0xffL);
                buffer[count++] = (byte)((this._CompressedSize & 0xff00L) >> 8);
                buffer[count++] = (byte)((this._CompressedSize & 0xff0000L) >> 0x10);
                buffer[count++] = (byte)((this._CompressedSize & 0xff000000L) >> 0x18);
                buffer[count++] = (byte)(this._UncompressedSize & 0xffL);
                buffer[count++] = (byte)((this._UncompressedSize & 0xff00L) >> 8);
                buffer[count++] = (byte)((this._UncompressedSize & 0xff0000L) >> 0x10);
                buffer[count++] = (byte)((this._UncompressedSize & 0xff000000L) >> 0x18);
            }
            buffer[count++] = (byte)(length & 0xff);
            buffer[count++] = (byte)((length & 0xff00) >> 8);
            this._Extra = this.ConsExtraField();
            short num5 = (this._Extra == null) ? ((short)0) : ((short)this._Extra.Length);
            buffer[count++] = (byte)(num5 & 0xff);
            buffer[count++] = (byte)((num5 & 0xff00) >> 8);
            index = 0;
            while ((index < buffer2.Length) && ((count + index) < buffer.Length))
            {
                buffer[count + index] = buffer2[index];
                index++;
            }
            count += index;
            if (this._Extra != null)
            {
                index = 0;
                while (index < this._Extra.Length)
                {
                    buffer[count + index] = this._Extra[index];
                    index++;
                }
                count += index;
            }
            this._LengthOfHeader = count;
            s.Write(buffer, 0, count);
            this._EntryHeader = new byte[count];
            for (index = 0; index < count; index++)
            {
                this._EntryHeader[index] = buffer[index];
            }
        }

        public Encoding ActualEncoding
        {
            get
            {
                return this._actualEncoding;
            }
        }

        internal Stream ArchiveStream
        {
            get
            {
                if ((this._archiveStream == null) && (this._zipfile != null))
                {
                    this._zipfile.Reset();
                    this._archiveStream = this._zipfile.ReadStream;
                }
                return this._archiveStream;
            }
        }

        internal bool AttributesIndicateDirectory
        {
            get
            {
                return ((this._InternalFileAttrs == 0) && ((this._ExternalFileAttrs & 0x10) == 0x10));
            }
        }

        public short BitField
        {
            get
            {
                return this._BitField;
            }
        }

        public string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this._Comment = value;
                this._metadataChanged = true;
            }
        }

        public long CompressedSize
        {
            get
            {
                return this._CompressedSize;
            }
        }

        public short CompressionMethod
        {
            get
            {
                return this._CompressionMethod;
            }
            set
            {
                if ((value != 0) && (value != 8))
                {
                    throw new InvalidOperationException("Unsupported compression method. Specify 8 or 0.");
                }
                this._CompressionMethod = value;
                this._ForceNoCompression = this._CompressionMethod == 0;
            }
        }

        public double CompressionRatio
        {
            get
            {
                if (this.UncompressedSize == 0L)
                {
                    return 0.0;
                }
                return (100.0 * (1.0 - ((1.0 * this.CompressedSize) / (1.0 * this.UncompressedSize))));
            }
        }

        public int Crc32
        {
            get
            {
                return this._Crc32;
            }
        }

        public EncryptionAlgorithm Encryption
        {
            get
            {
                return this._Encryption;
            }
            set
            {
                this._Encryption = value;
                if (value == EncryptionAlgorithm.WinZipAes256)
                {
                    this._KeyStrengthInBits = 0x100;
                }
                else if (value == EncryptionAlgorithm.WinZipAes128)
                {
                    this._KeyStrengthInBits = 0x80;
                }
            }
        }

        internal long FileDataPosition
        {
            get
            {
                if (this.__FileDataPosition == -1L)
                {
                    this.SetFdpLoh();
                }
                return this.__FileDataPosition;
            }
        }

        public string FileName
        {
            get
            {
                return this._FileNameInArchive;
            }
            set
            {
                if ((value == null) || (value == ""))
                {
                    throw new ZipException("The FileName must be non empty and non-null.");
                }
                string str = NameInArchive(value, null);
                this._FileNameInArchive = value;
                if (this._zipfile != null)
                {
                    this._zipfile.NotifyEntryChanged();
                }
                this._metadataChanged = true;
            }
        }

        public bool ForceNoCompression
        {
            get
            {
                return this._ForceNoCompression;
            }
            set
            {
                this._ForceNoCompression = value;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return this._IsDirectory;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return this._LastModified;
            }
            set
            {
                this._LastModified = value;
            }
        }

        private int LengthOfHeader
        {
            get
            {
                if (this._LengthOfHeader == 0)
                {
                    this.SetFdpLoh();
                }
                return this._LengthOfHeader;
            }
        }

        public string LocalFileName
        {
            get
            {
                return this._LocalFileName;
            }
        }

        public bool OverwriteOnExtract
        {
            get
            {
                return this._OverwriteOnExtract;
            }
            set
            {
                this._OverwriteOnExtract = value;
            }
        }

        public string Password
        {
            set
            {
                this._Password = value;
                if (this._Password == null)
                {
                    this._Encryption = EncryptionAlgorithm.None;
                }
                else if (this.Encryption == EncryptionAlgorithm.None)
                {
                    this._Encryption = EncryptionAlgorithm.PkzipWeak;
                }
            }
        }

        public Encoding ProvisionalAlternateEncoding
        {
            get
            {
                return this._provisionalAlternateEncoding;
            }
            set
            {
                this._provisionalAlternateEncoding = value;
            }
        }

        public long UncompressedSize
        {
            get
            {
                return this._UncompressedSize;
            }
        }

        public bool UsesEncryption
        {
            get
            {
                return (this.Encryption != EncryptionAlgorithm.None);
            }
        }

        public bool UseUnicodeAsNecessary
        {
            get
            {
                return (this._provisionalAlternateEncoding == Encoding.GetEncoding("UTF-8"));
            }
            set
            {
                this._provisionalAlternateEncoding = value ? Encoding.GetEncoding("UTF-8") : ZipFile.DefaultEncoding;
            }
        }

        public short VersionNeeded
        {
            get
            {
                return this._VersionNeeded;
            }
        }

        public WantCompressionCallback WantCompression
        {
            [CompilerGenerated]
            get
            {
                return this.WantCompression;
            }
            [CompilerGenerated]
            set
            {
                this.WantCompression = value;
            }
        }

        public ReReadApprovalCallback WillReadTwiceOnInflation
        {
            [CompilerGenerated]
            get
            {
                return this.WillReadTwiceOnInflation;
            }
            [CompilerGenerated]
            set
            {
                this.WillReadTwiceOnInflation = value;
            }
        }
    }
}

