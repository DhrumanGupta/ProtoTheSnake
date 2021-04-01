using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DiscordSnakeBot.Services
{
    public class StartupService
    {
        public static IServiceProvider Provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
        {
            Provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;
        }

        public async Task StartAsync()
        {
            string token = _config["tokens:discord"];
        }
    }
}
