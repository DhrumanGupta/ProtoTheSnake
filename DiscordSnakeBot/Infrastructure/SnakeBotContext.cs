using System.ComponentModel.DataAnnotations;
using DiscordSnakeBot.GameCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Game = DiscordSnakeBot.GameCore.Game;

namespace DiscordSnakeBot.Infrastructure
{
    public class SnakeBotContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql("server=localhost;user=root;database=discord_snake_game;port=3306;Connect Timeout=5");

    }
}