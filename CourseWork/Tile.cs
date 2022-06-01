using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public enum TileType : byte
    {
        NONE = 0,
        GROUND = 1,
        TRAIL = 2,
        DARK = 3,
    }
    public struct TileState
    {
        public TileType Type;
        public byte Id;
    }
    public class Tile : Object
    {
        public TileType Type { get; set; } = TileType.GROUND;
        public int Id { get; set; } = 0;
        public static int TileSize = 32;//32

        public override FloatRect Bounds => throw new NotImplementedException();

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
                case TileType.DARK:
                    shape.FillColor = new(0, 64, 0);
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
        public override int GetHashCode()
        {
            int result = Type.GetHashCode();
            result = 31 * result + Id.GetHashCode();
            return result;
        }
    }
}
