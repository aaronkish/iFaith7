namespace Ionic.Zlib
{
    using System;
    using System.IO;
    using System.Text;

    internal class SharedUtils
    {
        public static int ReadInput(Stream sourceStream, byte[] target, int start, int count)
        {
            if (target.Length == 0)
            {
                return 0;
            }
            if (count == 0)
            {
                return 0;
            }
            return sourceStream.Read(target, start, count);
        }

        public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
        {
            if (target.Length == 0)
            {
                return 0;
            }
            char[] buffer = new char[target.Length];
            int num2 = sourceTextReader.Read(buffer, start, count);
            if (num2 == 0)
            {
                return -1;
            }
            for (int i = start; i < (start + num2); i++)
            {
                target[i] = (byte) buffer[i];
            }
            return num2;
        }

        internal static byte[] ToByteArray(string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString);
        }

        internal static char[] ToCharArray(byte[] byteArray)
        {
            return Encoding.UTF8.GetChars(byteArray);
        }

        public static int URShift(int number, int bits)
        {
            if (number >= 0)
            {
                return (number >> bits);
            }
            return ((number >> bits) + (((int) 2) << ~bits));
        }

        public static int URShift(int number, long bits)
        {
            return URShift(number, (int) bits);
        }

        public static long URShift(long number, int bits)
        {
            if (number >= 0L)
            {
                return (number >> bits);
            }
            return ((number >> bits) + (((long) 2L) << ~bits));
        }

        public static long URShift(long number, long bits)
        {
            return URShift(number, (int) bits);
        }
    }
}

