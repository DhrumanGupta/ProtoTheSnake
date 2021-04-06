using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Infrastructure;

namespace DiscordSnakeBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly Servers _servers;

        public AdminModule(Servers servers)
        {
            _servers = servers;
        }
        
        [Command("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ChangePrefixAsync(string prefix = null)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                var guildPrefix = await _servers.GetGuildPrefix(Context.Guild.Id) ?? "!";
                
                var errorEmbed = EmbedFromPrefix(guildPrefix)
                    .AddField("Error:", "No prefix was given")
                    .Build();
                
                await ReplyAsync(null, false, errorEmbed);
                return;
            }

            if (prefix.Length > 6)
            {
                var guildPrefix = await _servers.GetGuildPrefix(Context.Guild.Id) ?? "!";
                
                var errorEmbed = EmbedFromPrefix(guildPrefix)
                    .AddField("Error:", "The given prefix was too long (must be under 7 characters)")
                    .Build();
                
                await ReplyAsync(null, false, errorEmbed);
                return;
            }
            
            await _servers.ModifyGuildPrefix(Context.Guild.Id, prefix);
            
            var embed = EmbedFromPrefix(prefix).Build();
            await ReplyAsync(null, false, embed);
        }

        private EmbedBuilder EmbedFromPrefix(string prefix)
        {
            var embed = new EmbedBuilder()
                .WithTitle($"New prefix for the bot is `{prefix}`")
                .WithColor(new Color(247, 49, 66))

                .WithFooter($"Requested by: {Context.User.Username}#{Context.User.Discriminator}")
                .WithCurrentTimestamp();

            return embed;
        }
    }
}