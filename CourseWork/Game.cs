using SFML.System;
using SFML.Window;

namespace CourseWork
{
    internal class Game
    {
        private World world;
        Clock clock;
        public Game()
        {
            player = new();
            world = new(2)
            {
                Player = player
            };
            player.SetLocation(world.Locations.First());
            player.Spawn();
            clock = new Clock();
            //location.PrintMap();
        }
        public void Update()
        {
            player.Movement.X = Keyboard.IsKeyPressed(Keyboard.Key.A) ? -player.MovementSpeed : Keyboard.IsKeyPressed(Keyboard.Key.D) ? player.MovementSpeed : 0;
            player.Movement.Y = Location.WORlD_COMPRESSION_Y * (Keyboard.IsKeyPressed(Keyboard.Key.W) ? -player.MovementSpeed : Keyboard.IsKeyPressed(Keyboard.Key.S) ? player.MovementSpeed : 0);
            player.IsTested = Keyboard.IsKeyPressed(Keyboard.Key.T);
            int deltaTime = clock.ElapsedTime.AsMilliseconds();
            player.Update(deltaTime);
           // locations.First().Update();
            //locations[1].Update();
            clock.Restart();
            Program.Window.Draw(world);
            //Game stats
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Player position: X({(int)player.Position.X}) Y({(int)player.Position.Y}) [{(int)player.Position.X / Tile.TILE_SIZE}][{(int)(player.Position.Y / Tile.TILE_SIZE / Location.WORlD_COMPRESSION_Y)}]          ");
            Console.WriteLine($"Camera position: X({-(int)player.location.Position.X}) Y({-(int)player.location.Position.Y})      ");
            Console.WriteLine($"Camera X: [{0 - (int)player.location.Position.X / Tile.TILE_SIZE},{Program.Window.Size.X / Tile.TILE_SIZE + 1 - (int)player.location.Position.X / Tile.TILE_SIZE}]    ");
            Console.WriteLine($"Camera Y: [{0 - (int)(player.location.Position.Y / Tile.TILE_SIZE / Location.WORlD_COMPRESSION_Y)},{Program.Window.Size.Y / Tile.TILE_SIZE / Location.WORlD_COMPRESSION_Y + 1 - (int)(player.location.Position.Y / Tile.TILE_SIZE / Location.WORlD_COMPRESSION_Y)}]    ");
           // Console.WriteLine((int)(1000/deltaTime) + "fps  ");
        }
    }
}
