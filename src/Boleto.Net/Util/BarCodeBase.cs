using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BoletoNet.Util
{
    public abstract class BarCodeBase
    {
        #region Variables
        private string _code;
        private int _height;
        private int _digits;

        private int _thin;
        private int _full;

        protected int XPos = 0;
        protected int YPos = 0;

        private string _contenttype;

        protected Brush Black = Brushes.Black;
        protected Brush White = Brushes.White;

        #endregion

        #region Property
        /// <summary>
        /// The Barcode.
        /// </summary>
        public string Code
        {
            get
            {
                try
                {
                    return _code;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    _code = value;
                }
                catch
                {
                    _code = null;
                }
            }
        }
        /// <summary>
        /// The width of the thin bar (pixels).
        /// </summary>
        public int Width
        {
            get
            {
                try
                {
                    return _thin;
                }
                catch
                {
                    return 1;
                }
            }
            set
            {
                try
                {
                    var temp = value;
                    _thin = temp;
                    //					_half = temp * 2;
                    _full = temp * 3;
                }
                catch
                {
                    const int temp = 1;
                    _thin = temp;
                    //					_half = temp * 2;
                    _full = temp * 3;
                }
            }
        }
        /// <summary>
        /// The Height of barcode (pixels).
        /// </summary>
        public int Height
        {
            get
            {
                try
                {
                    return _height;
                }
                catch
                {
                    return 15;
                }
            }
            set
            {
                try
                {
                    _height = value;
                }
                catch
                {
                    _height = 1;
                }
            }
        }
        /// <summary>
        /// Number of digits of the barcode.
        /// </summary>
        public int Digits
        {
            get
            {
                try
                {
                    return _digits;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    _digits = value;
                }
                catch
                {
                    _digits = 0;
                }
            }
        }
        /// <summary>
        /// Content type of code. Default: image/jpeg
        /// </summary>
        public string ContentType
        {
            get
            {
                try
                {
                    if (_contenttype == null)
                        return "image/jpeg";
                    return _contenttype;
                }
                catch
                {
                    return "image/jpeg";
                }
            }
            set
            {
                try
                {
                    _contenttype = value;
                }
                catch
                {
                    _contenttype = "image/jpeg";
                }
            }
        }
        protected int Thin
        {
            get
            {
                try
                {
                    return _thin;
                }
                catch
                {
                    return 1;
                }
            }
        }
        protected int Full
        {
            get
            {
                try
                {
                    return _full;
                }
                catch
                {
                    return 3;
                }
            }
        }
        #endregion
        protected virtual byte[] ToByte(Bitmap bitmap)
        {
            var mstream = new MemoryStream();
            var myImageCodecInfo = GetEncoderInfo(ContentType);
            var myEncoderParameter0 = new EncoderParameter(Encoder.Quality, (long)100);
            var myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = myEncoderParameter0;

            bitmap.Save(mstream, myImageCodecInfo, myEncoderParameters);

            return mstream.GetBuffer();
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            var encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        protected virtual void DrawPattern(ref Graphics g, string Pattern)
        {
            for (var i = 0; i < Pattern.Length; i++)
            {
                var tempWidth = Pattern[i] == '0' ? _thin : _full;

                g.FillRectangle(i % 2 == 0 ? Black : White, XPos, YPos, tempWidth, _height);

                XPos += tempWidth;
            }
        }


    }
}
