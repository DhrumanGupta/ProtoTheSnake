using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SnakeGame;

namespace DiscordSnakeBot.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        [Command("start"), Alias("play")]
        public async Task StartAsync()
        {
            await ReplyAsync("Starting");
        }
    }
}
