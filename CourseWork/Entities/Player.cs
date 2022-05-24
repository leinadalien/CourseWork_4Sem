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
        public Location Location { get; set; }
        public Player(Location location)
        {
            Location = location;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(shape, states);
        }
        public void Update(int deltaTime)
        {
            UpdateMove(deltaTime);
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
    }
}
