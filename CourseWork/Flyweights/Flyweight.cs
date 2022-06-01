using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public abstract class Flyweight<T> : IFlyweight<T>
    {
        protected T sharedState;
        protected Flyweight(T sharedState)
        {
            this.sharedState = sharedState;
        }
        public abstract void Draw(T uniqueState, RenderTarget target, RenderStates states);
    }
}
