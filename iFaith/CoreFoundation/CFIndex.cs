namespace CoreFoundation
{
    using System;
    using System.Runtime.InteropServices;

    public class CFIndex
    {
        internal IntPtr theIndex;

        public CFIndex()
        {
        }

        public CFIndex(IntPtr Index)
        {
            this.theIndex = Index;
        }

        public static implicit operator CFIndex(IntPtr Index)
        {
            return new CFIndex(Index);
        }

        public int ToInteger()
        {
            return Marshal.ReadInt32(this.theIndex);
        }
    }
}

