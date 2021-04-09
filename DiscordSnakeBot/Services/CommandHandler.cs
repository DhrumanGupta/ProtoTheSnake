using Discord.Commands;
using Discord.WebSocket;
using DiscordSnakeBot.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;

namespace DiscordSnakeBot.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly Servers _servers;
        private readonly Games _games;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service,
            IConfiguration config, Servers servers, Games games)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
            _servers = servers;
            _games = games;
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
            if (!(arg is SocketUserMessage message) || message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            var prefix = await _servers.GetGuildPrefix(((SocketGuildChannel) message.Channel).Guild.Id) ?? _config["defaultPrefix"];
            if (!message.HasStringPrefix(prefix, ref argPos) &&
                !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            if (reaction.UserId == _client.CurrentUser.Id) return;

            var message = await cachedMessage.GetOrDownloadAsync();
            if (message.Author.Id != _client.CurrentUser.Id) return;

            if (!_games.HasGame(reaction.UserId)) return;
            await message.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
            
            if (!_games.IsMessageTheirGame(reaction.UserId, message.Id)) return;

            _games.GetGame(reaction.UserId).HandleInput(reaction.Emote.Name);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified || !result.IsSuccess)
            {
                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        var embed = new EmbedBuilder()
                            .WithDescription($"Command `{context.Message.Content}` does not exist")
                            .Build();
                        await context.Channel.SendMessageAsync(null, false, embed);
                        break;
                }
            }
        }
    }
}