using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Stone : Transformable, IObject
    {
        private RectangleShape shape = new(new Vector2f(32, 32));
        public Stone()
        {
            shape.FillColor = Color.White;
        }
        public FloatRect GlobalBounds { get { return shape.GetGlobalBounds(); } }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
        public virtual bool Intersects(IObject other)
        {
            return shape.GetGlobalBounds().Intersects(other.GlobalBounds);
        }
    }
}
