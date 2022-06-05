using CourseWork.Entities;
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
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    if (random.NextDouble() > 0.7)
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
                    else if (random.NextDouble() > 0.9975)
                    {
                        Wolves.Add(new(this) { TruePosition = new Vector2f(j * Tile.TileSize, i * Tile.TileSize) + TruePosition });
                    }
                }
            }
            return Objects;
        }
    }
}
