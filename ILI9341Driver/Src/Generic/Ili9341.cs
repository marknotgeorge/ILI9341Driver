using System;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace Ili9341Driver
{
    public abstract partial class Ili9341
    {
        // Any faster than 33MHz causes an exception
        public const int MaxSPIFrequency = 33000000;

        protected readonly GpioPin _dataCommandPin;
        protected readonly GpioPin _resetPin;
        protected readonly GpioPin _backlightPin;
        protected readonly GpioPin _chipSelectPin;

        protected readonly SpiDevice _spi;

        protected byte[] _orientationFlags;

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

        private ushort _cursorX;

        public ushort CursorX
        {
            get { return _cursorX; }
            set { _cursorX = value; }
        }

        private ushort _cursorY;

        public ushort CursorY
        {
            get { return _cursorY; }
            set { _cursorY = value; }
        }

        private ushort _textColor;

        public ushort TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private Font _textFont;
        public Font TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        private bool _textWrap = true;

        public bool TextWrap
        {
            get { return _textWrap; }
            set { _textWrap = value; }
        }

        private int _dataBitLength;
        public int DataBitLength
        {
            get
            {
                return _dataBitLength;
            }
            set
            {
                _dataBitLength = value;
                _spi.ConnectionSettings.DataBitLength = value;
            }
        }


        #region Constructors

        public Ili9341(
            GpioPin chipSelectPin = null,
            GpioPin dataCommandPin = null,
            GpioPin resetPin = null,
            GpioPin backlightPin = null,            
            int spiClockFrequency = MaxSPIFrequency,
            SpiMode spiMode = SpiMode.Mode0,
            string spiBus = "SPI1",
            Orientations rotation = Orientations.Landscape,            
            Font textFont = null)
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

            

            if (textFont == null)
            {
                _textFont = new StandardFixedWidthFont();
            }


            var connectionSettings = new SpiConnectionSettings(chipSelectPin.PinNumber)
            {
                DataBitLength = 8,
                ClockFrequency = spiClockFrequency,
                Mode = spiMode,                
            };

            _spi = SpiDevice.FromId(spiBus, connectionSettings);

            InitializeScreen();
            SetRotation(rotation);
        }

        #endregion Constructors

        #region Communication Methods

        public void WriteData(byte[] data)
        {
            _spi.Write(data);
        }

        public void WriteData(ushort[] data)
        {
            _spi.Write(data);
        }

        protected virtual void SendCommand(Commands command)
        {
            _dataCommandPin.Write(GpioPinValue.Low);
            WriteData(new[] { (byte)command });
        }

        public void SendData(params byte[] data)
        {
            _dataCommandPin.Write(GpioPinValue.High);
            WriteData(data);
        }

        public void SendData(params ushort[] data)
        {
            _dataCommandPin.Write(GpioPinValue.High);
            WriteData(data);
        }

        public void WriteReset(GpioPinValue value)
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

        public void SetRotation(Orientations orientation)
        {
            SendCommand(Commands.MemoryAccessControl);

            switch (orientation)
            {
                default:
                case Orientations.Portrait:
                    SendData((byte)(MemoryAccessControl.MX | MemoryAccessControl.BGR));                    
                    Width = 240;
                    Height = 320;
                    break;
                case Orientations.Landscape:
                    SendData((byte)(MemoryAccessControl.MV | MemoryAccessControl.BGR));
                    Width = 320;
                    Height = 240;
                    break;
                case Orientations.PortraitFlip:
                    SendData((byte)(MemoryAccessControl.MY | MemoryAccessControl.ML | MemoryAccessControl.BGR));
                    Width = 240;
                    Height = 320;
                    break;
                case Orientations.LandscapeFlip:
                    SendData((byte)(MemoryAccessControl.ML | MemoryAccessControl.MV | MemoryAccessControl.MX | MemoryAccessControl.MY | MemoryAccessControl.BGR));
                    Width = 320;
                    Height = 240;
                    break;




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

        public void SetCursor(ushort x, ushort y)
        {
            _cursorX = x;
            _cursorY = y;
        }

        #endregion Public Methods

        #region Initialization
        public abstract void InitializeScreen();
        #endregion



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

