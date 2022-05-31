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
        public Glade() : base()
        {

        }
        public Glade(IntRect bounds)
        {
            TileCount = new(bounds.Width, bounds.Height);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Compression);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize * Compression);
            Random random = new();
            tiles = new Tuple<TileType, int>[TileCount.Y, TileCount.X];
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    tiles[i, j] = new(TileType.GROUND, random.Next(8));
                    //tiles[i, j].Position = new Vector2f(j * Tile.TileSize, i * Tile.TileSize * Compression);
                }
            }
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize * Compression);
        }
    }
}
