using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Entities
{
    public class Wolf : Entity
    {
        private float timeForBite = 1000;
        private float timeBite = 0;
        public Entity Target { get; set; }
        public Wolf(Location location) : base(location)
        {
            Health = 3;
            VisibilityRadius = 10 * Tile.TileSize;
            MovementSpeed = 0.5f;
            size = new(Tile.TileSize * 1.5f, Tile.TileSize, Tile.TileSize);
            sprite.Texture = Content.WolfTexture;
            Origin = new(size.X / 2, size.Y);
            prevPosition = TruePosition;
            animator = new(4, sprite, (int)size.X, (int)size.Y, 0.03f, MovementSpeed);
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
                if (Target != null && Target.Bounds.Intersects(new(new(TruePosition.X - VisibilityRadius, TruePosition.Y - VisibilityRadius), new Vector2f(2f,2f) * VisibilityRadius)))
                {
                    if (Intersects(Target) && Target.Alive)
                    {
                        timeBite += deltaTime;
                        if (timeBite > timeForBite)
                        {
                            timeBite -= timeForBite;
                            Target.Health--;
                        }
                        Movement = new(0, 0);
                    }
                    else
                    {
                        timeBite = 0;
                        if (Target.TruePosition.X - TruePosition.X != 0)
                        {
                            Movement.X = (Target.TruePosition.X - TruePosition.X > 0 ? 1 : -1) * MovementSpeed;
                        }
                        else
                        {
                            Movement.X = 0;
                        }
                        if (Target.TruePosition.Y - TruePosition.Y != 0)
                        {
                            Movement.Y = (Target.TruePosition.Y - TruePosition.Y > 0 ? 1 : -1) * MovementSpeed;
                        }
                        else
                        {
                            Movement.Y = 0;
                        }
                    }
                    
                    
                }
                else
                {
                    Movement = new(0,0);
                }
                base.Update(deltaTime);
            }
            Animate(deltaTime);
        }
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X - Origin.X, TruePosition.Y - size.Z), new(size.X, size.Z)); } }
    }
}
