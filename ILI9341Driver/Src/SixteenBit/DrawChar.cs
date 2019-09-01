using System;

namespace Ili9341Driver
{
    public partial class Ili9341SixteenBit
    {
        public void DrawChar(int x, int y, ushort color, FontCharacter character, bool isDebug = false)
        {
            lock (this)
            {
                SetWindow(x, x + character.Width - 1, y, y + character.Height - 1);

                var pixels = new byte[(character.Width * character.Height) * 2];
                var pixelPosition = 0;
                var lines = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x8, 0x4, 0x2, 0x1 };
                var swappedColor = SwapShortBytes(color);

                for (var segmentIndex = 0; segmentIndex < character.Data.Length; segmentIndex++)
                {
                    var segment = character.Data[segmentIndex];
                    for (var line = 0; line < lines.Length; line ++)
                    {
                        if (pixelPosition < pixels.Length)
                        {
                            pixels[pixelPosition] = (segment & lines[line]) != 0 ? swappedColor[0] : (byte)0;
                            pixels[pixelPosition + 1] = (segment & lines[line]) != 0 ? swappedColor[1] : (byte)0;
                            pixelPosition = pixelPosition + 2;
                        }
                    }                    
                }

                if (isDebug)
                {

                    var currentBuffer = string.Empty;
                    for (var pixel = 0; pixel < pixels.Length; pixel++)
                    {
                        if (pixels[pixel] > 0)
                        {
                            currentBuffer += "X";
                        }
                        else
                        {
                            currentBuffer += "-";
                        }
                        if (currentBuffer.Length >= character.Width)
                        {
                            Console.WriteLine(currentBuffer);
                            currentBuffer = string.Empty;
                        }
                    }
                }

                SendData(pixels);
            }

            
        }

        private byte[] SwapShortBytes(ushort color)
        {
            byte highByte = (byte)(color >> 8);
            byte lowByte = (byte)(color & 0xff);

            return new byte[] { highByte, lowByte };
        }
    }
}

