using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Electrical_Weathering;

public partial class MainWindow : FluentWindow
{
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        InitializeComponent();
        SystemThemeWatcher.Watch(this);
    }

    private void FilePath_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
        {
            _viewModel.LoadFromPathCommand.Execute(files[0]);
        }
    }

    private void Window_Closed(object? sender, EventArgs e)
    {
        // WPF exits naturally when the main window closes
    }
}
