using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Electrical_Weathering
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private WeatheringMachine WM;
        public Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private Stopwatch stopwatch = new();
        private Random rand = new Random();

        public Task WeatheringTask { get; set; } = null;

        private string _filePath;

        [ObservableProperty]
        private string _applicationTitle;

        [ObservableProperty]
        private WeatheringMode _mode = WeatheringMode.SKIA;

        [ObservableProperty]
        private double _noise = 0;

        [ObservableProperty]
        private double _green = 0;

        [ObservableProperty]
        private double _compressing = 0;

        [ObservableProperty]
        private double _scaling = 1d;

        [ObservableProperty]
        private string _imageInfo;

        [ObservableProperty]
        private bool _low_Checked = false;

        [ObservableProperty]
        private bool _medium_Checked = false;

        [ObservableProperty]
        private bool _high_Checked = false;

        [ObservableProperty]
        private bool _custom_Checked = false;

        [ObservableProperty]
        private bool _watermark_Available;

        [ObservableProperty]
        private bool _watermark_Checked = false;

        private ImageSource _previewImage;
        private ImageSource _selectedBitmap;

        public ImageSource PreviewImage
        { get => _previewImage; set { SetProperty(ref _previewImage, value); } }

        public ImageSource SelectedBitmap
        { get => _selectedBitmap; set { SetProperty(ref _selectedBitmap, value); } }

        public string FilePath
        {
            get => _filePath;
            set { SetProperty(ref _filePath, value); LoadImageFromPath(FilePath); }
        }

        public MainWindowViewModel(WeatheringMachine _wm)
        {
            WM = _wm;
            ApplicationTitle = "表情包电子包浆器";
            SelectedBitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/Demo{rand.Next(1, 9)}.jpg"));
            Watermark_Available = ((BitmapImage)SelectedBitmap).PixelWidth >= 240 && ((BitmapImage)SelectedBitmap).PixelHeight >= 120;
            PreviewImage = SelectedBitmap;
            ImageInfo = $"{((BitmapImage)PreviewImage).PixelWidth} x {((BitmapImage)PreviewImage).PixelHeight}";

            Mode = WeatheringMode.SKIA;
            Scaling = 1d;

        }

        [RelayCommand]
        public async Task ModeSwitch()
        {
            await Generate();
        }

        [RelayCommand]
        private void SelectFileClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Image Files|*.bmp;*.jpeg;*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        private void LoadImageFromPath(string path)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    BitmapImage Buffer = new BitmapImage();
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        Buffer.BeginInit();
                        Buffer.StreamSource = fs;
                        Buffer.CacheOption = BitmapCacheOption.OnLoad;
                        Buffer.EndInit();
                    }

                    SelectedBitmap = Buffer;
                    Watermark_Available = ((BitmapImage)SelectedBitmap).PixelWidth >= 240 && ((BitmapImage)SelectedBitmap).PixelHeight >= 120;
                    ResetClick();
                }
            }
            catch (Exception ex)
            {
                Wpf.Ui.Controls.MessageBox msg = new Wpf.Ui.Controls.MessageBox()
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
            ImageInfo = $"{((BitmapImage)PreviewImage).PixelWidth} x {((BitmapImage)PreviewImage).PixelHeight}";

            Low_Checked = false;
            Medium_Checked = false;
            High_Checked = false;
            Custom_Checked = false;

            Noise = 0;
            Green = 0;
            Compressing = 0;
            Scaling = 1;

            Watermark_Checked = false;
        }

        [RelayCommand]
        private async Task SliderMouseUp()
        {
            Custom_Checked = true;
            await Generate();
        }

        [RelayCommand]
        private async Task SliderMouseDown()
        {
            Custom_Checked = true;
            await Generate();
        }

        [RelayCommand]
        private async Task PresetLowClick()
        {
            Noise = 0;
            Green = 0.05;
            Compressing = 0.25;
            await Generate();
        }

        [RelayCommand]
        private async Task PresetMediumClick()
        {
            Noise = 0.02;
            Green = 0.1;
            Compressing = 0.5;
            await Generate();
        }

        [RelayCommand]
        private async Task PresetHighClick()
        {
            Noise = 0.05;
            Green = 0.15;
            Compressing = 0.75;
            await Generate();
        }

        [RelayCommand]
        private async Task WatermarkClick()
        {
            await Generate();
        }

        private async Task Generate()
        {
            if ( WeatheringTask != null&& !WeatheringTask.IsCompleted )
            {
                return;
            }
            stopwatch.Restart();
            WeatheringParam param = new WeatheringParam()
            {
                Noise = Noise,
                Green = Green,
                Quality = Compressing,
                AspectRatio = Scaling,
            };

            Mat Source = ((BitmapImage)SelectedBitmap).ToMat();
            Application.Current.Dispatcher.Invoke(() =>
            {
                PreviewImage = SelectedBitmap;
            });

            if (Mode == WeatheringMode.SKIA)
            {
                WeatheringTask = Task.Run(() =>
                {
                    var sourceBuffer = Source;
                    for (int i = 0; i < param.Green * 100; i++)
                    {
                        var Result = WM.WeatheringSkia(sourceBuffer, param, Watermark_Checked);
                        sourceBuffer = Result.Clone();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var writeableBitmap = Result.ToWriteableBitmap();
                            PreviewImage = writeableBitmap;
                            ImageInfo = $"{writeableBitmap.PixelWidth} x {writeableBitmap.PixelHeight} {stopwatch.ElapsedMilliseconds}ms";
                        });
                    }
                });
                await WeatheringTask;
                //Result = await Task.Run(() => WM.WeatheringSkia(Source, param, Watermark_Checked));
            }
            else
            {
                var Result = await Task.Run(() => WM.WeatheringCV(Source, param, Watermark_Checked));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var writeableBitmap = Result.ToWriteableBitmap();
                    PreviewImage = writeableBitmap;
                    ImageInfo = $"{writeableBitmap.PixelWidth} x {writeableBitmap.PixelHeight} {stopwatch.ElapsedMilliseconds}ms";
                });
            }
            stopwatch.Stop();
            

            return;
        }

        [RelayCommand]
        private void SaveClick()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Files|*.jpg|PNG Files|*.png|Bitmap Files|*.bmp";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    BitmapSource buffer = (BitmapSource)PreviewImage;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        BitmapEncoder encoder;

                        switch (Path.GetExtension(path).ToLower())
                        {
                            case ".jpg":
                                encoder = new JpegBitmapEncoder();
                                break;

                            case ".png":
                                encoder = new PngBitmapEncoder();
                                break;

                            case ".bmp":
                                encoder = new BmpBitmapEncoder();
                                break;

                            default:
                                encoder = new JpegBitmapEncoder();
                                break;
                        }
                        encoder.Frames.Add(BitmapFrame.Create(buffer));
                        encoder.Save(fileStream);
                        fileStream.Close();
                    }
                }
            }
            catch
            {
                Wpf.Ui.Controls.MessageBox msg = new Wpf.Ui.Controls.MessageBox()
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
}