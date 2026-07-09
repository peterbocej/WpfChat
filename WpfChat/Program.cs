using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using WpfChat.Data;
using WpfChat.Repositories;

namespace WpfChat;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var app = new App();

        app.Run(new MainWindow());
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: "_log/chat-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();

        services.AddSingleton<IConfigurationRoot>(config);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySQL(config.GetConnectionString("mysql-chat")!);
            var loggerFactory = LoggerFactory.Create(logbuild =>
            {
                logbuild
                  .AddSerilog(dispose: true)
                  .AddFilter((category, level) =>
                     category == DbLoggerCategory.Database.Command.Name &&
                     level >= LogLevel.Information);
            });
            options.UseLoggerFactory(loggerFactory);
        }, ServiceLifetime.Singleton);

        services.AddTransient<IMessagesRepository, MessagesRepository>();

        App.ServiceProvider = services.BuildServiceProvider();
    }
}
