using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Common.Page
{
    public class ValidateCode
    {
        private int letterWidth = 16;
        private int letterHeight = 21;
        private char[] chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz".ToCharArray();
        private string[] fonts = { "Arial", "Georgia" };
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="numberLength"></param>
        /// <returns></returns>
        public string GetRandomNumberString(int numberLength)
        {
            Random random = new Random();
            string validateCode = string.Empty;
            for (int i = 0; i < numberLength; i++)
            {
                validateCode += chars[random.Next(0, chars.Length)].ToString();
            }
            return validateCode;
        }

        public void CreateImage(string validateCode, HttpContext context)
        {
            int imageWidth = validateCode.Length * letterWidth + 8;
            Random newRandom = new Random();
            Bitmap image = new Bitmap(imageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            //随机生成器
            Random random = new Random();
            g.Clear(Color.White);
            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //随机字体和颜色的验证码
            int fontIndex;
            for (int i = 0, len = validateCode.Length; i < len; i++)
            {
                fontIndex = newRandom.Next(fonts.Length - 1);
                string charCode = validateCode.Substring(i, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePoint = new Point(i * letterWidth + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                g.DrawString(charCode, new Font(fonts[fontIndex], 12, FontStyle.Bold), newBrush, thePoint);
            }
            //扭曲图片
            image = TwistImage(image, true, 1, 1);
            //将生成的图片发送到客户端
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            context.Response.ClearContent();
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(stream.ToArray());
            g.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 随机获取一个颜色
        /// </summary>
        /// <returns></returns>
        public Color GetRandomColor()
        {
            Random random_First = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(random_First.Next(30));
            Random random_Second = new Random((int)DateTime.Now.Ticks);
            int red = random_First.Next(210);
            int green = random_Second.Next(180);
            int blue = red + green > 300 ? 0 : 400 - red - green;
            blue = blue > 255 ? 255 : blue;
            return Color.FromArgb(red, green, blue);
        }
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBitMap">图片路径</param>
        /// <param name="BXDir">如果为true则扭曲</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public Bitmap TwistImage(Bitmap srcBitMap, bool BXDir, double nMultValue, double dPhase)
        {
            Bitmap destMap = new Bitmap(srcBitMap.Width, srcBitMap.Height);
            Graphics g = Graphics.FromImage(destMap);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, destMap.Width, destMap.Height);
            g.Dispose();
            double dBaseAxisLen = BXDir ? (double)destMap.Height : (double)destMap.Width;
            for (int i = 0; i < destMap.Width; i++)
            {
                for (int j = 0; j < destMap.Height; j++)
                {
                    double dx = BXDir ? (2 * Math.PI * j) / dBaseAxisLen : (2 * Math.PI * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    //获取当前点的颜色
                    int n0IDX = 0, n0IDY = 0;
                    n0IDX = BXDir ? (int)(i + dy * nMultValue) : i;
                    n0IDY = BXDir ? j : (int)(j + dy * nMultValue);
                    Color color = srcBitMap.GetPixel(i, j);
                    if (n0IDX > 0 && n0IDX < destMap.Width && n0IDY > 0 && n0IDY < destMap.Height)
                    {
                        destMap.SetPixel(n0IDX, n0IDY, color);
                    }
                }
            }
            return destMap;
        }
    }
}
