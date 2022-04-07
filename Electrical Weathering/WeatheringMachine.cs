using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Windows.Media.Imaging;
namespace Electrical_Weathering
{
    public static class WeatheringMachine
    {
        //public WeatheringMachine() { }

        public static BitmapSource Generate(BitmapSource SourceImage, double iN, double iG, double iQ, double AspectRatio)
        {
            Mat SourceMat = BitmapSourceConverter.ToMat(SourceImage);

            if (iN != 0)
                Noising(SourceMat, iN);

            if (iG != 0)
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

        private static void Noising(Mat SourceMat, double intensity)
        {
            using (Mat NoiseMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 0, 0, 255)))
            {
                Cv2.Randn(NoiseMat, new Scalar(0, 0, 0, 255), new Scalar(255, 255, 255, 255));
                Cv2.AddWeighted(SourceMat, 1 - intensity, NoiseMat, intensity, 0, SourceMat);
            }
        }

        private static void Greening(Mat SourceMat, double intensity)
        {
            using (Mat GreenMat = new Mat(SourceMat.Height, SourceMat.Width, MatType.CV_8UC4, new Scalar(0, 255, 0, 255)))
            {
                Cv2.AddWeighted(SourceMat, 1 - intensity, GreenMat, intensity, 0, SourceMat);
            }
        }

    }
}
