using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class ObjectFlyweight : Flyweight<Object>
    {
        public ObjectFlyweight(Object sharedObject) : base(sharedObject) { }
        public override void Draw(Object uniqueObject, RenderTarget target, RenderStates states)
        {
            sharedState.Draw(target, states);
        }
    }
}
