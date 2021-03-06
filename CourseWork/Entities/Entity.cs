using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public abstract class Entity : Object
    {
        public int Health = 5;
        public bool Alive = true;
        public Location Location { get; set; }
        protected Animator animator;
        public float MovementSpeed = 0.15f;
        public Vector2f Movement;
        protected Vector2f prevPosition;
        public int VisibilityRadius { get; set; } = 10;
        protected Entity(Location location)
        {
            IsTrigger = true;
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            sprite.TextureRect = new(0, 0, (int)size.X, (int)size.Y);
            Location = location;
        }
        private Vector2f CheckCollisionFor(Location location)
        {
            Vector2f firstPositon = new(TruePosition.X, TruePosition.Y);
            Vector2f result = firstPositon;
            foreach (var collisionObject in location.Objects)
            {
                if (Intersects(collisionObject) && !collisionObject.IsTrigger)
                {
                    Vector2f insidePosition = new(TruePosition.X, TruePosition.Y);
                    TruePosition = new(prevPosition.X, insidePosition.Y);
                    result = TruePosition;
                    if (Intersects(collisionObject))
                    {
                        TruePosition = new(insidePosition.X, prevPosition.Y);
                        result = TruePosition;
                        if (Intersects(collisionObject))
                        {
                            result = prevPosition;
                        }
                    }
                }
            }
            TruePosition = firstPositon;
            return result;
        }
        protected void UpdateCollision()
        {
            Vector2f locationPosition = CheckCollisionFor(Location);

            if (locationPosition == TruePosition)
            {
                foreach (Location location in Location.ConnectedLocations)
                {
                    locationPosition = CheckCollisionFor(location);
                    if (locationPosition != TruePosition)
                    {
                        TruePosition = locationPosition;
                        break;
                    }
                }
            }
            else
            {
                TruePosition = locationPosition;
            }
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
        protected void UpdateMove(int deltaTime)
        {
            TruePosition += Movement * deltaTime;
            Vector2f positionInBounds = CheckBoundsFor(Location);
            if (positionInBounds != TruePosition)
            {
                bool isInTransition = false;
                foreach (Location location in Location.ConnectedLocations)
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
        public virtual void Update(int deltaTime)
        {
            if (Health <= 0)
            {
                Alive = false;
            }
            prevPosition = new(TruePosition.X, TruePosition.Y);
            UpdateMove(deltaTime);
            UpdateCollision();
            Position = new(TruePosition.X, TruePosition.Y * World.Compression);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(sprite, states);
        }
    }
}