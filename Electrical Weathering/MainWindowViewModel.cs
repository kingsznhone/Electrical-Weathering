using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Electrical_Weathering
{
    public class MainWindowViewModel : ObservableObject
    {
        private WeatheringMachine WM;
        public Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private Stopwatch stopwatch = new();
        private Random rand = new Random();
        private string applicationTitle;
        private string filePath;
        private WeatheringMode mode;
        private double noise;
        private double green;
        private double compressing;
        private double scaling;
        private string imageInfo;
        private bool low_Checked;
        private bool medium_Checked;
        private bool high_Checked;
        private bool custom_Checked;
        private bool watermark_Available;
        private bool watermark_Checked;
        private ImageSource previewImage;
        private ImageSource selectedBitmap;

        public ImageSource PreviewImage
        { get => previewImage; set { SetProperty(ref previewImage, value); } }

        public ImageSource SelectedBitmap
        { get => selectedBitmap; set { SetProperty(ref selectedBitmap, value); } }

        public string FilePath
        {
            get => filePath;
            set { SetProperty(ref filePath, value); LoadImageFromPath(FilePath); }
        }

        public WeatheringMode Mode
        { get => mode; set { SetProperty(ref mode, value); } }

        public double Noise
        { get => noise; set { SetProperty(ref noise, value); } }

        public double Green
        { get => green; set { SetProperty(ref green, value); } }

        public double Compressing
        { get => compressing; set { SetProperty(ref compressing, value); } }

        public double Scaling
        { get => scaling; set { SetProperty(ref scaling, value); } }

        public string ImageInfo
        { get => imageInfo; set { SetProperty(ref imageInfo, value); } }

        public bool Low_Checked
        { get => low_Checked; set { SetProperty(ref low_Checked, value); } }

        public bool Medium_Checked
        { get => medium_Checked; set { SetProperty(ref medium_Checked, value); } }

        public bool High_Checked
        { get => high_Checked; set { SetProperty(ref high_Checked, value); } }

        public bool Custom_Checked
        { get => custom_Checked; set { SetProperty(ref custom_Checked, value); } }

        public bool Watermark_Available
        { get => watermark_Available; set { SetProperty(ref watermark_Available, value); } }

        public bool Watermark_Checked
        { get => watermark_Checked; set { SetProperty(ref watermark_Checked, value); } }

        public string ApplicationTitle
        { get => applicationTitle; set { SetProperty(ref applicationTitle, value); } }

        public ICommand SelectFileClickCommand { get; private set; }

        public ICommand ModeSwitchCommand { get; private set; }

        public ICommand PresetLowClickCommand { get; private set; }

        public ICommand PresetHighClickCommand { get; private set; }

        public ICommand PresetMediumClickCommand { get; private set; }

        public ICommand ResetClickCommand { get; private set; }

        public ICommand SaveClickCommand { get; private set; }

        public ICommand SliderMouseUpCommand { get; private set; }

        public ICommand SliderMouseDownCommand { get; private set; }

        public ICommand WatermarkCommand { get; private set; }

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

            ModeSwitchCommand = new AsyncRelayCommand(Generate);
            PresetLowClickCommand = new AsyncRelayCommand(SetLow);
            PresetMediumClickCommand = new AsyncRelayCommand(SetMedium);
            PresetHighClickCommand = new AsyncRelayCommand(SetHigh);
            SelectFileClickCommand = new RelayCommand(OpenFileDialog);
            SliderMouseUpCommand = new AsyncRelayCommand(SliderMouseUp);
            SliderMouseDownCommand = new AsyncRelayCommand(SliderMouseDown);
            WatermarkCommand = new AsyncRelayCommand(Watermark);
            ResetClickCommand = new RelayCommand(ControlReset);
            SaveClickCommand = new RelayCommand(Save);
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Image Files|*.bmp;*.jpeg;*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        private void ModeSwitch()
        {
            Generate();
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
                    ControlReset();
                }
            }
            catch
            {
                Wpf.Ui.Controls.MessageBox msg = new Wpf.Ui.Controls.MessageBox()
                {
                    Title = "Excuse me?",
                    Content = "就你那破图还想电子包浆？",
                    CloseButtonText = "OK",
                    CloseButtonAppearance = Wpf.Ui.Controls.ControlAppearance.Primary
                };
                msg.ShowDialogAsync();
            }
        }

        private void ControlReset()
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

        private async Task SliderMouseUp()
        {
            Custom_Checked = true;
            await Generate();
        }

        private async Task SliderMouseDown()
        {
            Custom_Checked = true;
            await Generate();
        }

        private async Task SetLow()
        {
            Noise = 0;
            Green = 0.05;
            Compressing = 0.8;
            await Generate();
        }

        private async Task SetMedium()
        {
            Noise = 0.02;
            Green = 0.1;
            Compressing = 0.85;
            await Generate();
        }

        private async Task SetHigh()
        {
            Noise = 0.05;
            Green = 0.15;
            Compressing = 0.90;
            await Generate();
        }

        private async Task Watermark()
        {
            await Generate();
        }

        private async Task Generate()
        {

            WriteableBitmap Result;
            stopwatch.Restart();
            WeatheringParam param = new WeatheringParam()
            {
                Noise = Noise,
                Green = Green,
                Quality = Compressing,
                AspectRatio = Scaling,
            };

            Func<BitmapImage, Mat> ConvertToMatFunc = (source) => source.ToMat();


            Mat Source = ((BitmapImage)SelectedBitmap).ToMat();

            if (Mode == WeatheringMode.SKIA)
            {
                Result = WM.WeatheringSkia(Source, param, Watermark_Checked);
            }
            else
            {
                Result = WM.WeatheringCV(Source, param, Watermark_Checked);
            }
            stopwatch.Stop();
            Application.Current.Dispatcher.Invoke(() =>
            {
                PreviewImage = Result;
            });
            PreviewImage = Result;

            ImageInfo = $"{Result.PixelWidth} x {Result.PixelHeight} {stopwatch.ElapsedMilliseconds}ms";

            return;
        }

        private void Save()
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