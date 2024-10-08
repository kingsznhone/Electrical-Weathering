﻿using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Electrical_Weathering
{
    public enum WeatheringMode
    {
        SKIA,
        OPENCV
    }

    public struct WeatheringParam
    {
        public double Noise;
        public double Green;
        public double Quality;
        public double AspectRatio;
    }

    public class WeatheringMachine
    {
        private const int CYR = 77; // 0.299
        private const int CYG = 150;   // 0.587
        private const int CYB = 29; // 0.114

        private const int CUR = -43;  // -0.16874
        private const int CUG = -85; // -0.33126
        private const int CUB = 128;  // 0.5

        private const int CVR = 128;  // 0.5
        private const int CVG = -107;// -0.41869
        private const int CVB = -21; // -0.08131

        private const int CSHIFT = 8;

        private Mat Watermark_Zhihu;
        private Mat Watermark_Sina;
        private Mat Watermark_Tieba;
        private Mat Watermark_Toutiao;

        public WeatheringMachine()
        {
            string path = "pack://application:,,,/Resources/Watermark_Zhihu.png";
            Watermark_Zhihu = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)).ToMat();
            path = "pack://application:,,,/Resources/Watermark_Weibo.png";
            Watermark_Sina = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)).ToMat();
            path = "pack://application:,,,/Resources/Watermark_Tieba.png";
            Watermark_Tieba = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)).ToMat();
            path = "pack://application:,,,/Resources/Watermark_Toutiao.png";
            Watermark_Toutiao = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)).ToMat();
        }

        private Func<int, int> ClampUnsign = (int x) => x >= 0 ? x <= 255 ? x : 255 : 0;

        private Func<int, int> ClampSign = (int x) => x < -128 ? -128 : x <= 127 ? x : 127;

        public WriteableBitmap WeatheringSkia(Mat source, WeatheringParam param, bool Watermark)
        {
            Mat SourceMat = source;

            //Some Meme only have one color channel.
            if (SourceMat.Channels() == 1)
            {
                SourceMat = SourceMat.CvtColor(ColorConversionCodes.GRAY2BGRA);
            }

            if (Watermark)
            {
                AddWatermark(ref SourceMat, Watermark_Zhihu);
                AddWatermark(ref SourceMat, Watermark_Sina);
                AddWatermark(ref SourceMat, Watermark_Toutiao);
                AddWatermark(ref SourceMat, Watermark_Tieba);
            }

            if (param.Noise != 0.0)
            {
                Noising(ref SourceMat, param.Noise);
            }

            //Greening
            for (int iter = 0; iter < param.Green * 100; iter++)
            {
                Parallel.For(0, SourceMat.Height, (y, state) =>
                {
                    for (int x = 0; x < SourceMat.Width; x++)
                    {
                        var p = SourceMat.Get<Vec3b>(y, x);
                        SkiaYUV(ref p);
                        SourceMat.Set(y, x, p);
                    }
                });
            }

            if (param.Quality != 0)
            {
                SourceMat = Compressing(SourceMat, param.Quality);
            }

            if (param.AspectRatio != 1)
            {
                SourceMat = SourceMat.Resize(new Size(0, 0), param.AspectRatio, param.AspectRatio, InterpolationFlags.Area);
            }

            return SourceMat.ToWriteableBitmap();
        }

        public WriteableBitmap WeatheringCV(Mat source, WeatheringParam param, bool Watermark)
        {
            Mat SourceMat = source;
            if (SourceMat.Type() == MatType.CV_8UC1)
            {
                SourceMat = SourceMat.CvtColor(ColorConversionCodes.GRAY2BGRA);
            }
            if (Watermark)
            {
                AddWatermark(ref SourceMat, Watermark_Zhihu);
                AddWatermark(ref SourceMat, Watermark_Sina);
                AddWatermark(ref SourceMat, Watermark_Toutiao);
                AddWatermark(ref SourceMat, Watermark_Tieba);
            }

            if (param.Noise != 0.0)
            {
                Noising(ref SourceMat, param.Noise);
            }
            if (param.Green != 0.0)
            {
                Greening(ref SourceMat, param.Green);
            }
            if (param.AspectRatio != 1.0)
            {
                SourceMat = SourceMat.Resize(new Size(0, 0), param.AspectRatio, param.AspectRatio, InterpolationFlags.Area);
            }
            if (param.Quality != 0.0)
            {
                SourceMat = Compressing(SourceMat, param.Quality);
            }
            return SourceMat.ToWriteableBitmap();
        }

        public void AddWatermark(ref Mat SourceMat, Mat Watermark)
        {
            Mat ROI = SourceMat.SubMat(new Rect(SourceMat.Width - Watermark.Width, SourceMat.Height - Watermark.Height, Watermark.Width, Watermark.Height));
            Vec4b vf;
            Vec3b vb;
            double alpha = 1;
            for (int r = 0; r < ROI.Rows; ++r)
            {
                for (int c = 0; c < ROI.Cols; ++c)
                {
                    vf = Watermark.Get<Vec4b>(r, c);
                    if (vf[3] > 0) // alpha channel > 0
                    {
                        // Blending
                        vb = ROI.Get<Vec3b>(r, c);
                        vb[0] = (byte)(alpha * vf[0] + (1 - alpha) * vb[0]);
                        vb[1] = (byte)(alpha * vf[1] + (1 - alpha) * vb[1]);
                        vb[2] = (byte)(alpha * vf[2] + (1 - alpha) * vb[2]);
                        ROI.Set(r, c, vb);
                    }
                }
            }
        }

        private void SkiaYUV(ref Vec3b p)
        {
            int Y = ClampUnsign(((CYR * p.Item2 + CYG * p.Item1 + CYB * p.Item0) >> CSHIFT) - 1);
            int Cr = ClampSign(((CUR * p.Item2 + CUG * p.Item1 + CUB * p.Item0) >> CSHIFT) - 1);
            int Cb = ClampSign(((CVR * p.Item2 + CVG * p.Item1 + CVB * p.Item0) >> CSHIFT) - 1);

            int YY1 = Y << 16;

            p.Item0 = Convert.ToByte(ClampUnsign((YY1 + 116130 * Cr) >> 16)); // B
            p.Item1 = Convert.ToByte(ClampUnsign((YY1 - 22553 * Cr - 46802 * Cb) >> 16)); ; // G
            p.Item2 = Convert.ToByte(ClampUnsign((YY1 + 91881 * Cb) >> 16));// R

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
                double DesiredAlpha = 1 - 0.5 * intensity; //[0.5,1]
                double DesiredBeta = 0.5 * Math.Sqrt(intensity); //[0,0.5]
                double DesiredGamma = -2.5 * intensity * 100;
                DesiredGamma = DesiredGamma >= -45 ? DesiredGamma : -45;// [-45,0]

                //Mix with 2 mat
                Cv2.AddWeighted(SourceMat, DesiredAlpha, GreenMat, DesiredBeta, DesiredGamma, SourceMat);

                //
                SourceMat.ConvertTo(SourceMat, SourceMat.Type(), 1 + 0.25 * intensity, -intensity * 128);

                //Lift G channel brightness
                Mat[] bgra;
                Cv2.Split(SourceMat, out bgra);
                //Cv2.Rectangle(bgra[3], new OpenCvSharp.Rect(0, 0, bgra[3].Width, bgra[3].Height), new Scalar(255), -1);
                bgra[1].ConvertTo(bgra[1], bgra[1].Type(), 1, 32 * intensity);
                Cv2.Merge(bgra, SourceMat);
            }
        }

        private Mat Compressing(Mat SourceMat, double intensity)
        {
            ImageEncodingParam param = new ImageEncodingParam(ImwriteFlags.JpegQuality, checked((byte)Convert.ToInt32((1.0 - intensity) * 100.0)));
            byte[] buffer = SourceMat.ImEncode(".jpg", param);
            return Mat.ImDecode(buffer);
        }
    }
}