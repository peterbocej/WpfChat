using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WpfChat;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ConfigureServices();
        var app = new App();

        app.Run(new MainWindow());
    }

    private static void ConfigureServices()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var services = new ServiceCollection();

        services.AddSingleton<IConfigurationRoot>(config);

        App.ServiceProvider = services.BuildServiceProvider();
    }
}
