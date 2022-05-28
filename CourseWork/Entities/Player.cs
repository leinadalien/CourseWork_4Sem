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
        public override FloatRect Bounds { get { return new(new Vector2f(Position.X - Origin.X, Position.Y - size.Z * Location.Compression), new(size.X, size.Z * Location.Compression)); } }
        public Player(Location location)
        {
            MovementSpeed = 0.3f;
            VisibilityRadius = 20;
            size = new(32, 64, 32);
            Origin = new(size.X / 2, size.Y);
            shape.FillColor = Color.Blue;
            Location = location;
            prevPosition = Position;
        }
        public Player(Player other)
        {
            MovementSpeed = other.MovementSpeed;
            VisibilityRadius = other.VisibilityRadius;
            size = other.size;
            Origin = other.Origin;
            shape.FillColor = other.shape.FillColor;
            Location = other.Location;
            prevPosition = other.prevPosition;
            Position = new(other.Position.X, other.Position.Y);
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
            if (curPosition.X < size.X / 2)
            {
                curPosition.X = size.X / 2;
            }
            if (curPosition.Y < size.Y)
            {
                curPosition.Y = size.Y;
            }
            //TODO
            if (curPosition.X > Program.Window.Size.X - Location.Position.X - size.X / 2)
            {
                curPosition.X = Program.Window.Size.X - Location.Position.X - size.X / 2;
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
                    Vector2f tempPosition = Position;
                    if (prevPosition.X != Position.X)
                    {
                        tempPosition.X = prevPosition.X;
                    }
                    if (prevPosition.Y != Position.Y)
                    {
                        tempPosition.Y = prevPosition.Y;
                    }
                    Position = tempPosition;
                }
            }
            prevPosition = Position;
        }
    }
}
