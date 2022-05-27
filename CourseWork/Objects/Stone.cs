﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Stone : Object
    {
        public Stone()
        {
            Origin = new(0, size.Y);
            shape.FillColor = Color.White;
        }

        public override FloatRect Bounds { get { return new(new Vector2f(Position.X, Position.Y - size.Z * Location.Compression), new(size.X, size.Y * Location.Compression)); } }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
    }
}
