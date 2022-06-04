using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Animator
    {
        private float currentFrame = 0;
        public float AnimationSpeed = 0.03f;
        private bool facingRight = true;
        private int frames;
        public float MovementSpeed;
        private Sprite sprite;
        public Animator(int frames, Sprite sprite, float animationSpeed = 0.03f, float movementspeed = 1f)
        {
            this.frames = frames;
            this.sprite = sprite;
            AnimationSpeed = animationSpeed;
            MovementSpeed = movementspeed;
        }
        public void Walk(int deltaTime, Vector2f movement)
        {
            currentFrame += deltaTime * AnimationSpeed * MovementSpeed;
            currentFrame %= frames;
            if (movement.X > 0)
            {
                facingRight = true;
            }
            else if (movement.X < 0)
            {
                facingRight = false;
            }
            UpdateFrame();
        }
        public void Idle()
        {
            currentFrame = 0;
            UpdateFrame();
        }
        private void UpdateFrame()
        {
            if (facingRight)
            {
                sprite.TextureRect = new((int)currentFrame * Tile.TileSize, 0, Tile.TileSize, Tile.TileSize * 2);
            }
            else
            {
                sprite.TextureRect = new(((int)currentFrame + 1) * Tile.TileSize, 0, -Tile.TileSize, Tile.TileSize * 2);
            }
        }
    }
}
