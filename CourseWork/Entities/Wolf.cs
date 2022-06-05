using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Entities
{
    public class Wolf : Entity
    {/*
        public Wolf()
        {
            MovementSpeed = 0.5f;
            size = new(Tile.TileSize * 1.5f, Tile.TileSize, Tile.TileSize);
            sprite.Texture = Content.WolfTexture;
            MovementSpeed = 1f;
            VisibilityRadius = 20;//20
            Origin = new(size.X / 2, size.Y);
            prevPosition = TruePosition;
            animator = new(3, sprite, 0.03f, MovementSpeed);
            animator.Idle();
        }*/
        public override FloatRect Bounds => throw new NotImplementedException();
    }
}
