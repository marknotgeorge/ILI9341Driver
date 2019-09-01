namespace Ili9341Driver
{
    public partial class Ili9341SixteenBit
    {
        public void DrawVerticalLine(int x, int y, int length, ushort color)
        {
            lock (this)
            {
                SetWindow(x, x, y, y + length);

                var buffer = new ushort[length];

                for (int i = 0; i < length; i++)
                {
                    buffer[i] = color;
                }

                SendData(buffer);
            }
        }

        public void DrawFastVLine(int x, int y, int length, ushort color)
        {
            DrawVerticalLine(x, y, length, color);
        }
    }
}
