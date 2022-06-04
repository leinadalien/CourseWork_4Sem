﻿using SFML.Graphics;
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
        private Animator animator;
        public float MovementSpeed = 0.15f;
        public Vector2f Movement;
        private Vector2f prevPosition;
        public Location Location { get; set; }
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X - Origin.X, TruePosition.Y - size.Z), new(size.X, size.Z)); } }
        public Player(Location location)
        {
            size = new(Tile.TileSize, Tile.TileSize * 2, Tile.TileSize);
            sprite.Texture = Content.PlayerTexture;
            MovementSpeed = 1f;
            VisibilityRadius = 20;//20
            Origin = new(size.X / 2, size.Y);
            Location = location;
            prevPosition = TruePosition;
            animator = new(3, sprite, 0.03f, MovementSpeed);
            animator.Idle();
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(sprite, states);
        }
        public void Animate(int deltaTime)
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
        public void Update(int deltaTime)
        {
            prevPosition = new(TruePosition.X, TruePosition.Y);
            UpdateMove(deltaTime);
            UpdateCollision();
            Animate(deltaTime);
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
        private Vector2f CheckCollisionFor(Location location)
        {
            Vector2f firstPositon = new(TruePosition.X, TruePosition.Y);
            Vector2f result = firstPositon;
            foreach (var collisionObject in location.Objects)
            {
                if (Intersects(collisionObject))
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
        private void UpdateCollision()
        {
            Vector2f locationPosition = CheckCollisionFor(Location);

            if(locationPosition == TruePosition)
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
    }
}
