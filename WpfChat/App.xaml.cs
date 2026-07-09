using System.Windows;

using Microsoft.Extensions.DependencyInjection;

namespace WpfChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; set; } = default!;
        public static T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
