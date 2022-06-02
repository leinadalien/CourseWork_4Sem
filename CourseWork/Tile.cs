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
    public class Tile : Transformable, Drawable
    {
        private RectangleShape shape;
        public static int TileSize = 48;//48
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
        public Tile(TileState state)
        {
            type = state.Type;
            id = state.Id;
            brightness = state.Brightness;
            shape = new(new Vector2f(TileSize, TileSize * 1.5f));
            switch (type)
            {
                case TileType.GROUND:
                    shape.Origin = new(0, 16);
                    shape.Texture = Content.GrassTexture;
                    shape.TextureRect = new IntRect(id * 32, 0, 32, 48);
                    
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
        }
        public void Draw(RenderTarget target, RenderStates states)
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
