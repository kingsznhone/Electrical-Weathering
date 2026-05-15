using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Electrical_Weathering;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly WeatheringMachine _wm;
    private readonly Stopwatch _stopwatch = new();
    private readonly Random _rand = new();
    private readonly Lock _generateLock = new();
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private double _green;

    [ObservableProperty]
    private double _compressing;

    [ObservableProperty]
    private double _scaling = 1d;

    [ObservableProperty]
    private string _imageInfo = string.Empty;

    [ObservableProperty]
    private bool _low_Checked;

    [ObservableProperty]
    private bool _medium_Checked;

    [ObservableProperty]
    private bool _high_Checked;

    [ObservableProperty]
    private bool _custom_Checked;

    [ObservableProperty]
    private ImageSource? _previewImage;

    [ObservableProperty]
    private ImageSource? _selectedBitmap;

    [ObservableProperty]
    private string? _filePath;

    public MainWindowViewModel(WeatheringMachine wm)
    {
        _wm = wm;
        ApplicationTitle = "表情包电子包浆器";
        var initialBitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/Demo{_rand.Next(1, 8)}.jpg"));
        SelectedBitmap = initialBitmap;
        PreviewImage = SelectedBitmap;
        ImageInfo = $"{initialBitmap.PixelWidth} x {initialBitmap.PixelHeight}";
        Scaling = 1d;
    }

    [RelayCommand]
    private void SelectFileClick()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "All Image Files|*.bmp;*.jpeg;*.jpg;*.png"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            LoadFromPath(openFileDialog.FileName);
        }
    }

    [RelayCommand]
    private void LoadFromPath(string path)
    {
        FilePath = path;
        LoadImageFromPath(path);
    }

    private void LoadImageFromPath(string path)
    {
        try
        {
            var buffer = new BitmapImage();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                buffer.BeginInit();
                buffer.StreamSource = fs;
                buffer.CacheOption = BitmapCacheOption.OnLoad;
                buffer.EndInit();
            }
            SelectedBitmap = buffer;
            ResetClick();
        }
        catch (Exception ex)
        {
            var msg = new Wpf.Ui.Controls.MessageBox
            {
                Title = "就你那破图还想电子包浆？",
                Content = $"{ex.Message}",
                CloseButtonText = "OK",
                CloseButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Primary
            };
            msg.ShowDialogAsync();
        }
    }

    [RelayCommand]
    private void ResetClick()
    {
        PreviewImage = SelectedBitmap;
        if (PreviewImage is BitmapImage bmp)
            ImageInfo = $"{bmp.PixelWidth} x {bmp.PixelHeight}";

        Low_Checked = false;
        Medium_Checked = false;
        High_Checked = false;
        Custom_Checked = false;

        Green = 0;
        Compressing = 0;
        Scaling = 1;
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task SliderMouseUp()
    {
        Custom_Checked = true;
        await Generate();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task PresetLowClick()
    {
        Green = 0.15;
        Compressing = 0.05;
        await Generate();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task PresetMediumClick()
    {
        Green = 0.30;
        Compressing = 0.1;
        await Generate();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task PresetHighClick()
    {
        Green = 0.45;
        Compressing = 0.25;
        await Generate();
    }

    private async Task Generate()
    {
        CancellationToken token;
        lock (_generateLock)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            token = _cts.Token;
        }

        _stopwatch.Restart();

        var param = new WeatheringParam
        {
            Green = Green,
            Quality = Compressing,
            AspectRatio = Scaling,
        };

        if (SelectedBitmap is not BitmapImage bitmapImage)
            return;

        using Mat source = bitmapImage.ToMat();
        PreviewImage = SelectedBitmap;

        try
        {
            // Scaling is independent of weathering — apply once up front
            using Mat scaledSource = param.AspectRatio != 1.0
                ? await Task.Run(() => source.Resize(new Size(0, 0), param.AspectRatio, param.AspectRatio, InterpolationFlags.Area), token)
                : source.Clone();

            int greenIterations = Math.Max(0, (int)Math.Round(param.Green * 100, MidpointRounding.AwayFromZero));
            int iterations = Math.Max(greenIterations, HasSingleSkiaPassWork(param) ? 1 : 0);

            if (iterations == 0)
            {
                // Only scaling was requested — display the scaled result directly
                var wb = scaledSource.ToWriteableBitmap();
                PreviewImage = wb;
                ImageInfo = $"{wb.PixelWidth} x {wb.PixelHeight} {_stopwatch.ElapsedMilliseconds}ms";
            }
            else
            {
                Mat? ownedBuffer = null;
                Mat sourceBuffer = scaledSource;
                WriteableBitmap? writeableBitmap = null;
                try
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        // Green is passed as a flag (> 0 means apply one greening pass this iteration)
                        var iterParam = param with { Green = i < greenIterations ? 1.0 : 0.0 };
                        Mat? result = null;
                        try
                        {
                            result = await Task.Run(() => _wm.WeatheringSkia(sourceBuffer, iterParam), token);

                            // Reuse WriteableBitmap to avoid repeated LOH allocations each iteration
                            if (writeableBitmap is null ||
                                writeableBitmap.PixelWidth  != result.Width ||
                                writeableBitmap.PixelHeight != result.Height)
                            {
                                writeableBitmap = result.ToWriteableBitmap();
                            }
                            else
                            {
                                WriteableBitmapConverter.ToWriteableBitmap(result, writeableBitmap);
                            }
                            PreviewImage = writeableBitmap;
                            ImageInfo = $"{writeableBitmap.PixelWidth} x {writeableBitmap.PixelHeight} {i + 1}/{iterations}it {_stopwatch.ElapsedMilliseconds}ms";

                            ownedBuffer?.Dispose();
                            ownedBuffer = result;
                            sourceBuffer = result;
                            result = null;
                        }
                        finally
                        {
                            result?.Dispose();
                        }
                    }
                }
                finally
                {
                    ownedBuffer?.Dispose();
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Task was cancelled — no action needed
        }
        finally
        {
            _stopwatch.Stop();
        }
    }

    private static bool HasSingleSkiaPassWork(WeatheringParam param) =>
        param.Quality != 0.0 || param.AspectRatio != 1.0;

    [RelayCommand]
    private void SaveClick()
    {
        try
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Files|*.jpg|PNG Files|*.png|Bitmap Files|*.bmp"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                if (PreviewImage is not BitmapSource buffer)
                    return;

                using var fileStream = new FileStream(path, FileMode.Create);
                BitmapEncoder encoder = Path.GetExtension(path).ToLower() switch
                {
                    ".jpg" => new JpegBitmapEncoder(),
                    ".png" => new PngBitmapEncoder(),
                    ".bmp" => new BmpBitmapEncoder(),
                    _ => new JpegBitmapEncoder(),
                };
                encoder.Frames.Add(BitmapFrame.Create(buffer));
                encoder.Save(fileStream);
            }
        }
        catch
        {
            var msg = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Excuse me?",
                Content = "保存失败，请向作者反馈",
                CloseButtonText = "OK",
                CloseButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Primary
            };
            msg.ShowDialogAsync();
        }
    }
}
