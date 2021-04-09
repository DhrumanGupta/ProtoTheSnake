using System;
using System.Collections.Generic;

namespace DiscordSnakeBot.GameCore
{
    public static class Renderer
    {
        private static readonly Dictionary<BlockType, string> RenderStringByBlockType = new Dictionary<BlockType, string>()
        {
            { BlockType.Boundary, ":red_square:" },
            { BlockType.Empty, ":black_large_square:" },
            { BlockType.Snake, ":green_circle:" },
            { BlockType.Apple, ":apple:" }
        };

        public static string Render(this Grid grid)
        {
            string rendered = string.Empty;
            for (int x = 0; x < grid.X; x++)
            {
                for (int y = 0; y < grid.Y; y++)
                {
                    BlockType blockType = grid.GridData[x, y];
                    rendered += RenderStringByBlockType[blockType];
                }

                rendered += '\n';
            }

            return rendered;
        }
    }
}
