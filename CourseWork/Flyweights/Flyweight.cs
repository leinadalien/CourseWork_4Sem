using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class Flyweight
    {
        private Object sharedObject;
        public Flyweight(Object sharedObject)
        {
            this.sharedObject = sharedObject;
        }
        public void Draw(Vector2f Position, RenderTarget target, RenderStates states)
        {
            sharedObject.Position = Position;
            sharedObject.Draw(target, states);
        }
    }
}
