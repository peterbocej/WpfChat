using System.Windows;
using System.Windows.Controls;

using WpfChat.ViewModel;

namespace WpfChat;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowVM _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowVM();
        DataContext = _viewModel;
    }

    private void btConnect_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Connect();
    }

    private void gdMessages_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        e.Handled = true;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        gdMessages.ScrollIntoView(gdMessages.Items.Cast<object>().Last());
    }
}