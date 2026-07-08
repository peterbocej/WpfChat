using System.Windows;

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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Login();
    }
}