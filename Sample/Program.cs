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
            Configuration.SetPinFunction(19, DeviceFunction.SPI2_MISI);
            Configuration.SetPinFunction(18, DeviceFunction.SPI2_MOSI);
            Configuration.SetPinFunction(5, DeviceFunction.SPI2_CLOCK);             
            
            var dataCommandPin = gpioController.OpenPin(33);
            var chipSelectPin = gpioController.OpenPin(15);
            var resetPin = gpioController.OpenPin(13);
            //var backlightPin = gpioController.OpenPin(32);

            var tft = new ILI9341(isLandscape: true,
                chipSelectPin: chipSelectPin,
                dataCommandPin: dataCommandPin,
                resetPin: resetPin,
                spiBus: "SPI2"//,
                /*backlightPin: backlightPin*/);

            var font = new StandardFixedWidthFont();
            tft.ClearScreen();
            tft.DrawString(10, 10, "Hello world!", 0xF800, font);
            //tft.BacklightOn = true;

            Thread.Sleep(Timeout.Infinite);            
        }
    }
}
