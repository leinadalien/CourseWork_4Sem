using CourseWork.GUI;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace CourseWork
{
    public class Game
    {
        int timeForScore = 0;
        public bool Pause = false;
        private Sprite heart = new(Content.HeartTexture);
        private Sprite key = new(Content.KeyTexture);
        public EventHandler<KeyEventArgs> KeyPressed { get; set; }
        public EventHandler<KeyEventArgs> KeyReleased { get; set; }
        public EventHandler<MouseButtonEventArgs> MouseClick { get; set; }
        public EventHandler PauseHandler { get; set; }
        public EventHandler PlayerDeadHandler { get; set; }
        public EventHandler PlayerWinHandler { get; set; }
        private World world;
        private Clock clock;
        public Game(int seed)
        {
            KeyPressed = MovePlayer;
            KeyReleased = MovementKeyReleased;
            KeyReleased += EscapeReleased;
            PauseHandler = (s, e) => { };
            PlayerDeadHandler = (s, e) => { };
            PlayerWinHandler = (s, e) => { };
            world = new(seed);
            MouseClick = (s, e) => world.MouseClick(e);
            clock = new Clock();
        }
        public void Update()
        {
            int deltaTime = clock.ElapsedTime.AsMilliseconds();
            if (!Pause)
            {
                world.Update(deltaTime);
                if (!world.Player.Alive)
                {
                    PlayerDeadHandler.Invoke(null, null);
                }
                if (world.PlayerAtHome)
                {
                    world.Player.Score *= world.Player.Health;
                    PlayerWinHandler.Invoke(null, null);
                }
                timeForScore += deltaTime;
                if (timeForScore > 15000)
                {
                    timeForScore -= 15000;
                    world.Player.Score--;
                }
            }/*
            //Game stats (for debug)
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Player map position: X({(int)world.Player.TruePosition.X}) Y({(int)world.Player.TruePosition.Y}) [{(int)world.Player.TruePosition.X / Tile.TileSize}][{(int)world.Player.TruePosition.Y / Tile.TileSize}]          ");
            Console.WriteLine($"Player Camera position: X({(int)(world.Player.Position.X + world.Position.X)}) Y({(int)(world.Player.Position.Y + world.Position.Y)})          ");

            Console.WriteLine($"Camera position: X({-(int)world.Position.X}) Y({-(int)world.Position.Y})      ");
            Console.WriteLine($"Camera X: [{0 - (int)world.Position.X / Tile.TileSize},{Program.Window.Size.X / Tile.TileSize + 1 - (int)world.Position.X / Tile.TileSize}]    ");
            Console.WriteLine($"Camera Y: [{0 - (int)world.Position.Y / Tile.TileSize},{Program.Window.Size.Y / Tile.TileSize + 1 - (int)world.Position.Y / Tile.TileSize}]    ");
            Console.WriteLine((1000 / (clock.ElapsedTime.AsMilliseconds() + 1)) + "fps  ");
            Console.WriteLine($"World compression: {World.Compression}");
            Console.WriteLine($"Find Key: {world.Player.WithKey}");
            Console.WriteLine($"Player health: {world.Player.Health}");*/
            clock.Restart();
        }
        public void Draw()
        {
            Program.Window.Draw(world);
            for (int i = 0; i < world.Player.Health; i++)
            {
                heart.Position = new(Tile.TileSize / 2 + i * Tile.TileSize, Tile.TileSize / 2);
                Program.Window.Draw(heart);
            }
            if (world.Player.WithKey)
            {
                key.Position = new(Program.Window.Size.X - Tile.TileSize * 1.5f, 0);
                Program.Window.Draw(key);
            }
            Text score = new($"Score: {((world.Player.Score > 0) ? world.Player.Score : 0)}", Content.Font, 30);
            score.Position = new(Program.Window.Size.X / 2 - score.GetLocalBounds().Width / 2, Tile.TileSize / 2);
            Program.Window.Draw(score);
        }
        private void MovePlayer(object? sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.A:
                    world.Player.Movement.X = -world.Player.MovementSpeed;
                    break;
                case Keyboard.Key.D:
                    world.Player.Movement.X = world.Player.MovementSpeed;
                    break;
                case Keyboard.Key.W:
                    world.Player.Movement.Y = -world.Player.MovementSpeed;
                    break;
                case Keyboard.Key.S:
                    world.Player.Movement.Y = world.Player.MovementSpeed;
                    break;
                default:
                    break;
            }
        }
        private void MovementKeyReleased(object? sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.A:
                    if (!Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        world.Player.Movement.X = 0;
                    }
                    else
                    {
                        world.Player.Movement.X = world.Player.MovementSpeed;
                    }
                    break;
                case Keyboard.Key.D:
                    if (!Keyboard.IsKeyPressed(Keyboard.Key.A))
                    {
                        world.Player.Movement.X = 0;
                    }
                    else
                    {
                        world.Player.Movement.X = -world.Player.MovementSpeed;
                    }
                    break;
                case Keyboard.Key.W:
                    if (!Keyboard.IsKeyPressed(Keyboard.Key.S))
                    {
                        world.Player.Movement.Y = 0;
                    }
                    else
                    {
                        world.Player.Movement.Y = world.Player.MovementSpeed;
                    }
                    break;
                case Keyboard.Key.S:
                    if (!Keyboard.IsKeyPressed(Keyboard.Key.W))
                    {
                        world.Player.Movement.Y = 0;
                    }
                    else
                    {
                        world.Player.Movement.Y = -world.Player.MovementSpeed;
                    }
                    break;
                default:
                    break;
            }
        }
        private void EscapeReleased(object? sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                if (!Pause)
                {
                    Pause = true;
                    PauseHandler.Invoke(sender, e);
                }
            }
        }
    }
}
