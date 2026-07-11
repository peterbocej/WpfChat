using Microsoft.EntityFrameworkCore;

using WpfChat.Domain.Settings;
using WpfChat.WebApi.Data;
using WpfChat.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Logging.AddConsole();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("MySql-chat")!);
}, ServiceLifetime.Scoped);
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

var chatSettings = new ChatSettings();
builder.Configuration.GetSection("Chat").Bind(chatSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
