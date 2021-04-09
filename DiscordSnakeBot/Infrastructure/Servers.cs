using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DiscordSnakeBot.Infrastructure
{
    public class Servers
    {
        private readonly SnakeBotContext _context;

        public Servers(SnakeBotContext context)
        {
            _context = context;
        }

        public async Task ModifyGuildPrefix(ulong id, string prefix)
        {
            var server = await _context.Servers
                .FindAsync(id);
            if (server == null)
            {
                _context.Add(new Server
                {
                    Id = id,
                    Prefix = prefix
                });
            }
            else
            {
                server.Prefix = prefix;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetGuildPrefix(ulong id)
        {
            var prefix = await _context.Servers.AsQueryable()
                .Where(x => x.Id == id)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();

            return await Task.FromResult(prefix);
        }
        
        public async Task ModifyGuildGameChannelId(ulong id, ulong channelId)
        {
            var server = await _context.Servers
                .FindAsync(id);
            if (server == null)
            {
                _context.Add(new Server
                {
                    Id = id,
                    GameChannelId = channelId
                });
            }
            else
            {
                server.GameChannelId = channelId;
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task<ulong> GetGuildGameChannelId(ulong id)
        {
            var channelId = await _context.Servers.AsQueryable()
                .Where(x => x.Id == id)
                .Select(x => x.GameChannelId)
                .FirstOrDefaultAsync();

            return await Task.FromResult(channelId);
        }
    }
    
    public class Server
    {
        [Key]
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public ulong GameChannelId { get; set; }
    }
}