using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SnakeGame
{
    public class Grid
    {
        public Grid(int x, int y)
        {
            CreateGridData(x, y);
            InitializeGridData();

            Vector2 originPos = new Vector2()
            {
                X = (int)(X / 2),
                Y = (int)(Y / 2)
            };

            _snake = new Snake(originPos);

            _apple = new Apple();
            SpawnApple();
        }

        #region Variables

        internal int X => GridData.GetLength(0);
        internal int Y => GridData.GetLength(1);
        internal Vector2 Size => new Vector2(X, Y);
        internal Vector2 SizeInside => new Vector2(X - 2, Y - 2);
        public BlockType[,] GridData { get; private set; }
        public bool IsSnakeDead => _snake.IsDead;
        public int Score => _snake.Positions.Count;

        private Vector2[] _boundary;
        private Snake _snake;
        private Apple _apple;

        #endregion Variables

        internal void Update(Direction direction)
        {
            var movement = InputManager.MovementFromDirection(direction);

            if (!IsMovementValid(movement))
            {
                _snake.IsDead = true;
                return;
            }

            _snake.Move(movement);
            if (_snake.AteApple(_apple.Position))
            {
                SpawnApple();
            }

            for (int x = 1; x < X - 1; x++)
            {
                for (int y = 1; y < Y - 1; y++)
                {
                    Vector2 thisPos = new Vector2(y, x);
                    this.GridData[x, y] = _snake.Positions.Contains(thisPos) ? BlockType.Snake : BlockType.Empty;

                    if (_apple.Position == thisPos)
                    {
                        this.GridData[x, y] = BlockType.Apple;
                    }
                }
            }
        }

        private bool IsMovementValid(Vector2 movement)
        {
            Vector2 newPos = _snake.Head + movement;

            if (_boundary.Contains(newPos))
            {
                return false;
            }

            var body = _snake.Body;
            for (int i = 0; i < body.Length; i++)
            {
                if (newPos == body[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void SpawnApple()
        {
            _apple.Spawn(SizeInside);

            while (_snake.Positions.Contains(_apple.Position))
            {
                _apple.Spawn(SizeInside);
            }
        }

        #region Initialize And Create Data

        private void CreateGridData(int x, int y)
        {
            if (x <= 0 || y <= 0)
            {
                Console.WriteLine("Invalid Size");
                Environment.Exit(1);
            }

            x = Math.Abs(x);
            y = Math.Abs(y);

            this.GridData = new BlockType[x, y];
        }

        private void InitializeGridData()
        {
            List<Vector2> boundary = new List<Vector2>();

            for (int x = 0; x < X; x++)
            {
                SetGridBoundary(x, 0, boundary);
                SetGridBoundary(x, Y - 1, boundary);
            }

            for (int y = 0; y < Y; y++)
            {
                SetGridBoundary(0, y, boundary);
                SetGridBoundary(X - 1, y, boundary);
            }

            this._boundary = boundary.ToArray();
        }

        private void SetGridBoundary(int x, int y, List<Vector2> boundary)
        {
            this.GridData[x, y] = BlockType.Boundary;
            boundary.Add(new Vector2(y, x));
        }

        #endregion
    }

    public enum BlockType
    {
        Empty,
        Boundary,
        Snake,
        Apple
    }
}
