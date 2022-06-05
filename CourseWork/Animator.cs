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
        int frameWidth;
        int frameHeight;
        private float walkFrame = 0;
        private float currentFrame = 0;
        public float AnimationSpeed = 0.03f;
        private bool facingRight = true;
        private int frames;
        public float MovementSpeed;
        private Sprite sprite;
        public Animator(int frames, Sprite sprite, int frameWidth, int frameHeight, float animationSpeed = 0.03f, float movementspeed = 1f)
        {
            this.frames = frames;
            this.sprite = sprite;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            AnimationSpeed = animationSpeed;
            MovementSpeed = movementspeed;
        }
        public void Walk(int deltaTime, Vector2f movement)
        {
            currentFrame = deltaTime * AnimationSpeed * MovementSpeed + walkFrame;
            currentFrame %= 3;
            if (movement.X > 0)
            {
                facingRight = true;
            }
            else if (movement.X < 0)
            {
                facingRight = false;
            }
            UpdateFrame();
            walkFrame = currentFrame;
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
                sprite.TextureRect = new((int)currentFrame * frameWidth, 0, frameWidth, frameHeight);
            }
            else
            {
                sprite.TextureRect = new(((int)currentFrame + 1) * frameWidth, 0, -frameWidth, frameHeight);
            }
        }
        public void Death()
        {
            currentFrame = 3;
            currentFrame %= frames;
            UpdateFrame();
        }
    }
}
