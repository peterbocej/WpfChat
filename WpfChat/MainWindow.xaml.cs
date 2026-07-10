using System.Windows;

using WpfChat.ViewModel;

namespace WpfChat;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private MainWindowVM ViewModel => (MainWindowVM)DataContext;

    private async void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.Save();
        await ViewModel.Disconnect();
    }

    private async void btSend_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SendMessage();
    }

    private async void btConnect_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.Connect();
    }

    private async void btDisconnect_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.Disconnect();
    }

    private async void btRefresh_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.Refresh();
    }
}