namespace ILI9341Driver
{
    public partial class ILI9341
    {
        void FillRectangle(int x0, int y0, int width, int height, ushort color)
        {
            lock (this)
            {
                var x1 = x0 + width;               

                for (int x = x0; x < x1; x++)
                {
                    DrawVerticalLine(x, y0, height, color);
                       
                }
            }
        }
    }
}