using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordSnakeBot.GameCore;
using Game = DiscordSnakeBot.GameCore.Game; 

namespace DiscordSnakeBot.Infrastructure
{
    public class Games
    {
        private Dictionary<ulong, Game> _gameByUserId = new Dictionary<ulong, Game>();

        public void AddGame(ulong userId, Game game)
        {
            game.OnGameUpdated += HandleGameUpdate;
            game.OnGameEnded += HandleGameEnded;
            _gameByUserId.Add(userId, game);
        }

        public bool HasGame(ulong userId)
        {
            return _gameByUserId.ContainsKey(userId);
        }

        public bool IsMessageTheirGame(ulong userId, ulong messageId)
        {
            return _gameByUserId[userId].Message.Id == messageId;
        }

        public Game GetGame(ulong userId)
        {
            return _gameByUserId[userId];
        }

        public void RemoveGame(ulong userId)
        {
            Game game = _gameByUserId[userId];
            game.OnGameUpdated -= HandleGameUpdate;
            game.OnGameEnded -= HandleGameEnded;
            
            _gameByUserId.Remove(userId);
        }

        private async Task HandleGameUpdate(Game game)
        {
            var level = game.Grid.Render();
            var embed = GetEmbed(level, game.Player);
            await game.Message.ModifyAsync(x => x.Embed = embed);
        }

        private async Task HandleGameEnded(Game game)
        {
            Embed embed = new EmbedBuilder()
                .WithTitle("Game Over!")
                .WithDescription($"{game.Player.Username}#{game.Player.Discriminator} reached a score of {game.Score}.\nThanks for playing!")
                .AddField("\u200B", "[GitHub Repository](https://github.com/DaRealBerlm/ProtoTheSnake)")
                .WithCurrentTimestamp()
                .Build();
            await game.Message.ModifyAsync(x => x.Embed = embed);
            
            RemoveGame(game.Player.Id);
        }

        private Embed GetEmbed(string level, SocketUser player)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Proto the Snake")
                .WithDescription(level)
                .WithFooter($"{player.Username}#{player.Discriminator}")
                .WithCurrentTimestamp();
            return builder.Build();
        }
    }
}