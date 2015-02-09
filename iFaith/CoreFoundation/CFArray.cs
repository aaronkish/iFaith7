namespace CoreFoundation
{
    using System;

    public class CFArray : CFType
    {
        public CFArray()
        {
        }

        public CFArray(IntPtr Number) : base(Number)
        {
        }

        public CFArray(IntPtr[] values)
        {
            try
            {
                base.typeRef = CFLibrary.CFArrayCreate(IntPtr.Zero, values, values.Length, IntPtr.Zero);
            }
            catch (Exception)
            {
                base.typeRef = IntPtr.Zero;
            }
        }

        public CFType GetValue(int index)
        {
            if (index >= this.GetCount)
            {
                return new CFType(IntPtr.Zero);
            }
            return new CFType(CFLibrary.CFArrayGetValueAtIndex(base.typeRef, index));
        }

        public int GetCount
        {
            get
            {
                return CFLibrary.CFArrayGetCount(base.typeRef);
            }
        }
    }
}

