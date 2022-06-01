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
        public Vector2f PositionOnMap;
        public Vector2f Movement;
        private Vector2f prevPosition;
        public Location Location { get; set; }
        public override FloatRect Bounds { get { return new(new Vector2f(PositionOnMap.X - Origin.X, PositionOnMap.Y - size.Z), new(size.X, size.Z)); } }
        public Player(Location location)
        {
            MovementSpeed = 0.7f;
            VisibilityRadius = 20;//20
            Origin = new(size.X / 2, size.Y);
            shape.FillColor = Color.Blue;
            Location = location;
            prevPosition = PositionOnMap;
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
            Position = new(PositionOnMap.X, PositionOnMap.Y * World.Compression);
        }
        private Vector2f CheckBoundsFor(Location location)
        {
            Vector2f localPosition = PositionOnMap - location.PositionOnMap;
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
            return localPosition + location.PositionOnMap;
        }
        private void UpdateMove(int deltaTime)
        {
            PositionOnMap += Movement * deltaTime;
            Vector2f positionInBounds = CheckBoundsFor(Location); 
            if (positionInBounds != PositionOnMap)
            {
                bool isInTransition = false;
                foreach(Location location in Location.ConnectedLocations)
                {
                    if (CheckBoundsFor(location) == PositionOnMap)
                    {
                        isInTransition = true;
                        Location = location;
                        break;
                    }
                }
                if (!isInTransition)
                {
                    PositionOnMap = positionInBounds;
                }
            }
            prevPosition = PositionOnMap;//not sure
        }
        private void UpdateCollision()
        {
            PositionOnMap -= Location.PositionOnMap;
            prevPosition -= Location.PositionOnMap;
            foreach (var collisionObject in Location.Objects)
            {
                if (Intersects(collisionObject))
                {
                    Vector2f insidePosition = new(PositionOnMap.X, PositionOnMap.Y);
                    PositionOnMap = new(prevPosition.X, insidePosition.Y);
                    if (Intersects(collisionObject))
                    {
                        PositionOnMap = new(insidePosition.X, prevPosition.Y);
                        if (Intersects(collisionObject))
                        {
                            PositionOnMap = prevPosition;
                        }
                    }
                }
            }
            PositionOnMap += Location.PositionOnMap;
            prevPosition = PositionOnMap;
        }
    }
}
