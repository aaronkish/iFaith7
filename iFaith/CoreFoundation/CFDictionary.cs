namespace CoreFoundation
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CFDictionary : CFType
    {
        public CFDictionary()
        {
        }

        public CFDictionary(IntPtr dictionary) : base(dictionary)
        {
        }

        public CFDictionary(string[] keys, IntPtr[] values)
        {
            IntPtr[] ptrArray = new IntPtr[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                ptrArray[i] = (IntPtr) new CFString(keys[i]);
            }
            CFDictionaryKeyCallBacks kcall = new CFDictionaryKeyCallBacks();
            CFDictionaryValueCallBacks vcall = new CFDictionaryValueCallBacks();
            base.typeRef = CFLibrary.CFDictionaryCreate(IntPtr.Zero, ptrArray, values, keys.Length, ref kcall, ref vcall);
        }

        public CFType GetValue(string value)
        {
            try
            {
                return new CFType(CFLibrary.CFDictionaryGetValue(base.typeRef, (IntPtr) new CFString(value)));
            }
            catch (Exception)
            {
                return new CFType(IntPtr.Zero);
            }
        }

        public static implicit operator IntPtr(CFDictionary value)
        {
            return value.typeRef;
        }

        public static implicit operator string(CFDictionary value)
        {
            return value.ToString();
        }

        public static implicit operator CFDictionary(IntPtr value)
        {
            return new CFDictionary(value);
        }

        public int Length
        {
            get
            {
                return CFLibrary.CFDictionaryGetCount(base.typeRef);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CFDictionaryCopyDescriptionCallBack(IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CFDictionaryEqualCallBack(IntPtr value1, IntPtr value2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CFDictionaryHashCallBack(IntPtr value);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct CFDictionaryKeyCallBacks
        {
            private int version;
            private CFDictionary.CFDictionaryRetainCallBack retain;
            private CFDictionary.CFDictionaryReleaseCallBack release;
            private CFDictionary.CFDictionaryCopyDescriptionCallBack copyDescription;
            private CFDictionary.CFDictionaryEqualCallBack equal;
            private CFDictionary.CFDictionaryHashCallBack hash;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CFDictionaryReleaseCallBack(IntPtr allocator, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CFDictionaryRetainCallBack(IntPtr allocator, IntPtr value);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct CFDictionaryValueCallBacks
        {
            private int version;
            private CFDictionary.CFDictionaryRetainCallBack retain;
            private CFDictionary.CFDictionaryReleaseCallBack release;
            private CFDictionary.CFDictionaryCopyDescriptionCallBack copyDescription;
            private CFDictionary.CFDictionaryEqualCallBack equal;
        }
    }
}

