using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public static class RenderManager
    {
        public static Dictionary<BlockType, string> RenderStringByBlockType = new Dictionary<BlockType, string>()
        {
            { BlockType.Boundary, "#" },
            { BlockType.Empty, "." },
            { BlockType.Snake, "*" },
            { BlockType.Apple, "\u25EF" }
        };

        public static void RenderGrid(Grid grid)
        {
            for (int x = 0; x < grid.X; x++)
            {
                for (int y = 0; y < grid.Y; y++)
                {
                    Console.SetCursorPosition(y, x);

                    BlockType blockType = grid.GridData[x, y];
                    Console.Write(RenderStringByBlockType[blockType]);
                }
            }
        }
    }
}
