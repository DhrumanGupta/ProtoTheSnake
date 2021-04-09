using System;
using System.Linq;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordSnakeBot.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Game = DiscordSnakeBot.GameCore.Game;

namespace DiscordSnakeBot.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        private readonly Servers _servers;
        private readonly Games _games;
        private readonly IConfiguration _configuration;
        private readonly Emoji[] _reactions;

        public GameModule(Servers servers, Games games, IConfiguration configuration)
        {
            _servers = servers;
            _games = games;
            _configuration = configuration;
            _reactions = new Emoji[]
            {
                new Emoji("⬅"),
                new Emoji("⬆"),
                new Emoji("➡"),
                new Emoji("⬇")
            };
        }

        [Command("start"), Alias("play")]
        public async Task StartAsync()
        {
            if (_games.HasGame(Context.Message.Author.Id))
            {
                await ReplyAsync("You already have a game going on!");
                return;
            }

            IMessageChannel channel = Context.Guild.GetChannel(await _servers.GetGuildGameChannelId(Context.Guild.Id)) as IMessageChannel;
            
            await StartNewGameAsync(channel);
        }

        private async Task StartNewGameAsync(IMessageChannel gameChannel)
        {
            if (Context.Message.Channel.Id != gameChannel.Id)
            {
                await gameChannel.SendMessageAsync($"{Context.Message.Author.Mention}, game will start here");
            }

            var embed = new EmbedBuilder()
                .WithTitle("Starting game...")
                .WithDescription("Get ready!")
                .Build();

            var message = await gameChannel.SendMessageAsync(null, false, embed);
            foreach (var reaction in _reactions)
            {
                await message.AddReactionAsync(reaction);
                await Task.Delay(450);
            }
            

            var game = new Game(10, 10)
            {
                Player = Context.Message.Author,
                Message = message,
                LoopTimeMs = Int32.Parse(_configuration["gameLoopInterval"])
            };
            _games.AddGame(Context.Message.Author.Id, game);
            game.Start();
        }

        [Command("leaderboard")]
        public async Task ShowLeaderboardAsync(SocketUser user = null)
        {
            if (user != null)
            {
                await ReplyAsync($"Not coming soon\nAlso, why did you ping {user.Username}");
            }
            else
            {
                await ReplyAsync("Not coming soon");
            }
        }
    }
}