namespace CoreFoundation
{
    using System;

    public class CFNumber : CFType
    {
        public CFNumber()
        {
        }

        public unsafe CFNumber(int Number)
        {
            int* valuePtr = &Number;
            base.typeRef = CFLibrary.CFNumberCreate(IntPtr.Zero, CFNumberType.kCFNumberIntType, valuePtr);
        }

        public CFNumber(IntPtr Number) : base(Number)
        {
        }

        public enum CFNumberType
        {
            kCFNumberCFIndexType = 14,
            kCFNumberCGFloatType = 0x10,
            kCFNumberCharType = 7,
            kCFNumberDoubleType = 13,
            kCFNumberFloat32Type = 5,
            kCFNumberFloat64Type = 6,
            kCFNumberFloatType = 12,
            kCFNumberIntType = 9,
            kCFNumberLongLongType = 11,
            kCFNumberLongType = 10,
            kCFNumberMaxType = 0x10,
            kCFNumberNSIntegerType = 15,
            kCFNumberShortType = 8,
            kCFNumberSInt16Type = 2,
            kCFNumberSInt32Type = 3,
            kCFNumberSInt64Type = 4,
            kCFNumberSInt8Type = 1
        }
    }
}

