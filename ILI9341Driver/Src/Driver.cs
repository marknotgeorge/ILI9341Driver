using System;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace ILI9341Driver
{
    public partial class ILI9341
    {
        const byte lcdPortraitConfig = 8 | 0x40;  //Change for reversed portrait letters issue
        const byte lcdLandscapeConfig = 44;

        private readonly GpioPin _dataCommandPin;
        private readonly GpioPin _resetPin;
        private readonly GpioPin _backlightPin;
        private readonly GpioPin _chipSelectPin;

        private readonly SpiDevice _spi;
        private bool _isLandscape;

        private bool _backlightOn;
        public bool BacklightOn
        {
            get
            {
                return _backlightOn;
            }
            set
            {
                if (_backlightPin != null)
                {
                    var pinValue = value ? GpioPinValue.High : GpioPinValue.Low;
                    _backlightPin.Write(pinValue);
                    _backlightOn = value;
                }
            }
        }

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
            private set
            {
                _width = value;
            }
        }

        private int _height;

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        #region Constructors

        public ILI9341(bool isLandscape = false,
            GpioPin chipSelectPin = null,
            GpioPin dataCommandPin = null,
            GpioPin resetPin = null,
            GpioPin backlightPin = null,
            int spiClockFrequency = 10000000,
            SpiMode spiMode = SpiMode.Mode3,
            string spiBus = "SPI1")
        {
            if (chipSelectPin != null)
            {
                _chipSelectPin = chipSelectPin;
                _chipSelectPin.SetDriveMode(GpioPinDriveMode.Output); 
            }
            else
            {
                throw new ArgumentNullException("chipSelectPin");
            }

            if (dataCommandPin != null)
            {
                _dataCommandPin = dataCommandPin;
                _dataCommandPin.SetDriveMode(GpioPinDriveMode.Output); 
            }
            else
            {
                throw new ArgumentNullException("dataCommandPin");
            }

            if (resetPin != null)
            {
                _resetPin = resetPin;
                _resetPin.SetDriveMode(GpioPinDriveMode.Output); 
            }

            if (backlightPin != null)
            {
                _backlightPin = backlightPin;
                _backlightPin.SetDriveMode(GpioPinDriveMode.Output); 
            }

            var connectionSettings = new SpiConnectionSettings(chipSelectPin.PinNumber)
            {
                DataBitLength = 8,
                ClockFrequency = spiClockFrequency,
                Mode = spiMode
            };

            _spi = SpiDevice.FromId(spiBus, connectionSettings);
            InitializeScreen();
            SetOrientation(isLandscape);
        }

        #endregion Constructors

        #region Communication Methods

        protected virtual void Write(byte[] data)
        {
            _spi.Write(data);
        }

        protected virtual void Write(ushort[] data)
        {
            _spi.Write(data);
        }

        protected virtual void SendCommand(Commands command)
        {
            _dataCommandPin.Write(GpioPinValue.Low);
            Write(new[] { (byte)command });
        }

        protected virtual void SendData(params byte[] data)
        {
            _dataCommandPin.Write(GpioPinValue.High);
            Write(data);
        }

        protected virtual void SendData(params ushort[] data)
        {
            _dataCommandPin.Write(GpioPinValue.High);
            Write(data);
        }

        protected virtual void WriteReset(GpioPinValue value)
        {
            lock (this)
            {
                if (_resetPin != null)
                {

                    _resetPin.Write(value);
                }
            }
        }

        #endregion Communication Methods

        #region Public Methods

        public void FillScreen(int left, int right, int top, int bottom, ushort color)
        {
            lock (this)
            {
                SetWindow(left, right, top, bottom);

                var buffer = new ushort[Width];

                if (color != 0)
                {
                    for (var i = 0; i < Width; i++)
                    {
                        buffer[i] = color;
                    }
                }

                for (int y = 0; y < Height; y++)
                {
                    SendData(buffer);
                }

            }
        }

        public void ClearScreen()
        {
            lock (this)
            {
                FillScreen(0, Width - 1, 0, Height - 1, 0);
            }
        }

        public void SetPixel(int x, int y, ushort color)
        {
            lock (this)
            {
                SetWindow(x, x, y, y);
                SendData(color);
            }
        }

        public void SetOrientation(bool isLandscape)
        {
            lock (this)
            {
                _isLandscape = isLandscape;
                SendCommand(Commands.MemoryAccessControl);

                if (isLandscape)
                {
                    SendData(lcdLandscapeConfig);
                    Width = 320;
                    Height = 240;
                }
                else
                {
                    SendData(lcdPortraitConfig);
                    Width = 240;
                    Height = 320;
                }

                SetWindow(0, Width - 1, 0, Height - 1);
            }
        }

        public void ScrollUp(int pixels)
        {
            lock (this)
            {
                SendCommand(Commands.VerticalScrollingStartAddress);
                SendData((ushort)pixels);

                SendCommand(Commands.MemoryWrite);
            }
        }

        #endregion Public Methods



        protected virtual void InitializeScreen()
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
                SendData(lcdPortraitConfig);

                SendCommand(Commands.PixelFormatSet);
                SendData(0x55);//16-bits per pixel

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

        public static ushort ColorFromRgb(byte r, byte g, byte b)
        {
            return (ushort)((r << 11) | (g << 5) | b);
        }

        public void SetWindow(int left, int right, int top, int bottom)
        {
            lock (this)
            {
                SendCommand(Commands.ColumnAddressSet);
                SendData((byte)((left >> 8) & 0xFF),
                         (byte)(left & 0xFF),
                         (byte)((right >> 8) & 0xFF),
                         (byte)(right & 0xFF));
                SendCommand(Commands.PageAddressSet);
                SendData((byte)((top >> 8) & 0xFF),
                         (byte)(top & 0xFF),
                         (byte)((bottom >> 8) & 0xFF),
                         (byte)(bottom & 0xFF));
                SendCommand(Commands.MemoryWrite);
            }
        }
    }
}

