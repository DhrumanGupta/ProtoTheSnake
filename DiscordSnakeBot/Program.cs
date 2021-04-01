using System;
using System.Threading.Tasks;


namespace DiscordSnakeBot
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}
