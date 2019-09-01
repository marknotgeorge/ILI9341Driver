using Ili9341Driver;
using nanoFramework.Hardware.Esp32;
using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace EighteenBitSample
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

            var tft = new Ili9341EighteenBit(
                rotation: Orientations.Portrait,
                chipSelectPin: chipSelectPin,
                dataCommandPin: dataCommandPin,
                resetPin: resetPin,
                spiBus: "SPI2"//,
                              /*backlightPin: backlightPin*/);

            tft.ClearScreen();

            Thread.Sleep(500);

            UInt32 testFillColor = 0xFF0000;
;
            //Console.WriteLine($"Test color: {testFillColor}");

            TestFill(tft, testFillColor);


            Thread.Sleep(Timeout.Infinite);
        }

        private static void TestFill(Ili9341EighteenBit tft, UInt32 color)
        {
            tft.FillScreen(color);
        }
    }
}
