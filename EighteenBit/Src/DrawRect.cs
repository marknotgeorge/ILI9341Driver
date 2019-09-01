using System;

namespace ILI9341Driver
{
    public partial class Ili9341EighteenBit
    {
        public void DrawRect(int left, int right, int top, int bottom, UInt32 color)
        {
            var colorArray = ColorHelper.HexTo666ByteArray(color);

            DrawRect(left, right, top, bottom, colorArray);


        }

        public void DrawRect(int left, int right, int top, int bottom, byte[] color )
        {
            lock(this)
            {
                SetWindow(left, right, top, bottom);

                var buffer = new byte[Width * 3];
                DataBitLength = 24;
                if (color[0] != 0 || color[1] != 0 || color[2] != 0)
                {
                    for (var i = 0; i < Width * 3; i = i + 3)
                    {
                        buffer[i] = color[0];
                        buffer[i + 1] = color[1];
                        buffer[i + 2] = color[2];
                    }
                }

                for (int y = 0; y < Height; y++)
                {
                    SendData(buffer);
                }
                DataBitLength = 8;
            }

        }

        /// <summary>
        /// Fills the area of the screen described by <paramref name="left"/> <paramref name="right"/> <paramref name="top"/> & <paramref name="bottom"/> with <paramref name="bottom"/>
        /// </summary>        
        /// <param name="color">The color in RGB format</param>
        public void FillScreen(UInt32 color)
        {
            DrawRect(0, Width - 1, 0, Height - 1, color);
        }

        public void FillScreen(byte red, byte blue, byte green)
        {
            var colorArray = ColorHelper.RGB888To666ByteArray(red, green, blue);


            DrawRect(0, Width - 1, 0, Height - 1, colorArray);
        }

        public void ClearScreen()
        {
            DrawRect(0, Width - 1, 0, Height - 1, 0);
        }
    }
    
}
