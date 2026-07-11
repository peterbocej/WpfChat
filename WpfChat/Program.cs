using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WpfChat.Domain.Settings;
using WpfChat.Services;

namespace WpfChat;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddLogging(conf => conf.AddDebug());
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        ChatSettings chatSettings = new();
        configuration.GetSection("Chat").Bind(chatSettings);
        services.AddSingleton(chatSettings);

        services.AddSingleton<IWebApiService, WebApiService>();

        App.ServiceProvider = services.BuildServiceProvider();

        var app = new App();
        app.Run(new MainWindow());
    }
}
