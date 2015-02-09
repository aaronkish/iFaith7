namespace Ionic.Zip
{
    using System;
    using System.IO;
    using System.Text;

    internal class SharedUtilities
    {
        private static Random _rnd = new Random();
        private static Encoding ibm437 = Encoding.GetEncoding("IBM437");
        private static Encoding utf8 = Encoding.GetEncoding("UTF-8");

        private SharedUtilities()
        {
        }

        private static int _ReadFourBytes(Stream s, string message)
        {
            byte[] buffer = new byte[4];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new BadReadException(string.Format(message, s.Position));
            }
            return ((((((buffer[3] * 0x100) + buffer[2]) * 0x100) + buffer[1]) * 0x100) + buffer[0]);
        }

        internal static int DateTimeToPacked(DateTime time)
        {
            ushort num = (ushort) (((time.Day & 0x1f) | ((time.Month << 5) & 480)) | (((time.Year - 0x7bc) << 9) & 0xfe00));
            ushort num2 = (ushort) ((((time.Second / 2) & 0x1f) | ((time.Minute << 5) & 0x7e0)) | ((time.Hour << 11) & 0xf800));
            return ((num << 0x10) | num2);
        }

        protected internal static long FindSignature(Stream stream, int SignatureToFind)
        {
            long position = stream.Position;
            int num2 = 0x10000;
            byte[] buffer = new byte[] { (byte) (SignatureToFind >> 0x18), (byte) ((SignatureToFind & 0xff0000) >> 0x10), (byte) ((SignatureToFind & 0xff00) >> 8), (byte) (SignatureToFind & 0xff) };
            byte[] buffer2 = new byte[num2];
            int num3 = 0;
            bool flag = false;
        Label_0050:
            num3 = stream.Read(buffer2, 0, buffer2.Length);
            if (num3 != 0)
            {
                for (int i = 0; i < num3; i++)
                {
                    if (buffer2[i] == buffer[3])
                    {
                        long offset = stream.Position;
                        stream.Seek((long) (i - num3), SeekOrigin.Current);
                        flag = ReadSignature(stream) == SignatureToFind;
                        if (flag)
                        {
                            break;
                        }
                        stream.Seek(offset, SeekOrigin.Begin);
                    }
                }
                if (!flag)
                {
                    goto Label_0050;
                }
            }
            if (!flag)
            {
                stream.Seek(position, SeekOrigin.Begin);
                return -1L;
            }
            return ((stream.Position - position) - 4L);
        }

        private static string GenerateRandomStringImpl(int length, int delta)
        {
            bool flag = delta == 0;
            char[] chArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                if (flag)
                {
                    delta = (_rnd.Next(2) == 0) ? 0x41 : 0x61;
                }
                chArray[i] = GetOneRandomChar(delta);
            }
            return new string(chArray);
        }

        private static char GetOneRandomChar(int delta)
        {
            return (char) (_rnd.Next(0x1a) + delta);
        }

        public static string GetTempFilename()
        {
            string path = null;
            do
            {
                path = "DotNetZip-" + GenerateRandomStringImpl(8, 0x61) + ".tmp";
            }
            while (File.Exists(path));
            return path;
        }

        internal static DateTime PackedToDateTime(int packedDateTime)
        {
            short num = (short) (packedDateTime & 0xffff);
            short num2 = (short) ((packedDateTime & 0xffff0000L) >> 0x10);
            int year = 0x7bc + ((num2 & 0xfe00) >> 9);
            int month = (num2 & 480) >> 5;
            int day = num2 & 0x1f;
            int hour = (num & 0xf800) >> 11;
            int minute = (num & 0x7e0) >> 5;
            int second = (num & 0x1f) * 2;
            if (second >= 60)
            {
                minute++;
                second = 0;
            }
            if (minute >= 60)
            {
                hour++;
                minute = 0;
            }
            if (hour >= 0x18)
            {
                day++;
                hour = 0;
            }
            DateTime now = DateTime.Now;
            try
            {
                now = new DateTime(year, month, day, hour, minute, second, 0);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw new ZipException("Bad date/time format in the zip file.", exception);
            }
            return now;
        }

        internal static int ReadInt(Stream s)
        {
            return _ReadFourBytes(s, "Could not read block - no data!  (position 0x{0:X8})");
        }

        internal static int ReadSignature(Stream s)
        {
            return _ReadFourBytes(s, "Could not read signature - no data!  (position 0x{0:X8})");
        }

        public static DateTime RoundToEvenSecond(DateTime source)
        {
            if ((source.Second % 2) == 1)
            {
                source += new TimeSpan(0, 0, 1);
            }
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second);
        }

        internal static string StringFromBuffer(byte[] buf, int maxlength)
        {
            return StringFromBuffer(buf, maxlength, ibm437);
        }

        internal static string StringFromBuffer(byte[] buf, int maxlength, Encoding encoding)
        {
            return encoding.GetString(buf, 0, buf.Length);
        }

        internal static byte[] StringToByteArray(string value)
        {
            return StringToByteArray(value, ibm437);
        }

        internal static byte[] StringToByteArray(string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static MemoryStream StringToMemoryStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            return stream;
        }

        public static string TrimVolumeAndSwapSlashes(string pathName)
        {
            if (string.IsNullOrEmpty(pathName))
            {
                return pathName;
            }
            if (pathName.Length < 2)
            {
                return pathName.Replace('\\', '/');
            }
            return (((pathName[1] == ':') && (pathName[2] == '\\')) ? pathName.Substring(3) : pathName).Replace('\\', '/');
        }

        internal static string Utf8StringFromBuffer(byte[] buf, int maxlength)
        {
            return StringFromBuffer(buf, maxlength, utf8);
        }

        internal static byte[] Utf8StringToByteArray(string value)
        {
            return StringToByteArray(value, utf8);
        }
    }
}

