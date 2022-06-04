using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public abstract class Object : Transformable, Drawable, IComparable<Object>
    {
        protected Sprite sprite;
        protected Vector3f size;
        public Vector2f TruePosition { get; set; }
        public FloatRect DrawableBounds { get { return new( new(TruePosition.X - Origin.X, TruePosition.Y * World.Compression - Origin.Y), new(size.X, size.Y)); } }
        public float Thickness { get { return size.Z; } set { size.Z = value; } }
        public float Width { get { return size.X; } set { size.X = value; } }
        public float Height { get { return size.Y; } set { size.Y = value; } }
        public abstract FloatRect Bounds { get; }
        protected Object()
        {
            sprite = new(Content.ObjectsTexture);
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
