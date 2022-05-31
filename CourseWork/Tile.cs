using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public enum TileType : byte
    {
        NONE,
        GROUND,
        TRAIL
    }
    public struct TileState
    {
        public TileType Type;
        public byte Id;
    }
    public class Tile : Object
    {
        public TileType Type { get; set; } = TileType.GROUND;
        public int Id { get; set; }
        public static int TileSize = 8;//32

        public override FloatRect Bounds => throw new NotImplementedException();

        public Tile() : this(TileType.GROUND, 0)
        {
            Type = TileType.GROUND;
            Id = 0;
        }
        public Tile(TileType type, int id)
        {
            Type = type;
            Id = id;
            shape = new(new Vector2f(TileSize, TileSize));
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
        public void UpdateShadow(double value)//NOT USED
        {
            value = 1 - value;
            if (value < 0) value = 0;
            if (value > 0.6) value = 0.6;
            value /= 0.6;
            byte bValue = (byte)(value * 255);
            shape.FillColor = new Color(bValue, bValue, bValue);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
    }
}
