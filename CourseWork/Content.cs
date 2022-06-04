using SFML.Graphics;

namespace CourseWork
{
   public class Content
    {
        public const string DIRECTORY = "../Content/";
        public static Texture TilesTexture = new(DIRECTORY + "tiles.jpg");
        public static Texture ObjectsTexture = new(DIRECTORY + "objects.png");
        public static Texture DarknessTexture = new(DIRECTORY + "darkness.png");
    }
}
