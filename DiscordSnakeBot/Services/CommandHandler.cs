using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Microsoft.Extensions.Hosting;

namespace DiscordSnakeBot.Services
{
    public class CommandHandler : InitializedService
    {
        private static IServiceProvider _provider;
        private static DiscordSocketClient _client;
        private static CommandService _service;
        private static IConfiguration _config;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.ReactionAdded += OnReactionAdded;
                
            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message) || message.Source != MessageSource.User) { return; }

            var argPos = 0;
            if (!message.HasStringPrefix(_config["prefix"], ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message.Source != MessageSource.Bot)
            {
                return;
            }
            
            
        }
        
        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified || !result.IsSuccess)
            {
                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        var embed = new EmbedBuilder()
                            .WithDescription($"Command {context.Message.Content} does not exist")
                            .Build();
                        await context.Channel.SendMessageAsync(null, false, embed);
                        break;
                    default:
                        await context.Channel.SendMessageAsync($"To following error occured:\n`{result.Error}`");
                        Console.WriteLine(result.Error);
                        break;
                }
            }
        }
    }
}