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
        public TileState() { }
        public TileType Type = TileType.NONE;
        public byte Id = 0;
        public float Brightness = 1;
    }
    public class Tile : Object
    {

        public static int TileSize = 48;//32
        private TileType type = TileType.GROUND;
        private byte id = 0;
        private float brightness = 255;
        private Color color = new(255,255,255);
        public TileType Type { get { return type; } }
        public int Id { get { return id; } }
        public float Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                brightness = value;
                shape.FillColor = new((byte)(color.R * brightness), (byte)(color.G * brightness), (byte)(color.B * brightness));
            }
        }

        public override FloatRect Bounds => throw new NotImplementedException();

        public Tile(TileState state)
        {
            type = state.Type;
            id = state.Id;
            brightness = state.Brightness;
            shape = new(new Vector2f(TileSize, TileSize));
            switch (type)
            {
                case TileType.GROUND:
                    shape.Texture = Content.GrassTexture;
                    shape.TextureRect = new IntRect(id * 32, 0, 32, 32);                
                    break;
                case TileType.TRAIL:
                    color = new(255, 255, 128);
                    break;
                case TileType.DARK:
                    color = new(0, 64, 0);
                    break;
                default:
                    break;
            }
            shape.FillColor = color;
            //shape.Scale = new Vector2f(1, World.Compression);
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
