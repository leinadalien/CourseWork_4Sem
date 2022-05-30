using SFML.Graphics;
using SFML.System;
using System.Drawing;

namespace CourseWork
{
    public abstract class Entity : Object
    {
        public int VisibilityRadius { get; set; } = 10;
        protected Entity()
        {
            shape.Size = new(Tile.TileSize, Tile.TileSize * 2);
            size = new(shape.Size.X, shape.Size.Y, Tile.TileSize);
        }
    }
}