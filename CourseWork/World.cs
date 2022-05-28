using CourseWork.Entities;
using CourseWork.Locations;
using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class World : Transformable, Drawable
    {
        private List<Location> locations;
        private RectangleShape darkness;
        public Player Player;
        private FloatRect bounds { get { return new(Position, new(1624, 1112)); } }
        public float Compression { get; set; } = 0.5f;
        public List<Location> Locations { get { return locations; } }
        public World(int locationsCount)
        {
            locations = new();
            for(int i = 0; i < locationsCount; i++)
            {
                locations.Add(new Glade());
            }
            locations.Last().Position = new(600, 600);
            var obj = new Stone();
            obj.Position = new(100,100);
            locations.First().AddObject(obj);
            var obj2 = new Stone();
            obj2.Position = new(100, 100);
            locations.Last().AddObject(obj2);
            Player = new(locations.First(), bounds);
            Player.Position = locations.First().StartPosition;
            darkness = new(new Vector2f(Player.VisibilityRadius * 2 * Tile.TILE_SIZE * 1.1f, Player.VisibilityRadius * 2 * Compression * Tile.TILE_SIZE * 1.1f));
            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        public void Update(int deltatime)
        {
            Player.Update(deltatime);
            darkness.Position = Player.Position;
            UpdatePosition();
            foreach (Location location in locations)
            {
                location.UpdateDrawableObjects(Player);
                if (Player.Bounds.Intersects(location.Bounds) && Player.Location != location)
                {
                    Player.Location = location;
                }
            }
        }
        public void UpdatePosition()
        {
            Vector2f curPosition = new(Program.Window.Size.X / 2 - Player.Position.X, Program.Window.Size.Y / 2 - Player.Position.Y);
            if (curPosition.X > 0)
            {
                curPosition.X = 0;
            }
            if (curPosition.Y > 0)
            {
                curPosition.Y = 0;
            }
            //TODO
            if (curPosition.X < Program.Window.Size.X - bounds.Width)
            {
                curPosition.X = Program.Window.Size.X - bounds.Width;
            }
            if (curPosition.Y < Program.Window.Size.Y - bounds.Height)
            {
                curPosition.Y = Program.Window.Size.Y - bounds.Height;
            }
            Position = curPosition;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            Player.Position -= Player.Location.Position;
            Player.Location.AddObject(Player);
            foreach (Location location in locations)
            {
                location.Draw(target, states);
            }
            Player.Position += Player.Location.Position;
            Player.Location.RemoveObject(Player);
            target.Draw(darkness, states);
        }
    }
}
