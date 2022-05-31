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
            IntBounds = bounds;
            TileCount = new(bounds.Width, bounds.Height);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Compression);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize * Compression);
            Random random = new();
            tiles = new TileState[TileCount.Y, TileCount.X];
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    tiles[i, j] = new() { Type = TileType.GROUND, Id = (byte)random.Next(8) };
                }
            }
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize * Compression);
        }

        
    }
}
