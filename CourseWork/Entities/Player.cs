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
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X - Origin.X, TruePosition.Y - size.Z), new(size.X, size.Z)); } }
        public Player(Location location)
        {
            MovementSpeed = 0.7f;
            VisibilityRadius = 20;//20
            Origin = new(size.X / 2, size.Y);
            shape.FillColor = Color.Blue;
            Location = location;
            prevPosition = TruePosition;
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
            Position = new(TruePosition.X, TruePosition.Y * World.Compression);
        }
        private Vector2f CheckBoundsFor(Location location)
        {
            Vector2f localPosition = TruePosition - location.TruePosition;
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
            return localPosition + location.TruePosition;
        }
        private void UpdateMove(int deltaTime)
        {
            TruePosition += Movement * deltaTime;
            Vector2f positionInBounds = CheckBoundsFor(Location); 
            if (positionInBounds != TruePosition)
            {
                bool isInTransition = false;
                foreach(Location location in Location.ConnectedLocations)
                {
                    if (CheckBoundsFor(location) == TruePosition)
                    {
                        isInTransition = true;
                        Location = location;
                        break;
                    }
                }
                if (!isInTransition)
                {
                    TruePosition = positionInBounds;
                }
            }
        }
        private void UpdateCollision()
        {
            foreach (var collisionObject in Location.Objects)
            {
                if (Intersects(collisionObject))
                {
                    Vector2f insidePosition = new(TruePosition.X, TruePosition.Y);
                    TruePosition = new(prevPosition.X, insidePosition.Y);
                    if (Intersects(collisionObject))
                    {
                        TruePosition = new(insidePosition.X, prevPosition.Y);
                        if (Intersects(collisionObject))
                        {
                            TruePosition = prevPosition;
                        }
                    }
                }
            }
            prevPosition = TruePosition;
        }
    }
}
