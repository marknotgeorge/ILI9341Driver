namespace Ili9341Driver
{
    public partial class Ili9341SixteenBit
    {
        public void DrawRect(int left, int right, int top, int bottom, ushort color)
        {
            lock(this)
            {
                SetWindow(left, right, top, bottom);

                var buffer = new byte[Width * 2];
                DataBitLength = 16;
                if (color != 0)
                {
                    byte highByte = (byte)(color >> 8);
                    byte lowByte = (byte)(color & 0xff);

                    for (var i = 0; i < Width * 2; i = i + 2)
                    {
                        buffer[i] = lowByte;
                        buffer[i + 1] = highByte;                        
                    }
                }

                for (int y = 0; y < Height; y++)
                {
                    SendData(buffer);
                }
                DataBitLength = 8;
            }
        }

        public void DrawRect(int left, int right, int top, int bottom, byte red, byte green, byte blue)
        {
            DrawRect(left, right, top, bottom, ColorHelper.RGB888ToBGR565(red, green, blue));

        }

        public void DrawPixel(int x, int y, ushort color)
        {
            DrawRect(x, x, y, y, color);
        }

        /// <summary>
        /// Fills the area of the screen described by <paramref name="left"/> <paramref name="right"/> <paramref name="top"/> & <paramref name="bottom"/> with <paramref name="bottom"/>
        /// </summary>        
        /// <param name="color">The color in RGB format</param>
        public void FillScreen(ushort color)
        {
            DrawRect(0, Width - 1, 0, Height - 1, color);
        }

        public void ClearScreen()
        {
            DrawRect(0, Width - 1, 0, Height - 1, 0);
        }
    }
}