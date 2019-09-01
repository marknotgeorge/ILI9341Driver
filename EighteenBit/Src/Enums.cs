using System;

namespace ILI9341Driver
{
    public enum Orientations
    {
        Portrait,
        Landscape,
        PortraitFlip,
        LandscapeFlip
    }

    [Flags]
    public enum MemoryAccessControl : byte
    {
        None = 0,
        MH = 0x04,
        BGR = 0x08,
        ML = 0x10,
        MV = 0x20,
        MX = 0x40,
        MY = 0x80
    }

    [Flags]
    public enum PixelFormat : byte
    {
        SixteenBit = 0x55,
        EighteenBit = 0x66
    }

    // Color definitions
    public enum Ili9341Colors : ushort
    {
        Black = 0x0000, //   0,   0,   0
        Navy = 0x000F, //   0,   0, 123
        Blue = 0x001F, //   0,   0, 255
        DarkGreen = 0x03E0, //   0, 125,   0
        DarkCyan = 0x03EF, //   0, 125, 123
        Green = 0x07E0, //   0, 255,   0
        Cyan = 0x07FF, //   0, 255, 255
        Maroon = 0x7800, // 123,   0,   0
        Purple = 0x780F, // 123,   0, 123
        Olive = 0x7BE0, // 123, 125,   0
        DarkGrey = 0x7BEF, // 123, 125, 123
        GreenYellow = 0xAFE5, // 173, 255,  41
        LightGrey = 0xC618, // 198, 195, 198       
        Red = 0xF800, // 255,   0,   0
        Magenta = 0xF81F, // 255,   0, 255
        Pink = 0xFC18, // 255, 130, 198
        Orange = 0xFD20, // 255, 165,   0 
        Yellow = 0xFFE0, // 255, 255,   0
        White = 0xFFFF // 255, 255, 255    
    }


}

