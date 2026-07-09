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
    }

    private void btRefresh_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Refresh();
    }

    private void btSend_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SendMessage();
    }
}