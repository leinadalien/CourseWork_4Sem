using CourseWork.Entities;
using CourseWork.Flyweights;
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
        private List<Transition> transitions;
        private RectangleShape darkness;
        public Player Player;
        public static FlyweightFactory FlyweightFactory { get; private set; } = new();
        private TileState[,] tiles;
        private Vector2f topLeftPoint = new(0, 0);
        private Vector2i mapSize = new(192, 192);//192
        private Vector2f size = new(192 * Tile.TileSize, 192 * Tile.TileSize * Location.Compression);
        public List<Location> Locations { get { return locations; } }
        public List<Transition> Transitions { get { return transitions; } }
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
            FlyweightFactory = new(
                new Tile(TileType.GROUND, 0),
                new Tile(TileType.GROUND, 1),
                new Tile(TileType.GROUND, 2),
                new Tile(TileType.GROUND, 3),
                new Tile(TileType.GROUND, 4),
                new Tile(TileType.GROUND, 5),
                new Tile(TileType.GROUND, 6),
                new Tile(TileType.GROUND, 7)
            );
            transitions = new();
            locations = new();
            GenerateRooms();
            GenerateTransitions(locations);
            tiles = new TileState[mapSize.X, mapSize.Y];
            foreach(Location location in locations)
            {
                IntRect bounds = location.IntBounds;
                TileState[,] locationTiles = location.GenerateTiles(new Random());
                for(int i = 0; i < bounds.Height; i++)
                {
                    for(int j = 0; j < bounds.Width; j++)
                    {
                        tiles[i + bounds.Top, j + bounds.Left] = locationTiles[i, j];
                    }
                }
            }
            Player = new(locations.First());
            Player.Position = locations.First().StartPosition + locations.First().Position;
            darkness = new(new Vector2f(Player.VisibilityRadius * 2 * Tile.TileSize * 1.1f, Player.VisibilityRadius * 2 * Location.Compression * Tile.TileSize * 1.1f));
            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        
        private void GenerateRooms()
        {
            Map root = new(new(new(0,0), mapSize));
            root.Split(new Random());
            List<IntRect> locationsBounds = root.GetRooms();
            foreach (IntRect locationBounds in locationsBounds)
            {
                int index = 0;
                var location = new Glade(locationBounds)
                {
                    FlyweightFactory = FlyweightFactory
                };
                while (index < locations.Count && location.CompareTo(locations[index]) >= 1)
                {
                    index++;
                }
                locations.Insert(index, location);
                UpdateSize(location);
            }
        }
        private void GenerateTransitions(List<Location> locations)
        {
            Random random = new();
            List<Location> tempLocations = new();
            
            foreach (Location firstLocation in locations)
            {
                foreach (Location tempLocation in locations)
                {
                    tempLocations.Add(tempLocation);
                }
                tempLocations.Remove(firstLocation);
                Location secondLocation = tempLocations[random.Next(tempLocations.Count)];
                List<IntRect> transitionBounds = Map.CreateTransition(firstLocation.IntBounds, secondLocation.IntBounds, random);
                foreach (IntRect transitionBound in transitionBounds)
                {
                    transitions.Add(new(transitionBound));
                }
            }
            foreach (Transition transition in transitions)
            {
                foreach (Location location in locations)
                {
                    if (transition.Intersects(location))
                    {
                        transition.ConnectLocation(location);
                        location.ConnectLocation(transition);
                    }
                }
                foreach (Transition location in transitions)
                {
                    if (transition.Intersects(location))
                    {
                        transition.ConnectLocation(location);
                        location.ConnectLocation(transition);
                    }
                }
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
            }
            foreach (Transition location in transitions)
            {
                location.UpdateDrawableObjects(Player);
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
            List<Location> temp = new();
            temp.AddRange(locations);
            temp.AddRange(transitions);
            foreach (Location location in temp)
            {
                location.Draw(target, states);
            }
            Player.Location.RemoveObject(Player);
            Player.Position += Player.Location.Position;
            target.Draw(darkness, states);
            
        }
    }
}
