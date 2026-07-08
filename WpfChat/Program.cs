using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WpfChat.ViewModel;
using WpfChat.Windows;

namespace WpfChat;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ConfigureServices();
        var app = new App();

        var main = App.GetRequiredService<MainWindow>();
        app.Run(main);
    }

    private static void ConfigureServices()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var services = new ServiceCollection();

        services.AddSingleton<IConfigurationRoot>(config);

        services.AddScoped(typeof(MainWindow));
        services.AddScoped(typeof(LoginWindow));

        services.AddScoped<IBaseViewModel, BaseViewModel>();
        services.AddScoped<IMainViewModel, MainViewModel>();
        services.AddScoped<ILoginViewModel, LoginViewModel>();

        App.ServiceProvider = services.BuildServiceProvider();
    }
}
