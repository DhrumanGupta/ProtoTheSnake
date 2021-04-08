using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Microsoft.EntityFrameworkCore;
using Game = SnakeGame.Game; 

namespace DiscordSnakeBot.Infrastructure
{
    public class Games
    {
        private readonly SnakeBotContext _context;

        public Games(SnakeBotContext context)
        {
            _context = context;
        }

        public async Task AddGameAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task<Game> GetGame(ulong userId)
        {
            return await _context.Games.FindAsync(userId);
        }

        public async Task RemoveGame(Game game)
        {
            try
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Embed GetEmbed(Game game)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("");
            return builder.Build();
        }
    }
}