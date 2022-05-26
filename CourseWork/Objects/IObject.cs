using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public interface IObject : Drawable
    {
        public FloatRect GlobalBounds { get; }
        public bool Intersects(IObject other);
    }
}
