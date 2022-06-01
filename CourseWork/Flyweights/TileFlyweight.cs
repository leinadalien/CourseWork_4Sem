using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Flyweights
{
    public class TileFlyweight : Flyweight<Tile>
    {
        public TileFlyweight(Tile sharedTile) : base(sharedTile) { }
        public override void Draw(Tile uniqueTile, RenderTarget target, RenderStates states)
        {
            sharedState.Position = uniqueTile.Position;
            sharedState.Brightness = uniqueTile.Brightness;
            sharedState.Draw(target, states);
        }
    }
}
