using SFML.Graphics;

namespace CourseWork
{
   public class Content
    {
        public const string DIRECTORY = "../Content/";
        public static Texture TilesTexture = new(DIRECTORY + "tiles.jpg");
        public static Texture StonesTexture = new(DIRECTORY + "stones.png");
        public static Texture HighTreesTexture = new(DIRECTORY + "high_trees.png");
        public static Texture FatTreesTexture = new(DIRECTORY + "fat_trees.png");
        public static Texture DarknessTexture = new(DIRECTORY + "darkness.png");
    }
}
