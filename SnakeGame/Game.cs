using System;
using System.Threading;

namespace ConsoleSnake
{
    public class Game
    {
        public Game(int x, int y)
        {
            _grid = new Grid(x, y);
        }

        private Thread _gameLoopThread;
        private Thread _inputThread;

        private Grid _grid;
        private bool _isGameRunning = false;
        private Direction _currentDirection;

        public void Start()
        {
            if (_isGameRunning) { return; }
            
            _isGameRunning = true;
            _currentDirection = Direction.North;

            _gameLoopThread = new Thread(new ThreadStart(GameLoop));
            _gameLoopThread.Start();

            _inputThread = new Thread(new ThreadStart(GetInput));
            _inputThread.Start();
        }

        private void GetInput()
        {
            while (!_grid.IsSnakeDead)
            {
                Direction newDirection = InputManager.DirectionFromConsoleInput();
                _currentDirection = InputManager.ValidateDirection(newDirection, _currentDirection);

                Thread.Sleep(250);
            }
        }

        private void GameLoop()
        {
            while (!_grid.IsSnakeDead)
            {
                RenderManager.RenderGrid(_grid);

                Thread.Sleep(250);

                _grid.Update(_currentDirection);
            }

            Console.WriteLine("\n");
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Your Score: {_grid.Score}");
            Console.ReadLine();
        }
    }

    public enum Direction
    {
        North,
        East,
        West,
        South,
        Invalid
    }
}
