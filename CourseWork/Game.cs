using SFML.System;
using SFML.Window;

namespace CourseWork
{
    public class Game
    {
        private GameSettings settings = new GameSettings()
        {
            AspectRatio = (16, 9),
            TileSize = 32,
        };
        public GameSettings Settings { get { return settings; } set { settings = value; } }
        public Game()
        {
            
        }
        void Update()
        {

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
