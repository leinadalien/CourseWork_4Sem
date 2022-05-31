using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.FlyWeights
{
    public class Flyweight
    {
        private Object sharedObject;
        public Flyweight(Object sharedObject)
        {
            this.sharedObject = sharedObject;
        }
        public void Draw(Object uniqueObject, RenderTarget target, RenderStates states)
        {
            sharedObject.Draw(target, states);
        }
    }
}
