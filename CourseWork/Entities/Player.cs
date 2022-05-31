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
        public float MovementSpeed = 0.15f;
        public Vector2f Movement;
        private Vector2f prevPosition;
        public Location Location { get; set; }
        public override FloatRect Bounds { get { return new(new Vector2f(Position.X - Origin.X, Position.Y - size.Z * Location.Compression), new(size.X, size.Z * Location.Compression)); } }
        public Player(Location location)
        {
            MovementSpeed = 0.7f;
            VisibilityRadius = 20;//20
            Origin = new(size.X / 2, size.Y);
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
        private Vector2f CheckBoundsFor(Location location)
        {
            Vector2f localPosition = Position - location.Position;
            if (localPosition.X < Origin.X)
            {
                localPosition.X = Origin.X;
            }
            if (localPosition.Y < Bounds.Height)
            {
                localPosition.Y = Bounds.Height;
            }
            if (localPosition.X > location.Width - Origin.X)
            {
                localPosition.X = location.Width - Origin.X;
            }
            if (localPosition.Y > location.Thickness)
            {
                localPosition.Y = location.Thickness;
            }
            return localPosition + location.Position;
        }
        private void UpdateMove(int deltaTime)
        {
            Position += new Vector2f(Movement.X * deltaTime, Movement.Y * deltaTime * Location.Compression);
            Vector2f positionInBounds = CheckBoundsFor(Location); 
            if (positionInBounds != Position)
            {
                bool isInTransition = false;
                foreach(Location location in Location.ConnectedLocations)
                {
                    if (CheckBoundsFor(location) == Position)
                    {
                        isInTransition = true;
                        Location = location;
                        break;
                    }
                }
                if (!isInTransition)
                {
                    Position = positionInBounds;
                }
            }
            
            
        }
        private void UpdateCollision()
        {
            Position -= Location.Position;
            prevPosition -= Location.Position;
            foreach (var collisionObject in Location.Objects)
            {
                if (Intersects(collisionObject))
                {
                    Vector2f insidePosition = new(Position.X, Position.Y);
                    Position = new(prevPosition.X, insidePosition.Y);
                    if (Intersects(collisionObject))
                    {
                        Position = new(insidePosition.X, prevPosition.Y);
                        if (Intersects(collisionObject))
                        {
                            Position = prevPosition;
                        }
                    }
                }
            }
            Position += Location.Position;
            prevPosition = Position;
        }
    }
}
