namespace Ili9341Driver
{
    public partial class Ili9341SixteenBit
    {
        public void DrawLine(int x0, int y0, int x1, int y1, ushort color)
        {
            lock (this)
            {
                // use the fast methods if possible
                if (x0 == x1)
                {
                    if (y1 > y0)
                    {
                        DrawVerticalLine(x0, y0, y1 - y0, color);
                    }
                    else
                    {
                        DrawVerticalLine(x0, y1, y0 - y1, color);
                    }
                    return;
                }
                if (y0 == y1)
                {
                    if(x1 > x0)
                    {
                        DrawHorizontalLine(x0, y0, x1 - x0, color);
                    }
                    else
                    {
                        DrawHorizontalLine(x1, y0, x0 - x1, color);
                    }
                    return;
                }

                // If x-coordinates are the same, and y-coordinates are the 
                // same, we have a single pixel.
                if ((x0 == x1) && (y0 == y1))
                {
                    DrawPixel(x0, y0, color);
                    return;
                }

                // The line function goes from left to right, so x0 must be
                // less than x1. If not, swap them.
                if (x0 < x1)
                {
                    var temp = x0;
                    x0 = x1;
                    x1 = temp;
                    temp = y0;
                    y0 = y1;
                    y1 = temp;
                }

                // The formula for the line is y = mx + c,
                // where m is deltaX / deltaY;
                var deltaX = (double)(x0 - x1);
                var deltaY = (double)(y0 - y1);

                // Calculate m and c
                var m = deltaX / deltaY;                
                var c = y0 - (m * x0);

                double y;
                for (var x = x0; x < x1; x++)
                {
                    // Calculate y
                    y = (m * x) + c;
                    DrawPixel(x, (int)y, color);
                }
            }
        }
    }
}