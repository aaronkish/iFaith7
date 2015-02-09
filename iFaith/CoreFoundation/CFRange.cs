namespace CoreFoundation
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CFRange
    {
        public int Location;
        public int Length;
        public CFRange(int l, int len)
        {
            this.Location = l;
            this.Length = len;
        }
    }
}

