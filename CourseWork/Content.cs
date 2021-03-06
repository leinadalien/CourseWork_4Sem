using SFML.Graphics;

namespace CourseWork
{
    public class Content
    {
        public static string DIRECTORY = Directory.GetCurrentDirectory() + "/Content/";
        public static Texture TilesTexture = new(DIRECTORY + "tiles.jpg");
        public static Texture StonesTexture = new(DIRECTORY + "stones.png");
        public static Texture HighTreesTexture = new(DIRECTORY + "high_trees.png");
        public static Texture FatTreesTexture = new(DIRECTORY + "fat_trees.png");
        public static Texture GrassTexture = new(DIRECTORY + "grass.png");
        public static Texture DarknessTexture = new(DIRECTORY + "darkness.png");
        public static Texture PlayerTexture = new(DIRECTORY + "player.png");
        public static Texture KeyTexture = new(DIRECTORY + "key.png");
        public static Texture HouseTexture = new(DIRECTORY + "house.png");
        public static Texture WolfTexture = new(DIRECTORY + "wolf.png");
        public static Texture HeartTexture = new(DIRECTORY + "heart.png");
        public static Font Font = new(DIRECTORY + "font.ttf");
    }
}
