using SFML.Graphics;
using SFML.System;
using System.Drawing;

namespace CourseWork
{
    public abstract class Entity : Transformable, Drawable, IObject
    {
        public int VisibilityRadius { get; set; } = 10;
        protected RectangleShape shape = new(new Vector2f(32, 64));
        public Vector2f Size { get { return shape.Size; } private set { shape.Size = value; } }
        public FloatRect GlobalBounds { get { return shape.GetGlobalBounds(); } }
        public abstract void Draw(RenderTarget target, RenderStates states);
        public virtual bool Intersects(IObject other)
        {
            return shape.GetGlobalBounds().Intersects(other.GlobalBounds);
        }
    }
}