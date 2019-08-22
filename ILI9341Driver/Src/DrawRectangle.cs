namespace ILI9341Driver
{
    public partial class ILI9341
    {
        public void DrawRectangle(int x, int y, int width, int height, ushort color)
        {
            var x1 = x + width;
            var y1 = y + height;

            DrawVerticalLine(x, y, height, color);          // Left
            DrawVerticalLine(x1, y, height + 1, color);     // Right
            DrawHorizontalLine(x, y, width, color);         // Top
            DrawHorizontalLine(x, y1, width + 1, color);    // Bottom
        }

        public void DrawRect(int x0, int y0, int w, int h, ushort color)
        {
            DrawRectangle(x0, y0, w, h, color);
        }
    }
}