using System;

namespace ILI9341Driver
{
    public static class ColorHelper
    {
        public static byte[] RGB888To666ByteArray(byte red, byte green, byte blue)
        { 
            var bytes = new byte[3];

            bytes[2] = red;  
            bytes[1] = green;  
            bytes[0] = blue;             

            return bytes;
        }

        public static byte[] HexTo666ByteArray(UInt32 hex)
        {
            byte red = 0;
            byte green = 0;
            byte blue = 0;

            if (hex > 0x01000000)
            {
                // Hex is RRGGBBAA - make it RRGGBB
                // (We're ignoring AA)
                var colorShift = hex >> 8;
                hex = colorShift;
            }

            // Split hex into individual color fragments
            red = (byte)((hex & 0xFF0000) >> 16);
            green = (byte)((hex & 0x00FF00) >> 8);
            blue = (byte)(hex & 0x0000FF);

            return RGB888To666ByteArray(red, green, blue);
        }

        public static ushort RGB888ToRGB565(byte red, byte green, byte blue)
        {
            ushort b = (ushort)((blue >> 3) & 0x1f);
            ushort g = (ushort)(((green >> 2) & 0x3f) << 5);
            ushort r = (ushort)(((red >> 3) & 0x1f) << 11);
            return (ushort)(r | g | b);
        }
    }
}
