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
        private TileType type = TileType.GROUND;
        private byte id = 0;
        public TileType Type { get { return type; } }
        public int Id { get { return id; } }
        public static int TileSize = 32;//32

        public override FloatRect Bounds => throw new NotImplementedException();

        public Tile(TileType type, byte id)
        {
            this.type = type;
            this.id = id;
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
            int result = type.GetHashCode();
            result = 31 * result + id.GetHashCode();
            return result;
        }
    }
}
