using System;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace Ili9341Driver
{
    public partial class Ili9341EighteenBit : Ili9341
    {
        #region Constructor
        public Ili9341EighteenBit(
            GpioPin chipSelectPin = null,
            GpioPin dataCommandPin = null,
            GpioPin resetPin = null,
            GpioPin backlightPin = null,
            int spiClockFrequency = MaxSPIFrequency,
            SpiMode spiMode = SpiMode.Mode0,
            string spiBus = "SPI1",
            Orientations rotation = Orientations.Landscape,
            Font textFont = null)
            :base(chipSelectPin, dataCommandPin, resetPin, backlightPin, spiClockFrequency, spiMode, spiBus, rotation, textFont)
        {

        }
        #endregion

        #region Initialize

        public override void InitializeScreen()
        {
            lock (this)
            {
                WriteReset(GpioPinValue.Low);
                Thread.Sleep(10);
                WriteReset(GpioPinValue.High);
                SendCommand(Commands.SoftwareReset);
                Thread.Sleep(10);
                SendCommand(Commands.DisplayOff);

                SendCommand(Commands.MemoryAccessControl);
                var initializeMAC = (MemoryAccessControl.MX|MemoryAccessControl.BGR);
                SendData((byte)initializeMAC);

                SendCommand(Commands.PixelFormatSet);
                SendData((byte)PixelFormat.EighteenBit); //18-bits per pixel

                SendCommand(Commands.FrameControlNormal);
                SendData(0x00, 0x1B);

                SendCommand(Commands.GammaSet);
                SendData(0x01);

                SendCommand(Commands.ColumnAddressSet); //width of the screen
                SendData(0x00, 0x00, 0x00, 0xEF);

                SendCommand(Commands.PageAddressSet); //height of the screen
                SendData(0x00, 0x00, 0x01, 0x3F);

                SendCommand(Commands.EntryModeSet);
                SendData(0x07);

                SendCommand(Commands.DisplayFunctionControl);
                SendData(0x0A, 0x82, 0x27, 0x00);

                SendCommand(Commands.SleepOut);
                Thread.Sleep(120);

                SendCommand(Commands.DisplayOn);
                Thread.Sleep(100);

                SendCommand(Commands.MemoryWrite);
            }
        }

        #endregion

    }
}

