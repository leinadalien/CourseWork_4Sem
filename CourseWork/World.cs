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
            //var obj = new Stone();
            
            //obj.Position = new(15,15);
            //locations.First().Objects.Add(obj);
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
            if (Player.Position.X - curPosition.X > Program.Window.Size.X / 2 && boundsRectangle.Size.X * Tile.TILE_SIZE - Player.Position.X > Program.Window.Size.X / 2)
            {
                curPosition.X = -Player.Position.X + Program.Window.Size.X / 2;
            }
            if (Player.Position.Y - curPosition.Y > Program.Window.Size.Y / 2 && boundsRectangle.Size.Y * Tile.TILE_SIZE * Compression - Player.Position.Y > Program.Window.Size.Y / 2)
            {
                curPosition.Y = -Player.Position.Y + Program.Window.Size.Y / 2;
            }
            if (curPosition.X + boundsRectangle.Size.X * Tile.TILE_SIZE < Program.Window.Size.X)
            {
                curPosition.X = -boundsRectangle.Size.X * Tile.TILE_SIZE + Program.Window.Size.X;
            }
            if (curPosition.Y + boundsRectangle.Size.Y * Tile.TILE_SIZE * Compression < Program.Window.Size.Y)
            {
                curPosition.Y = -boundsRectangle.Size.Y * Tile.TILE_SIZE * Compression + Program.Window.Size.Y;
            }
            Position = curPosition;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (Location location in locations)
            {
                if (location != Player.Location)
                {
                    location.Draw(target, states);
                }
            }
            Player.Location.Draw(target, states);
            target.Draw(Player, states);

            target.Draw(darkness, states);
        }
    }
}
