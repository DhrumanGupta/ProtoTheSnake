using System;
using System.Text;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            Game game = new Game(10, 15);
            game.Start();
        }
    }
}
