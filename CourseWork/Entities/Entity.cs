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
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            sprite.TextureRect = new(0, 0, (int)size.X, (int)size.Y);
        }
    }
}