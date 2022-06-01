using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public abstract class FlyweightFactory<T> : IFlyweightFactory<T>
    {
        protected Dictionary<int, IFlyweight<T>> flyweights = new();
        public abstract IFlyweight<T> GetFlyweight(T sharedState);
    }
}
