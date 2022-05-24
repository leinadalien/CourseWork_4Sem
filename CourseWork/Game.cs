using SFML.System;
using SFML.Window;

namespace CourseWork
{
    public class Game
    {
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
            world = new(3);
            clock = new Clock();
        }
        void Update()
        {
            world.Update(clock.ElapsedTime.AsMilliseconds());
            clock.Restart();
        }
        void Draw()
        {

        }
    }
    public struct GameSettings
    {
        public (int, int) AspectRatio { get; set; }
        public int TileSize { get; set; }
    }
}
