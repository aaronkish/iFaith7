namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.Text;

    public class TarHeader : ICloneable
    {
        public int checkSum;
        public static readonly int CHKSUMLEN = 8;
        public static readonly int DEVLEN = 8;
        public int devMajor;
        public int devMinor;
        public static readonly int GIDLEN = 8;
        public static readonly int GNAMELEN = 0x20;
        public static readonly string GNU_TMAGIC = "ustar  ";
        public int groupId;
        public StringBuilder groupName;
        public static readonly byte LF_BLK = 0x34;
        public static readonly byte LF_CHR = 0x33;
        public static readonly byte LF_CONTIG = 0x37;
        public static readonly byte LF_DIR = 0x35;
        public static readonly byte LF_FIFO = 0x36;
        public static readonly byte LF_LINK = 0x31;
        public static readonly byte LF_NORMAL = 0x30;
        public static readonly byte LF_OLDNORM = 0;
        public static readonly byte LF_SYMLINK = 50;
        public byte linkFlag;
        public StringBuilder linkName = new StringBuilder();
        public StringBuilder magic = new StringBuilder(TMAGIC);
        public static readonly int MAGICLEN = 8;
        public int mode;
        public static readonly int MODELEN = 8;
        public DateTime modTime;
        public static readonly int MODTIMELEN = 12;
        public StringBuilder name = new StringBuilder();
        public static readonly int NAMELEN = 100;
        public long size;
        public static readonly int SIZELEN = 12;
        public static readonly string TMAGIC = "ustar";
        public static readonly int UIDLEN = 8;
        public static readonly int UNAMELEN = 0x20;
        public int userId;
        public StringBuilder userName;

        public TarHeader()
        {
            string str = "PocketPC";
            if (str.Length > 0x1f)
            {
                str = str.Substring(0, 0x1f);
            }
            this.userId = 0;
            this.groupId = 0;
            this.userName = new StringBuilder(str);
            this.groupName = new StringBuilder(string.Empty);
        }

        public object Clone()
        {
            TarHeader header = new TarHeader();
            header.name = (this.name == null) ? null : new StringBuilder(this.name.ToString());
            header.mode = this.mode;
            header.userId = this.userId;
            header.groupId = this.groupId;
            header.size = this.size;
            header.modTime = this.modTime;
            header.checkSum = this.checkSum;
            header.linkFlag = this.linkFlag;
            header.linkName = (this.linkName == null) ? null : new StringBuilder(this.linkName.ToString());
            header.magic = (this.magic == null) ? null : new StringBuilder(this.magic.ToString());
            header.userName = (this.userName == null) ? null : new StringBuilder(this.userName.ToString());
            header.groupName = (this.groupName == null) ? null : new StringBuilder(this.groupName.ToString());
            header.devMajor = this.devMajor;
            header.devMinor = this.devMinor;
            return header;
        }

        public static int GetCheckSumOctalBytes(long val, byte[] buf, int offset, int length)
        {
            GetOctalBytes(val, buf, offset, length);
            buf[(offset + length) - 1] = 0x20;
            buf[(offset + length) - 2] = 0;
            return (offset + length);
        }

        public static int GetLongOctalBytes(long val, byte[] buf, int offset, int length)
        {
            byte[] buffer = new byte[length + 1];
            GetOctalBytes(val, buffer, 0, length + 1);
            Array.Copy(buffer, 0, buf, offset, length);
            return (offset + length);
        }

        public string GetName()
        {
            return this.name.ToString();
        }

        public static int GetNameBytes(StringBuilder name, byte[] buf, int offset, int length)
        {
            int num = 0;
            while ((num < length) && (num < name.Length))
            {
                buf[offset + num] = (byte) name[num];
                num++;
            }
            while (num < length)
            {
                buf[offset + num] = 0;
                num++;
            }
            return (offset + length);
        }

        public static int GetOctalBytes(long val, byte[] buf, int offset, int length)
        {
            byte[] buffer1 = new byte[length];
            int num = length - 1;
            buf[offset + num] = 0;
            num--;
            buf[offset + num] = 0x20;
            num--;
            if (val == 0L)
            {
                buf[offset + num] = 0x30;
                num--;
            }
            else
            {
                long num2 = val;
                while ((num >= 0) && (num2 > 0L))
                {
                    buf[offset + num] = (byte) (0x30 + ((byte) (num2 & 7L)));
                    num2 = num2 >> 3;
                    num--;
                }
            }
            while (num >= 0)
            {
                buf[offset + num] = 0x20;
                num--;
            }
            return (offset + length);
        }

        public static StringBuilder ParseName(byte[] header, int offset, int length)
        {
            StringBuilder builder = new StringBuilder(length);
            for (int i = offset; i < (offset + length); i++)
            {
                if (header[i] == 0)
                {
                    return builder;
                }
                builder.Append((char) header[i]);
            }
            return builder;
        }

        public static long ParseOctal(byte[] header, int offset, int length)
        {
            long num = 0L;
            bool flag = true;
            int num2 = offset + length;
            for (int i = offset; i < num2; i++)
            {
                if (header[i] == 0)
                {
                    return num;
                }
                if ((header[i] == 0x20) || (header[i] == 0x30))
                {
                    if (flag)
                    {
                        continue;
                    }
                    if (header[i] == 0x20)
                    {
                        return num;
                    }
                }
                flag = false;
                num = (num << 3) + (header[i] - 0x30);
            }
            return num;
        }
    }
}

