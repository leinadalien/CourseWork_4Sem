using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public abstract class Entity : Transformable, Drawable
    {
        public Vector3f Size { get; private set; }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}