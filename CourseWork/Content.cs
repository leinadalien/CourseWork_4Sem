using SFML.Graphics;

namespace CourseWork
{
   internal class Content
    {
        public const string DIRECTORY = "../Content/";
        public static Texture GrassTexture;
        public static Texture DarknessTexture;
        public static Texture[] TreesTextures;
        public static void Load()
        {
            GrassTexture = new(DIRECTORY + "grass.jpg");
            DarknessTexture = new(DIRECTORY + "darkness.png");
            TreesTextures = new Texture[1];
            for (int i = 0; i < TreesTextures.Length; i++)
            {
                TreesTextures[i] = new(DIRECTORY + $"/Tree/tree{(i + 1)}.png");
            }
        }
    }
}
