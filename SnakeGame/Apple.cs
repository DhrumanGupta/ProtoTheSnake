using System;
using System.Numerics;

namespace SnakeGame
{
    internal class Apple
    {
        internal Vector2 Position { get; set; }
        private Random _random;

        public Apple()
        {
            _random = new Random();
        }

        internal void Spawn(Vector2 area)
        {

            Position = new Vector2(
                _random.Next(0, (int)area.Y) + 1,
                _random.Next(0, (int)area.X) + 1
                );
        }
    }
}