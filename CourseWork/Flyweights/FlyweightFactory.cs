using CourseWork.FlyWeights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public abstract class FlyweightFactory<SubObject> where SubObject : Object
    {
        private List<Tuple<Flyweight, string>> flyweights = new();
        public FlyweightFactory(params SubObject[] objects)
        {
            foreach (var obj in objects)
            {
                flyweights.Add(new(new(obj), GetKey(obj)));
            }
        }
        public abstract string GetKey(SubObject obj);
        public Flyweight GetFlyweight(SubObject sharedObject)
        {
            string key = GetKey(sharedObject);
            if (!flyweights.Where(x => x.Item2 == key).Any())
            {
                flyweights.Add(new(new(sharedObject), GetKey(sharedObject)));
            }
            return flyweights.FirstOrDefault(x => x.Item2 == key).Item1;
        }
        public void PrintFlyweights()//NEED FOR DEBUG
        {
            foreach (var flyweight in flyweights)
            {
                Console.WriteLine(flyweight.Item2);
            }
        }
    }
}
