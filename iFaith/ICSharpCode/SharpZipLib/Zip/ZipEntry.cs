namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class ZipEntry : ICloneable
    {
        private string comment;
        private uint compressedSize;
        private uint crc;
        private uint dosTime;
        private byte[] extra;
        public int flags;
        private bool isCrypted;
        private ushort known;
        private static int KNOWN_CRC = 4;
        private static int KNOWN_CSIZE = 2;
        private static int KNOWN_SIZE = 1;
        private static int KNOWN_TIME = 8;
        private ICSharpCode.SharpZipLib.Zip.CompressionMethod method;
        private string name;
        public int offset;
        private uint size;
        private ushort version;
        public int zipFileIndex;

        public ZipEntry(ZipEntry e)
        {
            this.known = 0;
            this.method = ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated;
            this.extra = null;
            this.comment = null;
            this.zipFileIndex = -1;
            this.name = e.name;
            this.known = e.known;
            this.size = e.size;
            this.compressedSize = e.compressedSize;
            this.crc = e.crc;
            this.dosTime = e.dosTime;
            this.method = e.method;
            this.extra = e.extra;
            this.comment = e.comment;
        }

        public ZipEntry(string name)
        {
            this.known = 0;
            this.method = ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated;
            this.extra = null;
            this.comment = null;
            this.zipFileIndex = -1;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.DateTime = System.DateTime.Now;
            this.name = name;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.name;
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                if (value.Length > 65535)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.comment = value;
            }
        }

        public long CompressedSize
        {
            get
            {
                if ((this.known & KNOWN_CSIZE) == 0)
                {
                    return -1L;
                }
                return (long) this.compressedSize;
            }
            set
            {
                if ((value & -4294967296L) != 0L)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.compressedSize = (uint) value;
                this.known = (ushort) (this.known | ((ushort) KNOWN_CSIZE));
            }
        }

        public ICSharpCode.SharpZipLib.Zip.CompressionMethod CompressionMethod
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }

        public long Crc
        {
            get
            {
                if ((this.known & KNOWN_CRC) == 0)
                {
                    return -1L;
                }
                return (long) (this.crc & 0xffffffffL);
            }
            set
            {
                if ((this.crc & 18446744069414584320L) != 0L)
                {
                    throw new Exception();
                }
                this.crc = (uint) value;
                this.known = (ushort) (this.known | ((ushort) KNOWN_CRC));
            }
        }

        public System.DateTime DateTime
        {
            get
            {
                uint num = 2 * (this.dosTime & 0x1f);
                uint num2 = (this.dosTime >> 5) & 0x3f;
                uint num3 = (this.dosTime >> 11) & 0x1f;
                uint num4 = (this.dosTime >> 0x10) & 0x1f;
                uint num5 = (this.dosTime >> 0x15) & 15;
                uint num6 = ((this.dosTime >> 0x19) & 0x7f) + 0x7bc;
                return new System.DateTime((int) num6, (int) num5, (int) num4, (int) num3, (int) num2, (int) num);
            }
            set
            {
                this.DosTime = (uint) ((((((((value.Year - 0x7bc) & 0x7f) << 0x19) | (value.Month << 0x15)) | (value.Day << 0x10)) | (value.Hour << 11)) | (value.Minute << 5)) | (value.Second >> 1));
            }
        }

        public uint DosTime
        {
            get
            {
                if ((this.known & KNOWN_TIME) == 0)
                {
                    return 0;
                }
                return this.dosTime;
            }
            set
            {
                this.dosTime = value;
                this.known = (ushort) (this.known | ((ushort) KNOWN_TIME));
            }
        }

        public byte[] ExtraData
        {
            get
            {
                return this.extra;
            }
            set
            {
                if (value == null)
                {
                    this.extra = null;
                }
                else
                {
                    if (value.Length > 0xffff)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    this.extra = value;
                    try
                    {
                        int num3;
                        for (int i = 0; i < this.extra.Length; i += num3)
                        {
                            int num2 = (this.extra[i++] & 0xff) | ((this.extra[i++] & 0xff) << 8);
                            num3 = (this.extra[i++] & 0xff) | ((this.extra[i++] & 0xff) << 8);
                            if (num2 == 0x5455)
                            {
                                int num4 = this.extra[i];
                                if ((num4 & 1) != 0)
                                {
                                    int seconds = (((this.extra[i + 1] & 0xff) | ((this.extra[i + 2] & 0xff) << 8)) | ((this.extra[i + 3] & 0xff) << 0x10)) | ((this.extra[i + 4] & 0xff) << 0x18);
                                    this.DateTime = (new System.DateTime(0x7b2, 1, 1, 0, 0, 0) + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
                                    this.known = (ushort) (this.known | ((ushort) KNOWN_TIME));
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public bool IsCrypted
        {
            get
            {
                return this.isCrypted;
            }
            set
            {
                this.isCrypted = value;
            }
        }

        public bool IsDirectory
        {
            get
            {
                int length = this.name.Length;
                return ((length > 0) && (this.name[length - 1] == '/'));
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public long Size
        {
            get
            {
                if ((this.known & KNOWN_SIZE) == 0)
                {
                    return -1L;
                }
                return (long) this.size;
            }
            set
            {
                if ((value & -4294967296L) != 0L)
                {
                    throw new ArgumentOutOfRangeException("size");
                }
                this.size = (uint) value;
                this.known = (ushort) (this.known | ((ushort) KNOWN_SIZE));
            }
        }

        public ushort Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
    }
}

