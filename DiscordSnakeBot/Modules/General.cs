using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordSnakeBot.Modules
{
    public class General : ModuleBase
    {
        [Command("help")]
        public async Task Help()
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                .WithTitle("Play Snake Command List")
                .WithDescription("[GitHub Repository](https://github.com/DaRealBerlm/DiscordSnakeBot)")
                
                .AddField($"start", "`New Game`", true)
                .AddField("leaderboard", "`Server Leaderboard`", true)
                
                .WithColor(new Color(247, 49, 66))
                .WithFooter($"Requested by: {Context.User.Username}#{Context.User.Discriminator}")
                .WithCurrentTimestamp();

            var embed = builder.Build();

            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}