using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WpfChat.ViewModel;

namespace WpfChat;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ConfigureServices();
        var app = new App();
        var win = App.GetRequiredService<MainWindow>();
        app.Run(win);
    }

    private static void ConfigureServices()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var services = new ServiceCollection();

        services.AddSingleton<IConfigurationRoot>(config);
        
        services.AddSingleton(typeof(MainWindow));

        services.AddScoped<IBaseViewModel, BaseViewModel>();
        services.AddScoped<IMainViewModel, MainViewModel>();

        App.ServiceProvider = services.BuildServiceProvider();
    }
}
