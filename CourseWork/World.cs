using CourseWork.Entities;
using CourseWork.Locations;
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
        private Player player;
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
            player = new(locations.First());
            darkness = new(new Vector2f(player.VisibilityRadius * 2 * Tile.TILE_SIZE * 1.1f, player.VisibilityRadius * 2 * Compression * Tile.TILE_SIZE * 1.1f));
            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        public void Update(int deltatime)
        {
            player.Update(deltatime);
            darkness.Position = player.Position;
            UpdatePosition();
            foreach (Location location in Locations)
            {
                location.UpdateDrawableObjects(player);
            }
        }
        public void UpdatePosition()
        {
            Vector2f curPosition = Position;
            if (player.Position.X - curPosition.X > Program.Window.Size.X / 2 && boundsRectangle.Size.X * Tile.TILE_SIZE - player.Position.X > Program.Window.Size.X / 2)
            {
                curPosition.X = -player.Position.X + Program.Window.Size.X / 2;
            }
            if (player.Position.Y - curPosition.Y > Program.Window.Size.Y / 2 && boundsRectangle.Size.Y * Tile.TILE_SIZE * Compression - player.Position.Y > Program.Window.Size.Y / 2)
            {
                curPosition.Y = -player.Position.Y + Program.Window.Size.Y / 2;
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
                if (location != player.Location)
                {
                    location.DrawFloor(target, states);
                    location.Draw(target, states);
                }
            }
            player.Location.DrawFloor(target, states);
            target.Draw(player, states);
            target.Draw(darkness, states);
            player.Location.Draw(target, states);
        }
    }
}
