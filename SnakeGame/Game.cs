using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Game
    {
        public Game() { }

        public Game(int x, int y)
        {
            Grid = new Grid(x, y);
        }
        
        [Key]
        public ulong PlayerId { get; set; }

        public Grid Grid { get; set; }
        private bool _isGameRunning = false;
        private Direction _currentDirection;
        private Thread _gameThread;

        public void InitializeGrid(int x, int y)
        {
            Grid ??= new Grid(x, y);
        }

        public void Start()
        {
            if (_isGameRunning) { return; }

            _isGameRunning = true;
            _currentDirection = Direction.North;
            
            _gameThread ??= new Thread(GameLoop);
            _gameThread.Priority = ThreadPriority.BelowNormal;
            _gameThread.Start();
        }

        private void HandleInput()
        {
            while (!Grid.IsSnakeDead)
            {
                Direction newDirection = InputManager.DirectionFromConsoleInput();
                _currentDirection = InputManager.ValidateDirection(newDirection, _currentDirection);

                Thread.Sleep(250);
            }
        }

        private void GameLoop()
        {
            while (!Grid.IsSnakeDead)
            {
                Renderer.RenderGrid(Grid);

                Thread.Sleep(250);

                Grid.Update(_currentDirection);
            }

            Console.WriteLine("\n");
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Your Score: {Grid.Score}");
            Console.ReadLine();
        }
    }

    public class GameData
    {
        
        public int TimeoutMs { get; set; }
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
