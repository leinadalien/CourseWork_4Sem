using SFML.Graphics;

namespace CourseWork
{
   public class Content
    {
        public const string DIRECTORY = "../Content/";
        public static Texture TilesTexture = new(DIRECTORY + "tiles.png") { Smooth = true };
        public static Texture DarknessTexture = new(DIRECTORY + "darkness.png");
    }
}
