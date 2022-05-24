using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public abstract class Entity : Transformable, Drawable
    {
        public int VisibilityRadius { get; set; } = 10;
        protected RectangleShape shape = new(new Vector2f(32, 64));
        public Vector2f Size { get { return shape.Size; } private set { shape.Size = value; } }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}