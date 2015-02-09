namespace Ionic.Zip
{
    using System;
    using System.Text;

    internal class Util
    {
        internal static string FormatByteArray(byte[] b)
        {
            int num = 0x60;
            StringBuilder builder = new StringBuilder(num * 2);
            int index = 0;
            if ((num * 2) > b.Length)
            {
                for (index = 0; index < b.Length; index++)
                {
                    if ((index != 0) && ((index % 0x10) == 0))
                    {
                        builder.Append("\n");
                    }
                    builder.Append(string.Format("{0:X2} ", b[index]));
                }
            }
            else
            {
                index = 0;
                while (index < num)
                {
                    if ((index != 0) && ((index % 0x10) == 0))
                    {
                        builder.Append("\n");
                    }
                    builder.Append(string.Format("{0:X2} ", b[index]));
                    index++;
                }
                if (b.Length > (num * 2))
                {
                    builder.Append(string.Format("\n   ...({0} other bytes here)....\n", b.Length - (num * 2)));
                }
                for (index = 0; index < num; index++)
                {
                    if ((index != 0) && ((index % 0x10) == 0))
                    {
                        builder.Append("\n");
                    }
                    builder.Append(string.Format("{0:X2} ", b[(b.Length - num) + index]));
                }
            }
            return builder.ToString();
        }

        internal static string FormatByteArray(byte[] b, int limit)
        {
            byte[] destinationArray = new byte[limit];
            Array.Copy(b, 0, destinationArray, 0, limit);
            return FormatByteArray(destinationArray);
        }
    }
}

