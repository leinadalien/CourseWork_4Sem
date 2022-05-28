using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Door : Object
    {
        public Door(FloatRect bounds)
        {
            size = new(bounds.Width, bounds.Height, bounds.Height);
            Origin = new(size.X/2, size.Y/2);
            Position = new(bounds.Left, bounds.Top);
            shape = new(new Vector2f(size.X, size.Y));
            shape.FillColor = new(255, 255, 0, 127);
        }
        public override FloatRect Bounds { get { return new(Position - Origin, shape.Size); } }
    }
}