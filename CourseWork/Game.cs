using SFML.System;
using SFML.Window;

namespace CourseWork
{
    public class Game
    {
        public EventHandler<KeyEventArgs> KeyPressed;
        public EventHandler<KeyEventArgs> KeyReleased;
        private World world;
        private Clock clock;
        private GameSettings settings = new GameSettings()
        {
            AspectRatio = (16, 9),
            TileSize = 32,
        };
        public GameSettings Settings { get { return settings; } set { settings = value; } }
        public Game()
        {
            KeyPressed = MovePlayer;
            KeyReleased = MovementKeyReleased;
            world = new();
            clock = new Clock();
        }
        public void Update()
        {
            world.Update(clock.ElapsedTime.AsMilliseconds());
            //Game stats
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Player position: X({(int)world.Player.PositionOnMap.X}) Y({(int)world.Player.PositionOnMap.Y}) [{(int)world.Player.PositionOnMap.X / Tile.TileSize}][{(int)world.Player.PositionOnMap.Y / Tile.TileSize}]          ");
            Console.WriteLine($"Camera position: X({-(int)world.Position.X}) Y({-(int)world.Position.Y})      ");
            Console.WriteLine($"Camera X: [{0 - (int)world.Position.X / Tile.TileSize},{Program.Window.Size.X / Tile.TileSize + 1 - (int)world.Position.X / Tile.TileSize}]    ");
            Console.WriteLine($"Camera Y: [{0 - (int)world.Position.Y / Tile.TileSize},{Program.Window.Size.Y / Tile.TileSize + 1 - (int)world.Position.Y / Tile.TileSize}]    ");
            Console.WriteLine((1000 / clock.ElapsedTime.AsMilliseconds()) + "fps  ");
            Console.WriteLine($"World compression: {World.Compression}");
            
            clock.Restart();
            
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
    }
    public struct GameSettings
    {
        public (int, int) AspectRatio { get; set; }
        public int TileSize { get; set; }
    }
}
