using CourseWork.GUI;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace CourseWork
{
    public class Game
    {
        public bool Pause = false;
        public EventHandler<KeyEventArgs> KeyPressed { get; set; }
        public EventHandler<KeyEventArgs> KeyReleased { get; set; }
        public EventHandler<MouseMoveEventArgs> MouseMove { get; set; }
        public EventHandler<MouseButtonEventArgs> MouseClick { get; set; }
        public EventHandler PauseHundler { get; set; }
        private World world;
        private Clock clock;
        public Game(int seed)
        {
            KeyPressed = MovePlayer;
            KeyReleased = MovementKeyReleased;
            KeyReleased += EscapeReleased;
            MouseMove = (s, e) => { };
            MouseClick = (s, e) => { };
            PauseHundler = (s, e) => { };
            world = new(seed);
            clock = new Clock();
        }
        public void Update()
        {
            if (!Pause)
            {
                world.Update(clock.ElapsedTime.AsMilliseconds());
                //Game stats
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Player map position: X({(int)world.Player.TruePosition.X}) Y({(int)world.Player.TruePosition.Y}) [{(int)world.Player.TruePosition.X / Tile.TileSize}][{(int)world.Player.TruePosition.Y / Tile.TileSize}]          ");
                Console.WriteLine($"Player Camera position: X({(int)(world.Player.Position.X + world.Position.X)}) Y({(int)(world.Player.Position.Y + world.Position.Y)})          ");

                Console.WriteLine($"Camera position: X({-(int)world.Position.X}) Y({-(int)world.Position.Y})      ");
                Console.WriteLine($"Camera X: [{0 - (int)world.Position.X / Tile.TileSize},{Program.Window.Size.X / Tile.TileSize + 1 - (int)world.Position.X / Tile.TileSize}]    ");
                Console.WriteLine($"Camera Y: [{0 - (int)world.Position.Y / Tile.TileSize},{Program.Window.Size.Y / Tile.TileSize + 1 - (int)world.Position.Y / Tile.TileSize}]    ");
                Console.WriteLine((1000 / (clock.ElapsedTime.AsMilliseconds() + 1)) + "fps  ");
                Console.WriteLine($"World compression: {World.Compression}");

                clock.Restart();
            }
        }
        public void Draw()
        {
            Program.Window.Draw(world);
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
                Pause = !Pause;
                if (Pause)
                {
                    PauseHundler.Invoke(sender, e);
                }
            }
        }
    }
}
