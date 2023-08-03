using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Electrical_Weathering
{
    public enum WeatheringMode
    {
        Classic,
        NG
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private BitmapSource SelectedBitmap;
        private Stopwatch stopwatch = new();
        private WeatheringMode Mode;
        private WeatheringMachine WM = new();
        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            string path = $"Resources/Demo{rand.Next(1, 9)}.jpg";

            PreviewImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            SelectedBitmap = (BitmapSource)PreviewImage.Source;

            Slider_Scaling.ValueChanged += Slider_Scaling_ValueChanged;

            Mode = WeatheringMode.Classic;
            ModeClassic.IsChecked = true;
            Btn_Revert.IsEnabled = true;
            CenterWindowOnScreen();
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private async void Generate()
        {
            BitmapSource Result;
            stopwatch.Restart();
            WeatheringParam param = new WeatheringParam()
            {
                Noise = Slider_Noise.Value,
                Green = Slider_Greening.Value,
                Quality = Slider_Compressing.Value,
                AspectRatio = Slider_Scaling.Value,
            };

            Func<BitmapImage, Mat> ConvertToMatFunc = (source) => source.ToMat();

            if (Mode == WeatheringMode.Classic)
            {
                Result = WM.WeatheringSkia((BitmapImage)SelectedBitmap, param, Check_Watermark.IsChecked.Value);
            }
            else
            {
                Result = WM.WeatheringCV((BitmapImage)SelectedBitmap, param, Check_Watermark.IsChecked.Value);
            }
            stopwatch.Stop();
            PreviewImage.Source = Result;
            ImageSizeText.Text = $"{Result.PixelWidth} x {Result.PixelHeight}" + $"  {stopwatch.ElapsedMilliseconds}ms";
        }

        private void FileSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Image Files|*.bmp;*.jpeg;*.jpg;*.png";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    FilePathTextBox.Text = openFileDialog.FileName;
                    LoadImageFromPath(FilePathTextBox.Text);
                    Btn_Revert.IsEnabled = true;
                }
            }
            catch
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?", MessageBoxButton.OK);
            }
        }

        private void LoadImageFromPath(string path)
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
                PreviewImage.Source = SelectedBitmap;
                ImageSizeText.Text = $"{SelectedBitmap.PixelWidth} x {SelectedBitmap.PixelHeight}";

                Check_Watermark.IsEnabled = SelectedBitmap.PixelWidth >= 240 && SelectedBitmap.PixelHeight >= 120;

                ControlReset();
            }
        }

        private void ControlReset()
        {
            RB_Low.IsChecked = false;
            RB_Medium.IsChecked = false;
            RB_High.IsChecked = false;
            RB_Custom.IsChecked = false;

            Slider_Noise.Value = 0;
            Slider_Greening.Value = 0;
            Slider_Compressing.Value = 0;
            Slider_Scaling.Value = 1;

            Check_Watermark.IsChecked = false;
        }

        private void Slider_Noise_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextNoiseValue.Text = $"{Slider_Noise.Value:P0}";
        }

        private void Slider_Greening_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Mode == WeatheringMode.Classic)
            {
                TextGreeningValue.Text = $"{(int)(Slider_Greening.Value * 100)} 迭代";
            }
            else
            {
                TextGreeningValue.Text = $"{Slider_Greening.Value:P0}";
            }
        }

        private void Slider_Compressing_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextCompressingValue.Text = $"{Slider_Compressing.Value:P0}";
        }

        private void Slider_Scaling_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TextScalingValue.Text = $"{Slider_Scaling.Value:P0}";
            }
            catch (NullReferenceException) { }
        }

        private void RB_Low_Click(object sender, RoutedEventArgs e)
        {
            Slider_Noise.Value = 0;
            Slider_Greening.Value = 0.05;
            Slider_Compressing.Value = 0.8;
            Generate();
        }

        private void RB_Medium_Click(object sender, RoutedEventArgs e)
        {
            Slider_Noise.Value = 0.02;
            Slider_Greening.Value = 0.1;
            Slider_Compressing.Value = 0.85;
            Generate();
        }

        private void RB_High_Click(object sender, RoutedEventArgs e)
        {
            Slider_Noise.Value = 0.05;
            Slider_Greening.Value = 0.15;
            Slider_Compressing.Value = 0.90;
            Generate();
        }

        private void Slider_Noise_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RB_Custom.IsChecked = true;
        }

        private void Slider_Greening_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RB_Custom.IsChecked = true;
        }

        private void Slider_Compressing_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RB_Custom.IsChecked = true;
        }

        private void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Generate();
        }

        private void FilePathTextBox_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                FilePathTextBox.Text = files[0];
                LoadImageFromPath(FilePathTextBox.Text);
            }
        }

        private void FilePathTextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG File|jpeg;*.jpg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    BitmapSource buffer = (BitmapSource)PreviewImage.Source;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(buffer));
                        encoder.Save(fileStream);
                        fileStream.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("就你那破图还想电子包浆？", "保存失败");
            }
        }

        private void Btn_Revert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadImageFromPath(FilePathTextBox.Text);
            }
            catch
            {
            }
            Slider_Noise.Value = 0.0;
            Slider_Greening.Value = 0.0;
            Slider_Compressing.Value = 0.0;
            Slider_Scaling.Value = 1.0;

            Generate();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = e.Uri.ToString();
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void ModeClassic_Checked(object sender, RoutedEventArgs e)
        {
            Mode = WeatheringMode.Classic;
            try
            {
                TextGreeningValue.Text = $"{(int)(Slider_Greening.Value * 100)} 迭代";
                Generate();
            }
            catch { }
        }

        private void ModeNG_Checked(object sender, RoutedEventArgs e)
        {
            Mode = WeatheringMode.NG;
            try
            {
                TextGreeningValue.Text = $"{Slider_Greening.Value:P0}";
                Generate();
            }
            catch { }
        }

        private void Slider_KeyUp(object sender, KeyEventArgs e)
        {
            Generate();
        }

        private void Check_Watermark_Click(object sender, RoutedEventArgs e)
        {
            Generate();
        }
    }
}