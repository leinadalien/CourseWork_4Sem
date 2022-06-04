using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Locations
{
    public class Transition : Location
    {
        public Transition(IntRect bounds)
        {
            connectedLocations = new();
            IntBounds = bounds;
            sprite.Color = new(0, 255, 255, 120);
            TileCount = new(bounds.Width, bounds.Height);
            TruePosition = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize);
            sprite.TextureRect = new(0,0,(int)size.X,(int)size.Z);
        }

        public override List<Object> GenerateObjects(Random random)
        {
            return Objects;
        }
    }
}
