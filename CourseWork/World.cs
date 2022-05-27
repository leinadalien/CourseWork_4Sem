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
        private RectangleShape boundsRectangle;
        public float Compression { get; set; } = 0.5f;
        public List<Location> Locations { get { return locations; } }
        public World(int locationsCount)
        {
            boundsRectangle = new();
            locations = new();
            for(int i = 0; i < locationsCount; i++)
            {
                locations.Add(new Glade());
            }
            locations.Last().Position = new(600, 600);
            boundsRectangle.Size = new(2000, 1112);
            var obj = new Stone();
            obj.Position = new(100,100);
            locations.First().AddObject(obj);
            var obj2 = new Stone();
            obj2.Position = new(100, 100);
            locations.Last().AddObject(obj2);
            Player = new(locations.First());
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
            foreach (Location location in Locations)
            {
                location.UpdateDrawableObjects(Player);
                
            }
        }
        public void UpdatePosition()
        {
            Vector2f curPosition = Position;
            if (Player.Position.X - curPosition.X > Program.Window.Size.X / 2 && boundsRectangle.Size.X - Player.Position.X > Program.Window.Size.X / 2)
            {
                curPosition.X = -Player.Position.X + Program.Window.Size.X / 2;
            }
            if (Player.Position.Y - curPosition.Y > Program.Window.Size.Y / 2 && boundsRectangle.Size.Y - Player.Position.Y > Program.Window.Size.Y / 2)
            {
                curPosition.Y = -Player.Position.Y + Program.Window.Size.Y / 2;
            }
            if (curPosition.X + boundsRectangle.Size.X < Program.Window.Size.X)
            {
                curPosition.X = -boundsRectangle.Size.X + Program.Window.Size.X;
            }
            if (curPosition.Y + boundsRectangle.Size.Y < Program.Window.Size.Y)
            {
                curPosition.Y = -boundsRectangle.Size.Y + Program.Window.Size.Y;
            }
            Position = curPosition;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            Player.Location.AddObject(Player);
            foreach (Location location in locations)
            {
                location.Draw(target, states);
            }
            Player.Location.RemoveObject(Player);
            target.Draw(darkness, states);
        }
    }
}
