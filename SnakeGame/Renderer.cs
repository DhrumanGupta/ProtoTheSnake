using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public static class Renderer
    {
        private static readonly Dictionary<BlockType, string> RenderStringByBlockType = new Dictionary<BlockType, string>()
        {
            { BlockType.Boundary, "#" },
            { BlockType.Empty, "." },
            { BlockType.Snake, "*" },
            { BlockType.Apple, "\u25EF" }
        };

        public static string RenderGrid(Grid grid)
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
