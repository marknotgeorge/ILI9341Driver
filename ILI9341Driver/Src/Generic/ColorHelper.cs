using System;

namespace Ili9341Driver
{
    /// <summary>
    /// A class of color helper functions.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Converts individual red, green and blue components of a color to an array of bytes
        /// </summary>
        /// <param name="red">The red component of a color</param>
        /// <param name="green">The green component of a color</param>
        /// <param name="blue">The blue component of a color</param>
        /// <returns>The color as an array of bytes in the order Blue, Green, Red</returns>
        public static byte[] RGB888ToBGR666ByteArray(byte red, byte green, byte blue)
        { 
            var bytes = new byte[3];

            // The Ili9341 will ignore the two least significant bits, so no
            // bit-shifting necessary.
            bytes[0] = red;  
            bytes[1] = green;  
            bytes[2] = blue;             

            return bytes;
        }

        /// <summary>
        /// Converts a color in hex code format (RRGGBBAA or RRGGBB) to an array of bytes
        /// </summary>
        /// <param name="hex">The hex code</param>
        /// <returns>The color as an array of bytes in the order Blue, Green, Red</returns>
        public static byte[] HexCodeToBGR666ByteArray(UInt32 hex)
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

            return RGB888ToBGR666ByteArray(red, green, blue);
        }

        /// <summary>
        /// Converts individual red, green and blue components of a color to a 16-bit BGR565 code
        /// </summary>
        /// <param name="red">The red component of a color</param>
        /// <param name="green">The green component of a color</param>
        /// <param name="blue">The blue component of a color</param>
        /// <returns>The color as a 16-bit BGR565 code</returns>
        public static ushort RGB888ToBGR565(byte red, byte green, byte blue)
        {
            ushort b = (ushort)(((blue >> 3) & 0x1f) << 11);
            ushort g = (ushort)(((green >> 2) & 0x3f) << 5);
            ushort r = (ushort)((red >> 3) & 0x1f) ;
            return (ushort)(b | g | r);
        }

        /// <summary>
        /// Converts a color in hex code format (RRGGBBAA or RRGGBB) to 16-bit BGR565 code
        /// </summary>
        /// <param name="hex">The color in hex code format</param>
        /// <returns>The color as a 16-bit BGR565 code</returns>
        public static ushort HexCodeToRGB565(UInt32 hex)
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

            return RGB888ToBGR565(red, green, blue);
        }
    }
}
