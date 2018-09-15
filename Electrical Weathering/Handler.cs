using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrical_Weathering
{
    class Handler
    {
        static public Image Weathering(Image Source,long Quality)
        {
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            EncoderParameters myEncoderParameters = new EncoderParameters(1); 
            myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100 - Quality);

            using (Bitmap bitmap = new Bitmap(Source))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, myImageCodecInfo, myEncoderParameters);
                    return Image.FromStream(ms);
                }
            }
        }

        static public ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        static public Image Greening(Image Source,int alpha)
        {
            Bitmap background = new Bitmap(Source);

            Bitmap Mix = new Bitmap(Source);
            int x, y;
            for (x = 0; x < background.Width; x++)
            {
                for (y = 0; y < background.Height; y++)
                {
                    Color sourceColor = background.GetPixel(x, y);
                    Color mixColor = Color.FromArgb(
                        255,
                        sourceColor.R * (100 - alpha) / 100,
                        sourceColor.G * (100 - alpha) / 100 + 255 * alpha / 100,
                        sourceColor.B * (100 - alpha) / 100
                        );
                    Mix.SetPixel(x, y, mixColor);
                }
            }

            return Mix;
        }

        static public Image Noising(Image Source, int alpha)
        {
            Bitmap background = new Bitmap(Source);
            Bitmap Mix = new Bitmap(Source);
            Random rnd = new Random();
             
           
            int x, y;
            for (x = 0; x < background.Width; x++)
            {
                for (y = 0; y < background.Height; y++)
                {
                    Color sourceColor = background.GetPixel(x, y);
                    Color mixColor = Color.FromArgb(
                        255,
                        sourceColor.R * (100 - alpha) / 100 + rnd.Next(255) * alpha / 100, 
                        sourceColor.G * (100 - alpha) / 100 + rnd.Next(255) * alpha / 100, 
                        sourceColor.B * (100 - alpha) / 100 + rnd.Next(255) * alpha / 100
                        );
                    Mix.SetPixel(x, y, mixColor);
                }
            }

            return Mix;
        }

        static public Image Zooming(Image Source, float Aspect)
        {

            Bitmap result = new Bitmap((int)(Source.Width*Aspect), (int)(Source.Height * Aspect));
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(Source, 0, 0, result.Width, result.Height);
            }
            return result;
        }
    }
}
