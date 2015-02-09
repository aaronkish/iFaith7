namespace CoreFoundation
{
    using System;

    public class CFData : CFType
    {
        public CFData()
        {
        }

        public CFData(IntPtr Data) : base(Data)
        {
        }

        public unsafe CFData(byte[] Data)
        {
            byte[] buffer = Data;
            int length = buffer.Length;
            fixed (byte* numRef = buffer)
            {
                base.typeRef = CFLibrary.CFDataCreate(IntPtr.Zero, (IntPtr) numRef, length);
            }
        }

        public bool isData()
        {
            return (CFLibrary.CFGetTypeID(base.typeRef) == 0x13);
        }

        public int Length()
        {
            return CFLibrary.CFDataGetLength(base.typeRef);
        }

        public static implicit operator IntPtr(CFData value)
        {
            return value.typeRef;
        }

        public static implicit operator CFData(IntPtr value)
        {
            return new CFData(value);
        }

        public unsafe byte[] ToByteArray()
        {
            int len = this.Length();
            byte[] buffer = new byte[len];
            fixed (byte* numRef = buffer)
            {
                CFLibrary.CFDataGetBytes(base.typeRef, new CFRange(0, len), (IntPtr) numRef);
            }
            return buffer;
        }
    }
}

