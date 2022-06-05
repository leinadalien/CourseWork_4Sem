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
        private float timeForBite = 2000;
        private float timeBite = 2000;
        public Entity? Target { get; set; } = null;
        public Wolf(Location location) : base(location)
        {
            Health = 3;
            VisibilityRadius = 10 * Tile.TileSize;
            MovementSpeed = 0.4f;
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
                    timeBite += deltaTime;
                    if (Intersects(Target) && Target.Alive)
                    {
                        
                        if (timeBite > timeForBite)
                        {
                            timeBite = 0;
                            Target.Health--;
                        }
                        Movement = new(0, 0);
                    }
                    else
                    {
                        if (Target.TruePosition.X - TruePosition.X > 3)
                        {
                            Movement.X = MovementSpeed;
                        }
                        else if (Target.TruePosition.X - TruePosition.X < -3)
                        {
                            Movement.X = -MovementSpeed;
                        }
                        else
                        {
                            Movement.X = 0;
                        }
                        if (Target.TruePosition.Y - TruePosition.Y > 3)
                        {
                            Movement.Y = MovementSpeed;
                        }
                        else if (Target.TruePosition.Y - TruePosition.Y < -3)
                        {
                            Movement.Y = -MovementSpeed;
                        }
                        else
                        {
                            Movement.Y = 0;
                        }
                    }   
                }
                else
                {
                    Movement = new(0, 0);
                }
            }
            else
            {
                Movement = new(0, 0);
            }
            base.Update(deltaTime);
            Animate(deltaTime);
        }
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X - Origin.X, TruePosition.Y - size.Z), new(size.X, size.Z)); } }
    }
}
