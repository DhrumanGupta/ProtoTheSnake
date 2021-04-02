using System;
using System.Numerics;
using SnakeGame;

namespace SnakeGame
{
    public static class InputManager
    {
        internal static Direction DirectionFromConsoleInput()
        {
            ConsoleKey input = Console.ReadKey().Key;
            DeleteInputIfLetter(input);
            return GetDirectionFromKey(input);
        }

        private static void DeleteInputIfLetter(ConsoleKey input)
        {
            if (input != ConsoleKey.UpArrow && input != ConsoleKey.DownArrow && input != ConsoleKey.LeftArrow && input != ConsoleKey.RightArrow)
            {
                Console.Write("\b \b");
            }
        }

        private static Direction GetDirectionFromKey(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return Direction.North;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return Direction.East;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return Direction.South;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return Direction.West;

                default:
                    return Direction.Invalid;
            }
        }

        internal static Direction ValidateDirection(Direction newDirection, Direction currentDirection)
        {
            if (newDirection == Direction.Invalid)
            {
                newDirection = currentDirection;
            }

            if (newDirection == GetInverse(currentDirection))
            {
                newDirection = currentDirection;
            }

            return newDirection;
        }

        internal static Direction GetInverse(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;

                case Direction.East:
                    return Direction.West;

                case Direction.South:
                    return Direction.North;

                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.North;
            }
        }

        internal static Vector2 MovementFromDirection(Direction direction)
        {
            Vector2 movement = new Vector2();

            switch (direction)
            {
                case Direction.North:
                    movement.Y = -1;
                    break;

                case Direction.East:
                    movement.X = 1;
                    break;

                case Direction.South:
                    movement.Y = 1;
                    break;

                case Direction.West:
                    movement.X = -1;
                    break;
            }

            return movement;
        }
    }
}
