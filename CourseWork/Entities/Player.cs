using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Entities
{
    public class Player : Entity
    {
        public float MovementSpeed = 0.15f;
        public Vector2f Movement;
        private Vector2f prevPosition;
        public Location Location { get; set; }
        public Player(Location location)
        {
            Origin = new(Size.X / 2, Size.Y);
            shape.FillColor = Color.Blue;
            Location = location;
            prevPosition = Position;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
        }
        public void Update(int deltaTime)
        {
            UpdateMove(deltaTime);
            UpdateCollision();
        }
        private void UpdateMove(int deltaTime)
        {
            Position += Movement * deltaTime;

            Vector2f curPosition = Position;
            if (curPosition.X < Size.X / 2)
            {
                curPosition.X = Size.X / 2;
            }
            if (curPosition.Y < Size.Y)
            {
                curPosition.Y = Size.Y;
            }
            //need fix
            if (curPosition.X > Program.Window.Size.X - Location.Position.X - Size.X / 2)
            {
                curPosition.X = Program.Window.Size.X - Location.Position.X - Size.X / 2;
            }
            if (curPosition.Y > Program.Window.Size.Y - Location.Position.Y)
            {
                curPosition.Y = Program.Window.Size.Y - Location.Position.Y;
            }
            //
            Position = curPosition;
        }
        private void UpdateCollision()
        {
            foreach (var collisionObject in Location.Objects)
            {
                if (Intersects(collisionObject))
                {
                    Position = prevPosition;
                }
                else
                {
                    prevPosition = Position;
                }
            }
        }
    }
}
