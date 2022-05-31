using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Locations
{
    public class Glade : Location
    {
        public Glade(IntRect bounds)
        {
            var obj = new Stone
            {
                Position = new(3 * Tile.TileSize, 3 * Tile.TileSize * Compression)
            };
            AddObject(obj);
            IntBounds = bounds;
            TileCount = new(bounds.Width, bounds.Height);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Compression);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize * Compression);
            Random random = new();
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize * Compression);
        }

        
    }
}
