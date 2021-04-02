using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SnakeGame
{
    internal class Snake
    {
        internal List<Vector2> Positions { get; private set; }
        private Vector2 _tail;
        internal int Length => Positions.Count;
        internal bool IsDead { get; set; }
        internal Vector2 Head => Positions[0];
        internal Vector2[] Body => Positions.GetRange(1, Positions.Count - 1).ToArray();

        internal Snake(Vector2 startPos)
        {
            Positions = new List<Vector2>();
            this.Positions.Add(startPos);

            IsDead = false;
        }

        internal void Move(Vector2 movement)
        {
            _tail = Positions[Positions.Count - 1];
            for (int i = Positions.Count - 1; i >= 1 ; i--)
            {
                Positions[i] = Positions[i - 1];
            }

            Positions[0] += movement;
        }

        internal bool AteApple(Vector2 applePos)
        {
            if (applePos == Head)
            {
                EatApple();
                return true;
            }
            return false;
        }

        internal void EatApple()
        {
            Positions.Add(_tail);
        }
    }
}
