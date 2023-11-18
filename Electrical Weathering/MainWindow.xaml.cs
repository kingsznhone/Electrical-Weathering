using System;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Electrical_Weathering
{
    public partial class MainWindow : FluentWindow
    {
        private MainWindowViewModel VM;

        public MainWindow(MainWindowViewModel _vm)
        {
            VM = _vm;
            DataContext = VM;
            InitializeComponent();
            SystemThemeWatcher.Watch(this);
        }

        private void FilePath_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                VM.FilePath = files[0];
            }
        }

        private void _this_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}