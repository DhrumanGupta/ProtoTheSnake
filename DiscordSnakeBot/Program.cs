using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using DiscordSnakeBot.Infrastructure;
using DiscordSnakeBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordSnakeBot
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();
        
                    x.AddConfiguration(configuration);
                })
                .ConfigureLogging(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 200,
                        GatewayIntents = GatewayIntents.Guilds
                                         | GatewayIntents.GuildMembers
                                         | GatewayIntents.GuildMessageReactions
                                         | GatewayIntents.GuildMessages
                                         | GatewayIntents.GuildWebhooks
                    };
                    
                    config.Token = context.Configuration["token"];
                })
                .UseCommandService((context, config) =>
                {
                    config = new CommandServiceConfig()
                    {
                        CaseSensitiveCommands = false,
                        LogLevel = LogSeverity.Info
                    };
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<CommandHandler>()
                        .AddDbContext<SnakeBotContext>()
                        .AddSingleton<Servers>()
                        .AddSingleton<Games>();
                })
                .UseConsoleLifetime();
        
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}