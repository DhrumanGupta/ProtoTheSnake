using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordSnakeBot.GameCore
{
    public class Game
    {
        public Game(int x, int y)
        {
            InitializeGrid(x, y);
        }
        
        [Key]
        public SocketUser Player { get; set; }
        public IUserMessage Message { get; set; }
        public Grid Grid { get; set; }
        public int LoopTimeMs { get; set; }
        public event Func<Game, Task> OnGameUpdated;
        public event Func<Game, Task> OnGameEnded;
        public int Score => Grid.Score;

        private bool _isGameRunning = false;
        private Direction _currentDirection;

        private void InitializeGrid(int x, int y)
        {
            Grid ??= new Grid(x, y);
        }

        public void Start()
        {
            if (_isGameRunning) { return; }

            _isGameRunning = true;
            _currentDirection = Direction.North;
            
            GameLoop();
        }

        public void HandleInput(string input)
        {
            Direction newDirection = InputManager.GetDirectionFromEmote(input);
            _currentDirection = InputManager.ValidateDirection(newDirection, _currentDirection);
        }

        private async Task GameLoop()
        {
            while (!Grid.IsSnakeDead)
            {
                Grid.Update(_currentDirection);
                await Task.Delay(LoopTimeMs);
                if (OnGameUpdated != null) await OnGameUpdated?.Invoke(this);
            }
            
            await OnGameEnded?.Invoke(this);
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
