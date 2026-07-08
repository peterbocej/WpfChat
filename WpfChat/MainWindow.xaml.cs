using System.Windows;

namespace WpfChat;

public interface IMainWindow
{ }
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }
}