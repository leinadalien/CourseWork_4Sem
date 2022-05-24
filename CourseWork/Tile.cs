using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public enum TileType
    {
        NONE,
        GROUND,
        TRAIL
    }
    public class Tile : Transformable, Drawable
    {
        public TileType type = TileType.GROUND;
        public const int TILE_SIZE = 32;
        RectangleShape shape;
        public Tile() : this(TileType.GROUND, 0)
        {
            
        }
        public Tile(TileType type, int id)
        {
            
            shape = new(new Vector2f(TILE_SIZE, TILE_SIZE));
            switch (type)
            {
                case TileType.GROUND:
                    shape.Texture = Content.GrassTexture;
                    shape.TextureRect = new IntRect(id * 32, 0, 32, (int)(32 * Location.Compression));
                    break;
                case TileType.TRAIL:
                    shape.FillColor = new(255, 255, 128);
                    break;
                default:
                    break;
            }
            shape.Scale = new Vector2f(1, Location.Compression);
        }
        public void UpdateShadow(double value)
        {
            value = 1 - value;
            if (value < 0) value = 0;
            if (value > 0.6) value = 0.6;
            value /= 0.6;
            byte bValue = (byte)(value * 255);
            shape.FillColor = new Color(bValue, bValue, bValue);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
    }
}
