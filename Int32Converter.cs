using System.Runtime.InteropServices;

namespace ILI9341Driver
{

    [StructLayout(LayoutKind.Explicit)]
    struct Int32Converter
    {
        [FieldOffset(0)] public int Value;
        [FieldOffset(0)] public byte Byte0;
        [FieldOffset(1)] public byte Byte1;
        [FieldOffset(2)] public byte Byte2;
        [FieldOffset(3)] public byte Byte3;

        public Int32Converter(uint value)
        {
            Byte0 = Byte1 = Byte3 = Byte3 = 0;
            Value = value;
        }

        public static implicit operator Int32(Int32Converter value)
        {
            return value.Value;
        }

        public static implicit operator Int32Converter(uint value)
        {
            return new Int32Converter(value);
        }
    }
}