using SFML.Graphics;
using SFML.System;
using CourseWork.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Entities
{
    public class Player : Entity
    {
        public bool WithKey = false;
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X - Origin.X, TruePosition.Y - size.Z), new(size.X, size.Z)); } }
        public Player(Location location) : base(location)
        {
            size = new(Tile.TileSize, Tile.TileSize * 2, Tile.TileSize);
            sprite.Texture = Content.PlayerTexture;
            MovementSpeed = 0.3f;
            VisibilityRadius = 5 * Tile.TileSize;//20
            Origin = new(size.X / 2, size.Y);
            Location = location;
            prevPosition = TruePosition;
            animator = new(3, sprite, (int)size.X, (int)size.Y, 0.03f, MovementSpeed);
            animator.Idle();
        }
        
        public void Animate(int deltaTime)
        {
            if (Alive)
            {
                if (Movement.X != 0 || Movement.Y != 0)
                {
                    animator.Walk(deltaTime, Movement);
                }
                else
                {
                    animator.Idle();
                }
            }
            else
            {
                animator.Death();
            }
        }
        public override void Update(int deltaTime)
        {
            
            if (Alive)
            {
                base.Update(deltaTime);
            }
            Animate(deltaTime);
        }
    }
}
