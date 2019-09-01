using System;
using System.Threading;
using Ili9341Driver;
using nanoFramework.Hardware.Esp32;
using Windows.Devices.Gpio;

namespace SixteenBitSample
{
    public class Program
    {
        public static void Main()
        {
            var gpioController = GpioController.GetDefault();

            // Set SPI2 pins for ESP32, I'm using an Adafruit HUZZAH32
            // to test.
            Configuration.SetPinFunction(19, DeviceFunction.SPI2_MISO);
            Configuration.SetPinFunction(18, DeviceFunction.SPI2_MOSI);
            Configuration.SetPinFunction(5, DeviceFunction.SPI2_CLOCK);             
            
            var dataCommandPin = gpioController.OpenPin(33);
            var chipSelectPin = gpioController.OpenPin(15);
            var resetPin = gpioController.OpenPin(13);
            //var backlightPin = gpioController.OpenPin(32);

            var tft = new Ili9341SixteenBit(
                rotation: Orientations.Portrait,
                chipSelectPin: chipSelectPin,
                dataCommandPin: dataCommandPin,
                resetPin: resetPin,
                spiBus: "SPI2"//,
                /*backlightPin: backlightPin*/);


            var result = TestFillScreen(tft) / 10000;
            Console.WriteLine($"Screen fill test: {result}ms");            

            result = TestText(tft) / 10000;
            Console.WriteLine($"Text test: {result}ms");

            result = TestColor(tft) / 10000;
            Console.WriteLine($"Color test: {result}ms");
        
            Thread.Sleep(Timeout.Infinite);            
        }

        private static void TestFill(Ili9341SixteenBit tft, ushort color)
        {
            tft.FillScreen((ushort)color);
        }

        private static long TestText(Ili9341SixteenBit tft)
        {
            var start = DateTime.UtcNow.Ticks;
            tft.FillScreen((ushort)Colors565.Black);
            tft.SetCursor(0, 0);
            tft.TextColor = (ushort)Colors565.White;
            tft.WriteLine("Hello World!");
            tft.TextColor = (ushort)Colors565.Magenta;
            tft.WriteLine(1234.56);
            tft.TextColor = (ushort)Colors565.Red;
            tft.WriteLine(0xDEADBEEF);
            tft.WriteLine();
            tft.TextColor = (ushort)Colors565.Green;
            tft.WriteLine("A wizard's staff has a knob on the end");
            tft.WriteLine("And runes run up the shaft");
            tft.WriteLine("It's long and proud and stiff and loud");
            tft.WriteLine("It's the pride of wizardcraft.");

            var finish = DateTime.UtcNow.Ticks;
            return finish - start;
        }

        private static long TestFillScreen(Ili9341SixteenBit tft)
        {
            var start = DateTime.UtcNow.Ticks;

            tft.FillScreen((ushort)Colors565.Black);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Colors565.Red);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Colors565.Green);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Colors565.Blue);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Colors565.Black);
            Thread.Sleep(50);

            var finish = DateTime.UtcNow.Ticks;

            return finish - start;
        }

        private static long TestColor(Ili9341SixteenBit tft)
        {
            var start = DateTime.UtcNow.Ticks;

            tft.FillScreen((ushort)Colors565.Black);
            tft.SetCursor(0, 0);
            tft.TextColor = (ushort)Colors565.Navy;
            tft.WriteLine("Navy");
            tft.TextColor = (ushort)Colors565.Blue;
            tft.WriteLine("Blue");
            tft.TextColor = (ushort)Colors565.DarkGreen;
            tft.WriteLine("Dark Green");
            tft.TextColor = (ushort)Colors565.DarkCyan;
            tft.WriteLine("Dark Cyan");
            tft.TextColor = (ushort)Colors565.Green;
            tft.WriteLine("Green");
            tft.TextColor = (ushort)Colors565.Cyan;
            tft.WriteLine("Cyan");
            tft.TextColor = (ushort)Colors565.Maroon;
            tft.WriteLine("Maroon");
            tft.TextColor = (ushort)Colors565.Purple;
            tft.WriteLine("Purple");
            tft.TextColor = (ushort)Colors565.Olive;
            tft.WriteLine("Olive");
            tft.TextColor = (ushort)Colors565.DarkGrey;
            tft.WriteLine("Dark Grey");
            tft.TextColor = (ushort)Colors565.GreenYellow;
            tft.WriteLine("GreenYellow");
            tft.TextColor = (ushort)Colors565.LightGrey;
            tft.WriteLine("Light Grey");
            tft.TextColor = (ushort)Colors565.Red;
            tft.WriteLine("Red");
            tft.TextColor = (ushort)Colors565.Magenta;
            tft.WriteLine("Magenta");
            tft.TextColor = (ushort)Colors565.Pink;
            tft.WriteLine("Pink");
            tft.TextColor = (ushort)Colors565.Orange;
            tft.WriteLine("Orange");
            tft.TextColor = (ushort)Colors565.Yellow;
            tft.WriteLine("Yellow");
            tft.TextColor = (ushort)Colors565.White;            
            tft.WriteLine("White");

            var finish = DateTime.UtcNow.Ticks;
            return finish - start;
        }


    }
}
