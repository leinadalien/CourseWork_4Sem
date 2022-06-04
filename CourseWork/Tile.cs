using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public enum TileType : byte
    {
        TRAIL_INTERNAL_CORNER = 0,
        TRAIL_SIDE = 1,
        TRAIL_EXTERNAL_CORNER = 2,
        TRAIL = 3,
        GROUND = 4,
    }
    public struct TileState
    {
        public TileState() { }
        public TileType Type = TileType.GROUND;
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
        public float SpriteRotation { get { return sprite.Rotation; } }
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
            type = state.Type;
            sprite.Rotation = state.Rotation;
            sprite.Scale = new(1.2f, 1.2f);
            sprite.Origin = new(44 / 2, 44 / 2);
            sprite.Position = new(48/2,48/2);
            sprite.TextureRect = new IntRect(2 + (byte)type * 48, 2, 44, 44);
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
