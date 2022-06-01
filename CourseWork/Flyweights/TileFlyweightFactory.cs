using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class TileFlyweightFactory : FlyweightFactory<Tile>
    {
        public TileFlyweightFactory(params Tile[] objects)
        {
            foreach (var obj in objects)
            {

                flyweights.Add(obj.GetHashCode(), new TileFlyweight(obj));
            }
        }
        public override IFlyweight<Tile> GetFlyweight(Tile sharedTile)
        {
            int key = sharedTile.GetHashCode();
            if (!flyweights.Where(x => x.Key == key).Any())
            {

                flyweights.Add(key, new TileFlyweight(sharedTile));
            }
            return flyweights.First(x => x.Key == key).Value;
        }
    }
}
