namespace CoreFoundation
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CFType
    {
        internal const int _CFArray = 0x12;
        internal const int _CFBoolean = 0x15;
        internal const int _CFData = 0x13;
        internal const int _CFDictionary = 0x11;
        internal const int _CFNumber = 0x16;
        internal const int _CFString = 7;
        internal IntPtr typeRef;

        public CFType()
        {
        }

        public CFType(IntPtr handle)
        {
            this.typeRef = handle;
        }

        public string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder builder = new StringBuilder(arrInput.Length);
            for (int i = 0; i <= (arrInput.Length - 1); i++)
            {
                builder.Append(arrInput[i].ToString("X2"));
            }
            return builder.ToString();
        }

        private string CFBoolean()
        {
            return CFLibrary.CFBooleanGetValue(this.typeRef).ToString();
        }

        private string CFData()
        {
            CoreFoundation.CFData data = new CoreFoundation.CFData(this.typeRef);
            return Convert.ToBase64String(data.ToByteArray());
        }

        private string CFNumber()
        {
            IntPtr valuePtr = Marshal.AllocCoTaskMem(CFLibrary.CFNumberGetByteSize(this.typeRef));
            if (!CFLibrary.CFNumberGetValue(this.typeRef, CFLibrary.CFNumberGetType(this.typeRef), valuePtr))
            {
                return string.Empty;
            }
            int num = (int) CFLibrary.CFNumberGetType(this.typeRef);
            switch (num)
            {
                case 1:
                    return Marshal.ReadInt16(valuePtr).ToString();

                case 2:
                    return Marshal.ReadInt16(valuePtr).ToString();

                case 3:
                    return Marshal.ReadInt32(valuePtr).ToString();

                case 4:
                    return Marshal.ReadInt64(valuePtr).ToString();

                case 6:
                    return (Enum.GetName(typeof(CoreFoundation.CFNumber.CFNumberType), num) + " is not supported yet! NIGGA");
            }
            return (Enum.GetName(typeof(CoreFoundation.CFNumber.CFNumberType), num) + " is not supported yet!");
        }

        private string CFPropertyList()
        {
            return Encoding.UTF8.GetString(new CoreFoundation.CFData(CFLibrary.CFPropertyListCreateXMLData(IntPtr.Zero, this.typeRef)).ToByteArray());
        }

        private unsafe string CFString()
        {
            if (this.typeRef == IntPtr.Zero)
            {
                return null;
            }
            int len = CFLibrary.CFStringGetLength(this.typeRef);
            IntPtr ptr = CFLibrary.CFStringGetCharactersPtr(this.typeRef);
            IntPtr zero = IntPtr.Zero;
            if (ptr == IntPtr.Zero)
            {
                CFRange range = new CFRange(0, len);
                zero = Marshal.AllocCoTaskMem(len * 2);
                CFLibrary.CFStringGetCharacters(this.typeRef, range, zero);
                ptr = zero;
            }
            string str = new string((char*) ptr, 0, len);
            if (zero != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(zero);
            }
            return str;
        }

        public string GetDescription()
        {
            return new CoreFoundation.CFString(CFLibrary.CFCopyDescription(this.typeRef)).ToString();
        }

        public int GetTypeID()
        {
            return CFLibrary.CFGetTypeID(this.typeRef);
        }

        public static implicit operator IntPtr(CFType value)
        {
            return value.typeRef;
        }

        public static implicit operator CFType(IntPtr value)
        {
            return new CFType(value);
        }

        public override string ToString()
        {
            switch (CFLibrary.CFGetTypeID(this.typeRef))
            {
                case 0x11:
                    return this.CFPropertyList();

                case 0x12:
                    return this.CFPropertyList();

                case 0x13:
                    return this.CFData();

                case 0x15:
                    return this.CFBoolean();

                case 0x16:
                    return this.CFNumber();

                case 7:
                    return this.CFString();
            }
            return null;
        }
    }
}

