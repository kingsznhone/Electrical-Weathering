using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.IO.Pipes;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
namespace Electrical_Weathering_NG
{
    public struct FormatSpecParam
    {
        public ImwriteFlags Flag;
        public byte Value;
    }

    public class WeatheringMachine
    {

        public WeatheringMachine()
        {
        }

        public BitmapSource Generate(BitmapSource SourceImage, double iN,double iG,double iQ,double AspectRatio)
        {
            Mat SourceMat = BitmapSourceConverter.ToMat(SourceImage);
                
            if (iN!=0)
                Noising(SourceMat,iN);

            if (iG !=0)
                Greening(SourceMat, iG);

            if (AspectRatio != 1)
            {
                OpenCvSharp.Size size = new OpenCvSharp.Size(0, 0);
                SourceMat = SourceMat.Resize(size, AspectRatio, AspectRatio, InterpolationFlags.Area);
            }

            if (iQ != 0)
            {
                ImageEncodingParam param = new ImageEncodingParam(ImwriteFlags.JpegQuality, (byte)Convert.ToInt32((1 - iQ) * 100));
                byte[] buffer = SourceMat.ImEncode(".jpg", param);
                SourceMat = Mat.ImDecode(buffer);
            }

            return BitmapSourceConverter.ToBitmapSource(SourceMat);

        }

        private void Noising(Mat SourceMat, double intensity)
        {
            using (Mat NoiseMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 0, 0, 255)))
            {
                Cv2.Randn(NoiseMat, new Scalar(0, 0, 0, 255), new Scalar(255, 255, 255, 255));
                Cv2.AddWeighted(SourceMat, 1 - intensity, NoiseMat, intensity, 0, SourceMat);
            }
        }

        private void Greening(Mat SourceMat, double intensity)
        {
            using (Mat GreenMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 255, 0, 255)))
            {
                Cv2.AddWeighted(SourceMat, 1 - intensity, GreenMat, intensity, 0, SourceMat);
            }
        }

        
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage BMPImage;

            try
            {
                BMPImage = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return BMPImage;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static BitmapSource Bitmap2BitmapSource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap BitmapSource2Bitmap(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }


    }
}
