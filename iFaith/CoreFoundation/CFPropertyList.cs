namespace CoreFoundation
{
    using System;

    public class CFPropertyList : CFType
    {
        public CFPropertyList()
        {
        }

        public CFPropertyList(IntPtr PropertyList) : base(PropertyList)
        {
        }

        public CFPropertyList(string plistlocation)
        {
            IntPtr filePath = (IntPtr) new CFString(plistlocation);
            IntPtr fileURL = CFLibrary.CFURLCreateWithFileSystemPath(IntPtr.Zero, filePath, 2, false);
            IntPtr stream = CFLibrary.CFReadStreamCreateWithFile(IntPtr.Zero, fileURL);
            if (!CFLibrary.CFReadStreamOpen(stream))
            {
                base.typeRef = IntPtr.Zero;
            }
            IntPtr ptr4 = CFLibrary.CFPropertyListCreateFromStream(IntPtr.Zero, stream, 0, 2, 0, IntPtr.Zero);
            CFLibrary.CFReadStreamClose(stream);
            base.typeRef = ptr4;
        }

        public static implicit operator IntPtr(CFPropertyList value)
        {
            return value.typeRef;
        }

        public static implicit operator string(CFPropertyList value)
        {
            return value.ToString();
        }

        public static implicit operator CFPropertyList(IntPtr value)
        {
            return new CFPropertyList(value);
        }

        public static implicit operator CFPropertyList(string value)
        {
            return new CFPropertyList(value);
        }
    }
}

