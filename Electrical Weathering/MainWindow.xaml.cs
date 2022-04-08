using Microsoft.Win32;
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
    public partial class MainWindow : Window
    {
        BitmapSource SelectedBitmap;
        Stopwatch stopwatch = new();
        WeatheringMode Mode;
        WeatheringMachine WM = new();
        public MainWindow()
        {
            InitializeComponent();
            string path = $"ImageResources/Demo8.jpg";
            PreviewImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));

            Mode = WeatheringMode.Classic;
            ModeClassic.IsChecked = true;
            Slider_Scaling.ValueChanged += Slider_Scaling_ValueChanged;

            Random rand = new Random();
            //string path = $"ImageResources/Demo{rand.Next(1, 9)}.jpg";
            
            SelectedBitmap = (BitmapSource)PreviewImage.Source;
            CenterWindowOnScreen();
            Btn_Revert.IsEnabled = false;
            
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

        private void Generate()
        {
            BitmapSource Result;
            stopwatch.Start();
            if (Mode == WeatheringMode.Classic)
            {
                
                Result = WM.WeatheringClassic((BitmapImage)SelectedBitmap, Slider_Noise.Value, Slider_Greening.Value, Slider_Compressing.Value, Slider_Scaling.Value);
                
            }
            else
            {
                Result = WM.WeatheringNG((BitmapImage)SelectedBitmap, Slider_Noise.Value, Slider_Greening.Value, Slider_Compressing.Value, Slider_Scaling.Value);
            }
            stopwatch.Stop();
            PreviewImage.Source = Result;
            ImageSizeText.Text = $"{Result.PixelWidth} x {Result.PixelHeight}" + $"  {stopwatch.ElapsedMilliseconds}ms";

            stopwatch.Reset();
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
                BitmapImage BitmapBuffer = new BitmapImage();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    BitmapBuffer.BeginInit();
                    BitmapBuffer.StreamSource = fs;
                    BitmapBuffer.CacheOption = BitmapCacheOption.OnLoad;
                    //BitmapBuffer.UriSource = new Uri(FilePathTextBox.Text, UriKind.RelativeOrAbsolute);
                    BitmapBuffer.EndInit();
                }

                SelectedBitmap = BitmapBuffer;
                PreviewImage.Source = SelectedBitmap;
                ImageSizeText.Text = $"{SelectedBitmap.PixelWidth} x {SelectedBitmap.PixelHeight}";

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
        }

        private void Slider_Noise_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextNoiseValue.Text = $"{Slider_Noise.Value:P0}";
        }

        private void Slider_Greening_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Mode == WeatheringMode.Classic)
            {
                TextGreeningValue.Text = $"{(int)(Slider_Greening.Value*100)} 迭代";
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

        private void Slider_Noise_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
            LoadImageFromPath(FilePathTextBox.Text);
            Slider_Noise.Value = 0.0;
            Slider_Greening.Value = 0.0;
            Slider_Compressing.Value = 0.0;
            Slider_Scaling.Value = 1.0;
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
                Generate();
            }
            catch  { }
        }

        private void ModeNG_Checked(object sender, RoutedEventArgs e)
        {
            Mode = WeatheringMode.NG;
            try
            {
                Generate();
            }
            catch  { }
        }
    }
}
