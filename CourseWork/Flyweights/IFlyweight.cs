using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public interface IFlyweight<T>
    {
        public void Draw(T uniqueObject, RenderTarget target, RenderStates states);
    }
}
