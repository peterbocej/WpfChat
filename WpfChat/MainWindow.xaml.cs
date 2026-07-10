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

    private void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.Save();
        ViewModel.Disconnect()
            .GetAwaiter()
            .GetResult();
    }

    private void btSend_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SendMessage()
            .GetAwaiter()
            .GetResult();
    }

    private void btConnect_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Connect()
            .GetAwaiter()
            .GetResult();
    }

    private void btDisconnect_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Disconnect()
            .GetAwaiter()
            .GetResult();
    }
}