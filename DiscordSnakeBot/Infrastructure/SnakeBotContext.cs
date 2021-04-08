using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SnakeGame;
using Game = SnakeGame.Game;

namespace DiscordSnakeBot.Infrastructure
{
    public class SnakeBotContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql("server=localhost;user=root;database=discord_snake_game;port=3306;Connect Timeout=5");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .Property(b => b.Grid)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Grid>(v));
        }

    }
    
    public class Server
    {
        [Key]
        public ulong Id { get; set; }
        public string Prefix { get; set; }
    }
}