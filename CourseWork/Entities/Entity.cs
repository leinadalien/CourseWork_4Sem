using SFML.Graphics;
using SFML.System;
using System.Drawing;

namespace CourseWork
{
    public abstract class Entity : Object
    {
        public int VisibilityRadius { get; set; } = 10;
        protected Entity()
        {
            shape.Size = new(32, 64);
            size = new(shape.Size.X, shape.Size.Y, 32);
        }
    }
}