using System;
using System.Threading;
using ILI9341Driver;
using nanoFramework.Hardware.Esp32;
using Windows.Devices.Gpio;

namespace nanoIli9341.Sample
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

            var tft = new ILI9341(
                rotation: Orientations.Portrait,
                chipSelectPin: chipSelectPin,
                dataCommandPin: dataCommandPin,
                resetPin: resetPin,
                spiBus: "SPI2"//,
                /*backlightPin: backlightPin*/);


            var result = testFillScreen(tft) / 10000;
            Console.WriteLine($"Screen fill test: {result}ms");
            Thread.Sleep(500);

            result = testText(tft) / 10000;
            Console.WriteLine($"Text test: {result}ms");

            
        
            Thread.Sleep(Timeout.Infinite);            
        }        

        private static long testText(ILI9341 tft)
        {
            var start = DateTime.UtcNow.Ticks;
            tft.FillScreen((ushort)Ili9341Colors.Black);
            tft.SetCursor(0, 0);
            tft.TextColor = (ushort)Ili9341Colors.White;
            tft.WriteLine("Hello World!");
            tft.TextColor = (ushort)Ili9341Colors.Magenta;
            tft.WriteLine(1234.56);
            tft.TextColor = (ushort)Ili9341Colors.Red;
            tft.WriteLine(0xDEADBEEF);
            tft.WriteLine();
            tft.TextColor = (ushort)Ili9341Colors.Green;
            tft.WriteLine("A wizard's staff has a knob on the end");
            tft.WriteLine("And runes run up the shaft");
            tft.WriteLine("It's long and proud and stiff and loud");
            tft.WriteLine("It's the pride of wizardcraft.");

            var finish = DateTime.UtcNow.Ticks;
            return finish - start;
        }

        private static long testFillScreen(ILI9341 tft)
        {
            var start = DateTime.UtcNow.Ticks;

            tft.FillScreen((ushort)Ili9341Colors.Black);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Ili9341Colors.Red);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Ili9341Colors.Green);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Ili9341Colors.Blue);
            Thread.Sleep(50);
            tft.FillScreen((ushort)Ili9341Colors.Black);
            Thread.Sleep(50);

            var finish = DateTime.UtcNow.Ticks;

            return finish - start;
        }
    }
}
