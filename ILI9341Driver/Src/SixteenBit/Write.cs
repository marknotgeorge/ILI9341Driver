namespace Ili9341Driver
{
    public partial class Ili9341SixteenBit
    {
        public void Write(string s)
        {            

            // Needed because string in nanoframework doesn't implement IEnumerable
            var schars = s.ToCharArray();

            foreach (var c in schars)
            {
                var character = TextFont.GetFontData(c);

                if (c == '\n') // Line feed
                {
                    CursorX = 0;
                    CursorY += character.Height;
                }
                else if (c != '\r') // Ignore carriage return
                {
                    if (TextWrap && (CursorX + character.Width) > Width)
                    {
                        CursorX = 0;
                        CursorY += character.Height;
                    }
                    DrawChar(CursorX, CursorY, TextColor, character);
                    CursorX += character.Width;
                }                    
            }

        }

        public void Write(object s)
        {
            Write(s.ToString());
        }

        public void WriteLine(object s)
        {
            Write(s.ToString() + "\n\r");           
        }

        public void WriteLine()
        {
            Write("\n\r");
        }
    }
}

