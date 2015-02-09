namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;
    using System.Text;

    public class TarEntry
    {
        private static readonly DateTime datetTime1970 = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
        protected string file;
        protected ICSharpCode.SharpZipLib.Tar.TarHeader header;
        private static readonly long timeConversionFactor = 0x989680L;

        private TarEntry()
        {
        }

        public TarEntry(byte[] headerBuf)
        {
            this.Initialize();
            this.ParseTarHeader(this.header, headerBuf);
        }

        public void AdjustEntryName(byte[] outbuf, string newName)
        {
            int offset = 0;
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(new StringBuilder(newName), outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN);
        }

        public long ComputeCheckSum(byte[] buf)
        {
            long num = 0L;
            for (int i = 0; i < buf.Length; i++)
            {
                num += 0xff & buf[i];
            }
            return num;
        }

        public static TarEntry CreateEntryFromFile(string fileName)
        {
            TarEntry entry = new TarEntry();
            entry.Initialize();
            entry.GetFileTarHeader(entry.header, fileName);
            return entry;
        }

        public static TarEntry CreateTarEntry(string name)
        {
            TarEntry entry = new TarEntry();
            entry.Initialize();
            entry.NameTarHeader(entry.header, name);
            return entry;
        }

        public override bool Equals(object it)
        {
            return ((it is TarEntry) && this.header.name.ToString().Equals(((TarEntry) it).header.name.ToString()));
        }

        private static int GetCTime(DateTime dateTime)
        {
            return (int) ((dateTime.Ticks - datetTime1970.Ticks) / timeConversionFactor);
        }

        private static DateTime GetDateTimeFromCTime(long ticks)
        {
            return new DateTime(datetTime1970.Ticks + (ticks * timeConversionFactor));
        }

        public TarEntry[] GetDirectoryEntries()
        {
            if ((this.file == null) || !Directory.Exists(this.file))
            {
                return new TarEntry[0];
            }
            string[] fileSystemEntries = Directory.GetFileSystemEntries(this.file);
            TarEntry[] entryArray = new TarEntry[fileSystemEntries.Length];
            string file = this.file;
            if (!file.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                file = file + Path.DirectorySeparatorChar;
            }
            for (int i = 0; i < fileSystemEntries.Length; i++)
            {
                entryArray[i] = CreateEntryFromFile(fileSystemEntries[i]);
            }
            return entryArray;
        }

        public void GetFileTarHeader(ICSharpCode.SharpZipLib.Tar.TarHeader hdr, string file)
        {
            this.file = file;
            string str = file;
            if ((Path.DirectorySeparatorChar == '\\') && (str.Length > 2))
            {
                char c = str[0];
                char ch2 = str[1];
                if ((ch2 == ':') && char.IsLetter(c))
                {
                    str = str.Substring(2);
                }
            }
            str = str.Replace(Path.DirectorySeparatorChar, '/');
            while (str.StartsWith("/"))
            {
                str = str.Substring(1);
            }
            hdr.linkName = new StringBuilder(string.Empty);
            hdr.name = new StringBuilder(str);
            if (Directory.Exists(file))
            {
                hdr.mode = 0x9f33;
                hdr.linkFlag = ICSharpCode.SharpZipLib.Tar.TarHeader.LF_DIR;
                if ((hdr.name.Length == 0) || (hdr.name[hdr.name.Length - 1] != '/'))
                {
                    hdr.name.Append("/");
                }
                hdr.size = 0L;
            }
            else
            {
                hdr.mode = 0x18924;
                hdr.linkFlag = ICSharpCode.SharpZipLib.Tar.TarHeader.LF_NORMAL;
                hdr.size = new FileInfo(file.Replace('/', Path.DirectorySeparatorChar)).Length;
            }
            hdr.modTime = System.IO.File.GetLastAccessTime(file.Replace('/', Path.DirectorySeparatorChar));
            hdr.checkSum = 0;
            hdr.devMajor = 0;
            hdr.devMinor = 0;
        }

        public override int GetHashCode()
        {
            return this.header.name.ToString().GetHashCode();
        }

        private void Initialize()
        {
            this.file = null;
            this.header = new ICSharpCode.SharpZipLib.Tar.TarHeader();
        }

        public bool IsDescendent(TarEntry desc)
        {
            return desc.header.name.ToString().StartsWith(this.header.name.ToString());
        }

        public void NameTarHeader(ICSharpCode.SharpZipLib.Tar.TarHeader hdr, string name)
        {
            bool flag = name.EndsWith("/");
            hdr.checkSum = 0;
            hdr.devMajor = 0;
            hdr.devMinor = 0;
            hdr.name = new StringBuilder(name);
            hdr.mode = flag ? 0x9f33 : 0x18924;
            hdr.userId = 0;
            hdr.groupId = 0;
            hdr.size = 0L;
            hdr.checkSum = 0;
            hdr.modTime = DateTime.Now;
            hdr.linkFlag = flag ? ICSharpCode.SharpZipLib.Tar.TarHeader.LF_DIR : ICSharpCode.SharpZipLib.Tar.TarHeader.LF_NORMAL;
            hdr.linkName = new StringBuilder(string.Empty);
            hdr.userName = new StringBuilder(string.Empty);
            hdr.groupName = new StringBuilder(string.Empty);
            hdr.devMajor = 0;
            hdr.devMinor = 0;
        }

        public void ParseTarHeader(ICSharpCode.SharpZipLib.Tar.TarHeader hdr, byte[] header)
        {
            int offset = 0;
            hdr.name = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN;
            hdr.mode = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MODELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.MODELEN;
            hdr.userId = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.UIDLEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.UIDLEN;
            hdr.groupId = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.GIDLEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.GIDLEN;
            hdr.size = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.SIZELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.SIZELEN;
            hdr.modTime = GetDateTimeFromCTime(ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MODTIMELEN));
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.MODTIMELEN;
            hdr.checkSum = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.CHKSUMLEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.CHKSUMLEN;
            hdr.linkFlag = header[offset++];
            hdr.linkName = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN;
            hdr.magic = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MAGICLEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.MAGICLEN;
            hdr.userName = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.UNAMELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.UNAMELEN;
            hdr.groupName = ICSharpCode.SharpZipLib.Tar.TarHeader.ParseName(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.GNAMELEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.GNAMELEN;
            hdr.devMajor = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.DEVLEN);
            offset += ICSharpCode.SharpZipLib.Tar.TarHeader.DEVLEN;
            hdr.devMinor = (int) ICSharpCode.SharpZipLib.Tar.TarHeader.ParseOctal(header, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.DEVLEN);
        }

        public void SetIds(int userId, int groupId)
        {
            this.UserId = userId;
            this.GroupId = groupId;
        }

        public void SetNames(string userName, string groupName)
        {
            this.UserName = userName;
            this.GroupName = groupName;
        }

        public void WriteEntryHeader(byte[] outbuf)
        {
            int offset = 0;
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(this.header.name, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetOctalBytes((long) this.header.mode, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MODELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetOctalBytes((long) this.header.userId, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.UIDLEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetOctalBytes((long) this.header.groupId, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.GIDLEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetLongOctalBytes(this.header.size, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.SIZELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetLongOctalBytes((long) GetCTime(this.header.modTime), outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MODTIMELEN);
            int num3 = offset;
            for (int i = 0; i < ICSharpCode.SharpZipLib.Tar.TarHeader.CHKSUMLEN; i++)
            {
                outbuf[offset++] = 0x20;
            }
            outbuf[offset++] = this.header.linkFlag;
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(this.header.linkName, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.NAMELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(this.header.magic, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.MAGICLEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(this.header.userName, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.UNAMELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(this.header.groupName, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.GNAMELEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetOctalBytes((long) this.header.devMajor, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.DEVLEN);
            offset = ICSharpCode.SharpZipLib.Tar.TarHeader.GetOctalBytes((long) this.header.devMinor, outbuf, offset, ICSharpCode.SharpZipLib.Tar.TarHeader.DEVLEN);
            while (offset < outbuf.Length)
            {
                outbuf[offset++] = 0;
            }
            ICSharpCode.SharpZipLib.Tar.TarHeader.GetCheckSumOctalBytes(this.ComputeCheckSum(outbuf), outbuf, num3, ICSharpCode.SharpZipLib.Tar.TarHeader.CHKSUMLEN);
        }

        public string File
        {
            get
            {
                return this.file;
            }
        }

        public int GroupId
        {
            get
            {
                return this.header.groupId;
            }
            set
            {
                this.header.groupId = value;
            }
        }

        public string GroupName
        {
            get
            {
                return this.header.groupName.ToString();
            }
            set
            {
                this.header.groupName = new StringBuilder(value);
            }
        }

        public bool IsDirectory
        {
            get
            {
                if (this.file != null)
                {
                    return Directory.Exists(this.file);
                }
                if ((this.header == null) || ((this.header.linkFlag != ICSharpCode.SharpZipLib.Tar.TarHeader.LF_DIR) && !this.header.name.ToString().EndsWith("/")))
                {
                    return false;
                }
                return true;
            }
        }

        public DateTime ModTime
        {
            get
            {
                return this.header.modTime;
            }
            set
            {
                this.header.modTime = value;
            }
        }

        public string Name
        {
            get
            {
                return this.header.name.ToString();
            }
            set
            {
                this.header.name = new StringBuilder(value);
            }
        }

        public long Size
        {
            get
            {
                return this.header.size;
            }
            set
            {
                this.header.size = value;
            }
        }

        public ICSharpCode.SharpZipLib.Tar.TarHeader TarHeader
        {
            get
            {
                return this.header;
            }
        }

        public int UserId
        {
            get
            {
                return this.header.userId;
            }
            set
            {
                this.header.userId = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.header.userName.ToString();
            }
            set
            {
                this.header.userName = new StringBuilder(value);
            }
        }
    }
}

