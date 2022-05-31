using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class FlyweightFactory
    {
        private Dictionary<int, Flyweight> flyweights = new();
        public FlyweightFactory(params Object[] objects)
        {
            foreach (var obj in objects)
            {

                flyweights.Add(obj.GetHashCode(),new(obj));
            }
        }
        public Flyweight GetFlyweight(Object sharedObject)
        {
            int key = sharedObject.GetHashCode();
            if (!flyweights.Where(x => x.Key == key).Any())
            {
                
                flyweights.Add(key, new(sharedObject));
            }
            return flyweights.First(x => x.Key == key).Value;
        }
        public void PrintFlyweights()//NEED FOR DEBUG
        {
            foreach (var flyweight in flyweights)
            {
                Console.WriteLine(flyweight.GetHashCode());
            }
        }
    }
}
