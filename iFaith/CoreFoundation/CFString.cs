namespace CoreFoundation
{
    using System;

    public sealed class CFString : CFType
    {
        public CFString()
        {
        }

        public CFString(IntPtr myHandle) : base(myHandle)
        {
        }

        public CFString(string str)
        {
            base.typeRef = CFLibrary.__CFStringMakeConstantString(str);
        }

        public bool isString()
        {
            return (CFLibrary.CFGetTypeID(base.typeRef) == 7);
        }

        public static implicit operator IntPtr(CFString value)
        {
            return value.typeRef;
        }

        public static implicit operator string(CFString value)
        {
            return value.ToString();
        }

        public static implicit operator CFString(IntPtr value)
        {
            return new CFString(value);
        }

        public static implicit operator CFString(string value)
        {
            return new CFString(value);
        }
    }
}

