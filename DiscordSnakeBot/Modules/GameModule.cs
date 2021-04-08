using System;
using System.Linq;
using System.Security.Principal;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordSnakeBot.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Game = SnakeGame.Game;

namespace DiscordSnakeBot.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        private readonly Games _games;

        public GameModule(Games games)
        {
            _games = games;
        }
        
        [Command("start"), Alias("play")]
        public async Task StartAsync()
        {
            Game game = new Game(10, 10);
            game.Start();
            await _games.AddGameAsync(game);
            if (!(Context.Guild.Channels.FirstOrDefault(x => x.Id == 827273451055218788) is ISocketMessageChannel
                channel)) return;
            var text = Context.Message.Channel.Id == channel.Id ? string.Empty : Context.User.Mention;
            text += "\nStarting";
            channel?.SendMessageAsync(text);
        }
    }
}
