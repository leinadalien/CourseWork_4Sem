using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class TileFlyweightFactory : FlyweightFactory<Tile>
    {
        public TileFlyweightFactory(params Tile[] objects) : base(objects) { }
        public override string GetKey(Tile obj)
        {
            return $"Tile_{obj.Type}_{obj.Id}";
        }
    }
}
