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
            shape.FillColor = new(0, 255, 255, 120);
            TileCount = new(bounds.Width, bounds.Height);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Compression);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize * Compression);
            shape.Size = new(size.X,size.Z);
        }
    }
}
