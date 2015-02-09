namespace Ionic.Zlib
{
    using System;

    internal sealed class Adler
    {
        private static int BASE = 0xfff1;
        private static int NMAX = 0x15b0;

        internal static long Adler32(long adler, byte[] buf, int index, int len)
        {
            if (buf == null)
            {
                return 1L;
            }
            long num2 = adler & 0xffffL;
            long num3 = (adler >> 0x10) & 0xffffL;
            while (len > 0)
            {
                int num4 = (len < NMAX) ? len : NMAX;
                len -= num4;
                while (num4 >= 0x10)
                {
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num2 += buf[index++] & 0xff;
                    num3 += num2;
                    num4 -= 0x10;
                }
                if (num4 != 0)
                {
                    do
                    {
                        num2 += buf[index++] & 0xff;
                        num3 += num2;
                    }
                    while (--num4 != 0);
                }
                num2 = num2 % ((long) BASE);
                num3 = num3 % ((long) BASE);
            }
            return ((num3 << 0x10) | num2);
        }
    }
}

