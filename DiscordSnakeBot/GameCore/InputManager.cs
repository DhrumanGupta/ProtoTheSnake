using System;
using System.Numerics;

namespace DiscordSnakeBot.GameCore
{
    public static class InputManager
    {
        internal static Direction GetDirectionFromEmote(string input)
        {
            switch (input)
            {
                case "⬆":
                    return Direction.North;

                case "➡":
                    return Direction.East;

                case "⬇":
                    return Direction.South;

                case "⬅":
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
