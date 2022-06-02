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
            shape.FillColor = new(54, 54, 54);
        }

        public override FloatRect Bounds {get { return new(new Vector2f(TruePosition.X, TruePosition.Y - size.Z), new(size.X, size.Y)); } }
    }
}
