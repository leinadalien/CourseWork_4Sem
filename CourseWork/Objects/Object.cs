using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public abstract class Object : Transformable, Drawable, IComparable<Object>
    {
        protected Sprite sprite;
        protected Vector3f size;
        protected Color color = new(255, 255, 255, 255);
        protected float brightness = 1;
        protected float opacity = 1;
        public bool IsTrigger { get; set; } = false;
        public Vector2f TruePosition { get; set; }
        public float Brightness
        {
            get { return brightness; }
            set
            {
                brightness = value;
                byte r = (byte)(brightness * color.R);
                byte g = (byte)(brightness * color.G);
                byte b = (byte)(brightness * color.B);
                sprite.Color = new(r, g, b, sprite.Color.A);
            }
        }
        public float Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                byte a = (byte)(opacity * 255);
                sprite.Color = new(sprite.Color.R, sprite.Color.G, sprite.Color.B, a);
            }
        }
        public FloatRect DrawableBounds { get { return new( new(TruePosition.X - Origin.X, TruePosition.Y * World.Compression - Origin.Y), new(size.X, size.Y)); } }
        public float Thickness { get { return size.Z; } set { size.Z = value; } }
        public float Width { get { return size.X; } set { size.X = value; } }
        public float Height { get { return size.Y; } set { size.Y = value; } }
        public abstract FloatRect Bounds { get; }
        protected Object()
        {
            sprite = new(Content.StonesTexture);
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            sprite.TextureRect = new(0, 0, (int)size.X, (int)size.Y);
        }
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            Position = new(TruePosition.X, TruePosition.Y * World.Compression);
            states.Transform *= Transform;
            target.Draw(sprite, states);
        }
        public virtual bool Intersects(Object other)
        {
            return Bounds.Intersects(other.Bounds);
        }
        public int CompareTo(Object? other)
        {
            int result = TruePosition.Y.CompareTo(other?.TruePosition.Y);
            if (result == 0)
            {
                return TruePosition.X.CompareTo(other?.TruePosition.X);
            }
            else
            {
                return result;
            }
        }
        public override int GetHashCode()
        {
            int result = sprite.TextureRect.GetHashCode();
            result = 31 * result + sprite.Texture.NativeHandle.GetHashCode();
            result = 31 * result + sprite.Texture.Size.GetHashCode();
            return result;
        }
    }
}
