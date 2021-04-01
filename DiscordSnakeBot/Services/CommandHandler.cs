using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordSnakeBot.Services
{
    public class CommandHandler
    {
        public static IServiceProvider Provider;
        public static DiscordSocketClient Discord;
        public static CommandService Commands;
        public static IConfigurationRoot Config;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
        {
            Provider = provider;
            Discord = discord;
            Commands = commands;
            Config = config;

            Discord.Ready += OnReady;
            Discord.MessageReceived += OnMessageReceived;
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {Discord.CurrentUser.Username}#{Discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null || msg.Author.IsBot) { return; }

            var context = new SocketCommandContext(Discord, msg);

            var pos = 0;
            if (msg.HasStringPrefix(Config["prefix"], ref pos) || msg.HasMentionPrefix(Discord.CurrentUser, ref pos))
            {
                var result = await Commands.ExecuteAsync(context, pos, Provider);
                if (!result.IsSuccess)
                {
                    var reason = result.Error;

                    if (reason == CommandError.UnknownCommand)
                    {
                        var embed = new EmbedBuilder()
                            .WithDescription($"Command {msg} does not exist")
                            .Build();
                        await context.Channel.SendMessageAsync(null, false, embed);
                        return;
                    }
                    await context.Channel.SendMessageAsync($"To following error occured:\n`{reason}`");
                    Console.WriteLine(reason);
                }
            }
        }
    }
}