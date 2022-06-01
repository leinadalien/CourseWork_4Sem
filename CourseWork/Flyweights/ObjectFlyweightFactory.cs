using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class ObjectFlyweightFactory : FlyweightFactory<Object>
    {
        public override IFlyweight<Object> GetFlyweight(Object sharedObject)
        {
            int key = sharedObject.GetHashCode();
            if (!flyweights.Where(x => x.Key == key).Any())
            {

                flyweights.Add(key, new ObjectFlyweight(sharedObject));
            }
            return flyweights.First(x => x.Key == key).Value;
        }
    }
}
