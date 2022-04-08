using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
namespace Electrical_Weathering
{
    public class WeatheringMachine
    {
        private const int CYR = 77;

        private const int CYG = 150;

        private const int CYB = 29;

        private const int CUR = -43;

        private const int CUG = -85;

        private const int CUB = 128;

        private const int CVR = 128;

        private const int CVG = -107;

        private const int CVB = -21;

        private const int CSHIFT = 8;

        private Func<int, int> ClampRGB = (int x) => x >= 0 ? x <= 255 ? x : 255 : 0;

        private Func<int, int> ClampUV = (int x) => x < -128 ? -128 : x <= 127 ? x : 127;

        public BitmapSource WeatheringClassic(BitmapImage input, double iN, double iG, double iQ, double AspectRatio)
        {
            Mat SourceMat = input.ToMat();

            if (iN != 0.0)
            {
                Noising(ref SourceMat, iN);
            }

            for (int iter = 0; iter < iG*100; iter++) 
            { 
                Parallel.For(0, SourceMat.Height, (y, state) =>
                {
                    Parallel.For(0, SourceMat.Width, (x, state) =>
                    {
                        SourceMat.Set(y, x, FastYUV(SourceMat.Get<Vec3b>(y, x)));
                    });
                });
            }

            if (iQ != 0)
            {
                SourceMat = Compressing(SourceMat, iQ);
            }

            if (AspectRatio != 1)
            {
                OpenCvSharp.Size size = new OpenCvSharp.Size(0, 0);
                SourceMat = SourceMat.Resize(size, AspectRatio, AspectRatio, InterpolationFlags.Area);
            }
            
            return SourceMat.ToBitmapSource();

        }


        public BitmapSource WeatheringNG(BitmapSource SourceImage, double iN, double iG, double iQ, double AspectRatio)
        {
            Mat SourceMat = SourceImage.ToMat();
            if (iN != 0.0)
            {
                Noising(ref SourceMat, iN);
            }
            if (iG != 0.0)
            {
                Greening(ref SourceMat, iG);
            }
            if (AspectRatio != 1.0)
            {
                OpenCvSharp.Size size = new OpenCvSharp.Size(0, 0);
                SourceMat = SourceMat.Resize(size, AspectRatio, AspectRatio, InterpolationFlags.Area);
            }
            if (iQ != 0.0)
            {
                SourceMat = Compressing(SourceMat, iQ);
            }
            return SourceMat.ToBitmapSource();
        }

        private Vec3b FastYUV(Vec3b p)
        {

            int Y = ClampRGB((77 * unchecked((int)p.Item2) + 150 * unchecked((int)p.Item1) + 29 * unchecked((int)p.Item0) >> 8) - 1);
            int U = ClampUV((-43 * unchecked((int)p.Item2) + -85 * unchecked((int)p.Item1) + 128 * unchecked((int)p.Item0) >> 8) - 1);
            int V = ClampUV((128 * unchecked((int)p.Item2) + -107 * unchecked((int)p.Item1) + -21 * unchecked((int)p.Item0) >> 8) - 1);
            int YY1 = 65536 * Y;

            var newPixel =new Vec3b
            {
                Item0 = Convert.ToByte(ClampRGB(YY1 + 116130 * U >> 16)), // B
                Item1 = Convert.ToByte(ClampRGB(YY1 - 22553 * U - 46802 * V >> 16)), // G
                Item2 = Convert.ToByte(ClampRGB(YY1 + 91881 * V >> 16)) // R
            };
            return newPixel;

        }
        private void Noising(ref Mat SourceMat, double intensity)
        {
            using (Mat NoiseMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 0, 0, 255)))
            {
                Cv2.Randn(NoiseMat, new Scalar(0, 0, 0, 255), new Scalar(255, 255, 255, 255));
                Cv2.AddWeighted(SourceMat, 1 - intensity, NoiseMat, intensity, 0, SourceMat);
            }
        }

        private void Greening(ref Mat SourceMat, double intensity)
        {
            using (Mat GreenMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 255, 0, 255)))
            {
                Cv2.AddWeighted(SourceMat, 1 - intensity, GreenMat, intensity, 0, SourceMat);
                
                for(int y = 0;y< SourceMat.Height; y++)
                {
                    for (int x = 0; x < SourceMat.Width; x++)
                    {
                        SourceMat.Set(y, x, Delight(SourceMat.Get<Vec3b>(y, x),intensity));
                    }
                }
            }

            Vec3b Delight(Vec3b p,double intensity)
            {
                return new Vec3b
                {
                    Item0 = (byte)ClampRGB((int)(p.Item0 - (intensity * 64))),
                    Item1 = (byte)ClampRGB((int)(p.Item1 - (intensity * 64))),
                    Item2 = (byte)ClampRGB((int)(p.Item2 - (intensity * 64)))
                };
            }
        }


        private Mat Compressing(Mat SourceMat, double intensity)
        {
            ImageEncodingParam param = new ImageEncodingParam(ImwriteFlags.JpegQuality, checked((byte)Convert.ToInt32((1.0 - intensity) * 100.0)));
            byte[] buffer = SourceMat.ImEncode(".jpg", param);
            return Mat.ImDecode(buffer);
        }
        public Bitmap BitmapFromSource(BitmapImage bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapsource));
                encoder.Save(outStream);
                bitmap = new Bitmap(outStream);
                bitmap = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format32bppRgb);
            }
            return bitmap;
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0L;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                return result;
            }
        }

    }
}
