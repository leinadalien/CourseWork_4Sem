using SFML.Graphics;
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
        public Vector2f PositionOnMap;
        public Stone()
        {
            Origin = new(0, size.Y);
            shape.FillColor = new(54, 54, 54);
        }

        public override FloatRect Bounds {get { return new(new Vector2f(PositionOnMap.X, PositionOnMap.Y - size.Z), new(size.X, size.Y)); } }
    }
}
