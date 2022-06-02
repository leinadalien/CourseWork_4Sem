using SFML.Graphics;

namespace CourseWork
{
   public class Content
    {
        public const string DIRECTORY = "../Content/";
        public static Texture GrassTexture = new(DIRECTORY + "grass.png");
        public static Texture DarknessTexture = new(DIRECTORY + "darkness.png");
    }
}
