using System;

namespace ILI9341Driver
{
    public partial class ILI9341
    {
        public void DrawHorizontalLine(int x, int y, int length, ushort color)
        {
            lock (this)
            {
                SetWindow(x, x + length, y, y);

                var buffer = new ushort[length];

                for (int i = 0; i < length; i++)
                {
                    buffer[i] = color;
                }

                SendData(buffer);
            }
        }

        public void DrawFastHLine(int x, int y, int length, ushort color)
        {
            DrawHorizontalLine(x, y, length, color);
        }
    }
}
