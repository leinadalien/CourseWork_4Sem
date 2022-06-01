using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public interface IFlyweightFactory<T>
    {
        public IFlyweight<T> GetFlyweight(T sharedState);

    }
}
