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

    private void btConnect_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Connect();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        gdMessages.ScrollIntoView(gdMessages.Items.Cast<object>().Last());
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.Save();
    }
}