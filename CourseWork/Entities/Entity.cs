using SFML.Graphics;
using SFML.System;
using System.Drawing;

namespace CourseWork
{
    public abstract class Entity : Object
    {
        public int VisibilityRadius { get; set; } = 10;
        public FloatRect GlobalBounds { get { return shape.GetGlobalBounds(); } }
        protected Entity()
        {
            shape = new(new Vector2f(32, 64));
        }
    }
}