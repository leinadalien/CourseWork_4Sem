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
            IntBounds = bounds;
            TileCount = new(bounds.Width, bounds.Height);
            TruePosition = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize);
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize);
        }

        public override List<Object> GenerateObjects(Random random)
        {
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    if (i % 16 == 5 && j % 8 == 4 && TileCount.X - j > 4)
                    {
                        if (random.NextDouble() > 0.5)
                        {
                            House house = new() { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize) + TruePosition };
                            Objects.Add(house);
                            Houses.Add(house);
                        }
                    }
                    else if (i % 16 > 5 || j % 8 < 4)
                    {
                        if (random.NextDouble() > 0.85)
                        {
                            double randomObject = random.NextDouble();
                            if (randomObject > 0.98)
                            {
                                Objects.Add(new HighTree(random.Next(4)) { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2) + TruePosition, Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                            else if (randomObject > 0.97)
                            {
                                Objects.Add(new FatTree(random.Next(2)) { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2) + TruePosition, Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                            else if (randomObject > 0.92)
                            {
                                Objects.Add(new Stone(random.Next(4)) { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2) + TruePosition, Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                            else
                            {
                                Objects.Add(new Grass(random.Next(4)) { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2) + TruePosition, Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                        }
                    }
                }
            }
            return Objects;
        }
    }
}
