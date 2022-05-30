﻿using SFML.Graphics;
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
        protected RectangleShape shape;
        protected Vector3f size;
        public float Thickness { get { return size.Z; } set { size.Z = value; } }
        public float Width { get { return size.X; } set { size.X = value; } }
        public float Height { get { return size.Y; } set { size.Y = value; } }
        public abstract FloatRect Bounds { get; }
        protected Object()
        {
            shape = new(new Vector2f(32,32));
            size = new(shape.Size.X, shape.Size.Y, 32);
        }
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
        public virtual bool Intersects(Object other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public int CompareTo(Object? other)
        {
            int result = Position.Y.CompareTo(other?.Position.Y);
            if (result == 0)
            {
                return Position.X.CompareTo(other?.Position.X);
            }
            else
            {
                return result;
            }
        }
    }
}
