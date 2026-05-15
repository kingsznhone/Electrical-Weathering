using OpenCvSharp;
using System;
using System.Runtime.CompilerServices;

namespace Electrical_Weathering;

public readonly record struct WeatheringParam
{
    public double Green { get; init; }
    public double Quality { get; init; }
    public double AspectRatio { get; init; }
}

public class WeatheringMachine
{
    // RGB → Y (luma) coefficients, fixed-point scaled by 2^8
    private const int CYR = 77;   // 0.299
    private const int CYG = 150;  // 0.587
    private const int CYB = 29;   // 0.114

    // RGB → Cb/Cr coefficients, fixed-point scaled by 2^8
    private const int CUR = -43;  // -0.16874
    private const int CUG = -85;  // -0.33126
    private const int CUB = 128;  // 0.5

    private const int CVR = 128;  // 0.5
    private const int CVG = -107; // -0.41869
    private const int CVB = -21;  // -0.08131

    private const int CSHIFT = 8;

    // YCbCr → RGB back-conversion coefficients, fixed-point scaled by 2^16
    private const int YUV_B_CR  =  116130; //  1.77200 * 65536
    private const int YUV_G_CR  =  -22553; // -0.34414 * 65536
    private const int YUV_G_CB  =  -46802; // -0.71414 * 65536
    private const int YUV_R_CB  =   91881; //  1.40200 * 65536

    public WeatheringMachine()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ClampUnsign(int x) => Math.Clamp(x, 0, 255);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ClampSign(int x)   => Math.Clamp(x, -128, 127);

    // Sequential unsafe scan — much faster than At<Vec3b>/Set per pixel
    // Single pass per call; iteration count is controlled by the caller (ViewModel).
    private static unsafe void ApplyGreening(Mat sourceMat)
    {
        int width    = sourceMat.Width;
        int height   = sourceMat.Height;
        int channels = sourceMat.Channels();
        long step     = sourceMat.Step();
        byte* dataPtr = sourceMat.DataPointer;

        for (int y = 0; y < height; y++)
        {
            byte* row = dataPtr + y * step;
            for (int x = 0; x < width; x++)
                SkiaYUV(row + x * channels);
        }
    }

    public Mat WeatheringSkia(Mat source, WeatheringParam param)
    {
        Mat sourceMat = source.Clone();

        // Some memes only have one color channel
        if (sourceMat.Channels() == 1)
        {
            Mat grayMat = sourceMat;
            sourceMat = sourceMat.CvtColor(ColorConversionCodes.GRAY2BGRA);
            grayMat.Dispose();
        }

        // Greening
        if (param.Green > 0)
            ApplyGreening(sourceMat);

        if (param.Quality != 0)
        {
            Mat prev = sourceMat;
            sourceMat = Compressing(sourceMat, param.Quality);
            prev.Dispose();
            Mat compressed = sourceMat;
            sourceMat = sourceMat.CvtColor(ColorConversionCodes.BGR2BGRA);
            compressed.Dispose();
        }

        return sourceMat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SkiaYUV(byte* p)
    {
        // Forward: BGR → YCbCr (fixed-point, shift 8)
        int Y  = ClampUnsign(((CYR * p[2] + CYG * p[1] + CYB * p[0]) >> CSHIFT) - 1);
        int Cr = ClampSign  (((CUR * p[2] + CUG * p[1] + CUB * p[0]) >> CSHIFT) - 1);
        int Cb = ClampSign  (((CVR * p[2] + CVG * p[1] + CVB * p[0]) >> CSHIFT) - 1);

        // Inverse: YCbCr → BGR (fixed-point, shift 16)
        int YY1 = Y << 16;
        p[0] = (byte)ClampUnsign((YY1 + YUV_B_CR * Cr) >> 16);                        // B
        p[1] = (byte)ClampUnsign((YY1 + YUV_G_CR * Cr + YUV_G_CB * Cb) >> 16);        // G
        p[2] = (byte)ClampUnsign((YY1 + YUV_R_CB * Cb) >> 16);                        // R
    }

    private static Mat Compressing(Mat sourceMat, double intensity)
    {
        var param = new ImageEncodingParam(ImwriteFlags.JpegQuality,
            checked((byte)Convert.ToInt32((1.0 - intensity) * 100.0)));
        byte[] buffer = sourceMat.ImEncode(".jpg", param);
        return Mat.ImDecode(buffer);
    }
}
