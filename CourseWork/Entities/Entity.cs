using SFML.Graphics;
using SFML.System;
using System.Drawing;

namespace CourseWork
{
    public abstract class Entity : Object
    {

        protected Animator animator;
        public float MovementSpeed = 0.15f;
        public Vector2f Movement;
        protected Vector2f prevPosition;
        public int VisibilityRadius { get; set; } = 10;
        protected Entity()
        {
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            sprite.TextureRect = new(0, 0, (int)size.X, (int)size.Y);
        }
    }
}