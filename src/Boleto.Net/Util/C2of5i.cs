using System;
using System.Drawing;

namespace BoletoNet
{
    public class C2of5i : BarCodeBase
    {
        #region variables
        private string[] cPattern = new string[100];
        private const string START = "0000";
        private const string STOP = "1000";
        private Bitmap bitmap;
        private Graphics g;
        #endregion

        #region Constructor
        public C2of5i()
        {
        }
        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="Code">The string that contents the numeric code</param>
        /// <param name="BarWidth">The Width of each bar</param>
        /// <param name="Height">The Height of each bar</param>
        public C2of5i(string Code, int BarWidth, int Height)
        {
            this.Code = Code;
            this.Height = Height;
            this.Width = BarWidth;
        }
        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="Code">The string that contents the numeric code</param>
        /// <param name="BarWidth">The Width of each bar</param>
        /// <param name="Height">The Height of each bar</param>
        /// <param name="Digits">Number of digits of code</param>
        public C2of5i(string Code, int BarWidth, int Height, int Digits)
        {
            this.Code = Code;
            this.Height = Height;
            this.Width = BarWidth;
            this.Digits = Digits;
        }
        #endregion

        private void FillPatern()
        {
            int f;
            string strTemp;

            if (cPattern[0] == null)
            {
                cPattern[0] = "00110";
                cPattern[1] = "10001";
                cPattern[2] = "01001";
                cPattern[3] = "11000";
                cPattern[4] = "00101";
                cPattern[5] = "10100";
                cPattern[6] = "01100";
                cPattern[7] = "00011";
                cPattern[8] = "10010";
                cPattern[9] = "01010";
                //Create a draw pattern for each char from 0 to 99
                for (int f1 = 9; f1 >= 0; f1--)
                {
                    for (int f2 = 9; f2 >= 0; f2--)
                    {
                        f = f1 * 10 + f2;
                        strTemp = "";
                        for (int i = 0; i < 5; i++)
                        {
                            strTemp += cPattern[f1][i].ToString() + cPattern[f2][i].ToString();
                        }
                        cPattern[f] = strTemp;
                    }
                }
            }
        }
        /// <summary>
        /// Generate the Bitmap of Barcode.
        /// </summary>
        /// <returns>Return System.Drawing.Bitmap</returns>
        public Bitmap ToBitmap()
        {
            int i;
            string ftemp;

            xPos = 0;
            yPos = 0;

            if (this.Digits == 0)
            {
                this.Digits = this.Code.Length;
            }

            if (this.Digits % 2 > 0) this.Digits++;

            while (this.Code.Length < this.Digits || this.Code.Length % 2 > 0)
            {
                this.Code = "0" + this.Code;
            }

            int _width = (2 * Full + 3 * Thin) * (Digits) + 7 * Thin + Full;

            bitmap = new Bitmap(_width, Height);
            g = Graphics.FromImage(bitmap);

            //Start Pattern
            DrawPattern(ref g, START);

            //Draw code
            this.FillPatern();
            while (this.Code.Length > 0)
            {
                i = Convert.ToInt32(this.Code.Substring(0, 2));
                if (this.Code.Length > 2)
                    this.Code = this.Code.Substring(2, this.Code.Length - 2);
                else
                    this.Code = "";
                ftemp = cPattern[i];
                DrawPattern(ref g, ftemp);
            }

            //Stop Patern
            DrawPattern(ref g, STOP);

            return bitmap;
        }
        /// <summary>
        /// Returns the byte array of Barcode
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] ToByte()
        {
            return base.toByte(ToBitmap());
        }
    }
}
