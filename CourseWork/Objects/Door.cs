using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Door : Object
    {
        private List<Location> locations = new();
        public IntRect IntBounds { get; private set; }
        public Door(IntRect bounds, params Location[] locations)
        {
            IntBounds = bounds;
            this.locations.AddRange(locations);
            size = new(bounds.Width * Tile.TileSize, 0, bounds.Height * Tile.TileSize);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Location.Compression);
            Position = new(bounds.Left, bounds.Top);
            shape = new(new Vector2f(size.X, size.Y));
            shape.FillColor = new(255, 255, 0, 127);
        }
        public override FloatRect Bounds { get { return new(Position - Origin, shape.Size); } }
    }
}