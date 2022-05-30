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
        private Vector2f topLeftPoint = new(0,0);
        private Vector2i mapSize = new(192,192);
        private Vector2f size = new(192 * 32, 192 * 16);
        public List<Location> Locations { get { return locations; } }
        private void UpdateSize(Location location)
        {
            Vector2f deltaPosition = new(0, 0);
            if (location.Position.X < topLeftPoint.X)
            {
                topLeftPoint.X = -location.Position.X;
                size.X += deltaPosition.X;
            }
            if (location.Position.Y < topLeftPoint.Y)
            {
                topLeftPoint.Y = -location.Position.Y;
                size.Y += deltaPosition.Y;
            }
            if (location.Position.X + location.Width > Position.X + size.X)
            {
                size.X = location.Position.X + location.Width - Position.X;
            }
            if (location.Position.Y + location.Thickness > Position.Y + size.Y)
            {
                size.Y = location.Position.Y + location.Thickness - Position.Y;
            }
        }
        public World()
        {
            locations = new();
            GenerateByLeafs();
            Player = new(locations[1]);
            Player.Position = locations.First().StartPosition + locations.First().Position;
            darkness = new(new Vector2f(Player.VisibilityRadius * 2 * Tile.TileSize * 1.1f, Player.VisibilityRadius * 2 * Location.Compression * Tile.TileSize * 1.1f));
            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        private void GenerateByLeafs()
        {
            Leaf root = new(new(new(0,0), mapSize));
            root.Split(new Random());
            List<IntRect> locationsBounds = root.GetRooms();
            foreach (IntRect locationBounds in locationsBounds)
            {
                int index = 0;
                var location = new Glade(locationBounds);
                while (index < locations.Count && location.CompareTo(locations[index]) >= 1)
                {
                    index++;
                }
                locations.Insert(index, location);
                UpdateSize(location);
            }
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
            if (curPosition.X > topLeftPoint.X)
            {
                curPosition.X = topLeftPoint.X;
            }
            if (curPosition.Y > topLeftPoint.Y)
            {
                curPosition.Y = topLeftPoint.Y;
            }
            if (curPosition.X < Program.Window.Size.X - size.X)
            {
                curPosition.X = Program.Window.Size.X - size.X;
            }
            if (curPosition.Y < Program.Window.Size.Y - size.Y)
            {
                curPosition.Y = Program.Window.Size.Y - size.Y;
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
            Player.Location.RemoveObject(Player);
            Player.Position += Player.Location.Position;
            target.Draw(darkness, states);
        }
    }
}
