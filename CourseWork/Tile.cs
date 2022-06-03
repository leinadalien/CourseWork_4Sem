using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public enum TileType : byte
    {
        STONE_0 = 0,
        STONE_1 = 1,
        STONE_2 = 2,
        STONE_3 = 3,
        TRAIL_INTERNAL_CORNER = 4,
        TRAIL_SIDE = 5,
        TRAIL_EXTERNAL_CORNER = 6,
        TRAIL = 7,
        GROUND = 8,
        NONE = 255
    }
    public struct TileState
    {
        public TileState() { }
        public TileType Type = TileType.NONE;
        public float Brightness = 1;
        public float Rotation = 0;
    }
    public class Tile : Transformable, Drawable
    {
        private Sprite sprite;
        public static int TileSize = 48;//48
        private TileType type = TileType.GROUND;
        private float brightness = 255;
        private Color color = new(255,255,255);
        public TileType Type { get { return type; } }
        
        public float Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                brightness = value;
                sprite.Color = new((byte)(color.R * brightness), (byte)(color.G * brightness), (byte)(color.B * brightness));
            }
        }
        public Tile() : this(new()) { }
        public Tile(TileState state)
        {
            sprite = new(Content.TilesTexture);
            sprite.Origin = new(TileSize / 2, TileSize / 2);
            sprite.Position = new(TileSize / 2, TileSize / 2);
            type = state.Type;
            sprite.Rotation = state.Rotation;
            sprite.TextureRect = new IntRect((byte)type % 4 * 48,(byte)type / 4 * 48, 48, 48);
            Brightness = state.Brightness;
            //sprite.FillColor = new((byte)(color.R * brightness), (byte)(color.G * brightness), (byte)(color.B * brightness));
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(sprite, states);
        }
        public override int GetHashCode()
        {
            int result = type.GetHashCode();
            return result;
        }
    }
}
