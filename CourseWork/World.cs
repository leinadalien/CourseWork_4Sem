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
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
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
            Random random = new(1);
            GenerateRooms(random);
            GenerateTransitions(random);
            GenerateTiles(random);
            Player = new(locations.First());
            Player.Position = locations.First().StartPosition + locations.First().Position;
            darkness = new(new Vector2f(Player.VisibilityRadius * 2 * Tile.TileSize * 1.1f, Player.VisibilityRadius * 2 * Location.Compression * Tile.TileSize * 1.1f));
            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        
        private void GenerateRooms(Random random)
        {
            Map root = new(new(new(0,0), mapSize));
            root.Split(random);
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
        private void GenerateTransitions(Random random)
        {
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
            List<Location> temp = new();
            temp.AddRange(locations);
            temp.AddRange(transitions);
            foreach (Location firstLocation in temp)
            {
                foreach (Location secondLocation in temp.Where(x => x.IntBounds != firstLocation.IntBounds))
                {
                    IntRect tempBounds = new(secondLocation.IntBounds.Left - 1, secondLocation.IntBounds.Top - 1, secondLocation.IntBounds.Width + 2, secondLocation.IntBounds.Height + 2);
                    if (firstLocation.IntBounds.Intersects(tempBounds))
                    {
                        firstLocation.ConnectLocation(secondLocation);
                    }
                }
            }
            
        }
        private void GenerateTiles(Random random)
        {
            tiles = new TileState[mapSize.Y, mapSize.X];
            foreach (Location location in locations)
            {
                IntRect bounds = location.IntBounds;
                TileState[,] locationTiles = location.GenerateTiles(random);
                for (int i = 0; i < bounds.Height; i++)
                {
                    for (int j = 0; j < bounds.Width; j++)
                    {
                        tiles[i + bounds.Top, j + bounds.Left] = locationTiles[i, j];
                    }
                }
            }
            foreach (Location location in transitions)
            {
                IntRect bounds = location.IntBounds;
                //TileState[,] locationTiles = location.GenerateTiles(random);
                for (int i = 0; i < bounds.Height; i++)
                {
                    for (int j = 0; j < bounds.Width; j++)
                    {
                        if (tiles[i + bounds.Top, j + bounds.Left].Type == TileType.NONE)
                        {
                            tiles[i + bounds.Top, j + bounds.Left] = new() { Type = TileType.TRAIL };//locationTiles[i, j];
                        }
                    }
                }
            }
            for (int i = 0; i < mapSize.X; i++)
            {
                for (int j = 0; j < mapSize.Y; j++)
                {
                    if (tiles[i, j].Type == TileType.NONE)
                    {
                        tiles[i, j] = new() { Type = TileType.DARK};
                    }
                }
            }
        }
        public void Update(int deltatime)
        {
            Player.Update(deltatime);
            darkness.Position = Player.Position;
            UpdatePosition();
            UpdateDrawableObjects(Player);
        }
        public virtual void UpdateDrawableObjects(Entity entity)
        {
            firstDrawingPoint = -Position;
            lastDrawingPoint = -Position + (Vector2f)Program.Window.Size;
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
            /*
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
            Player.Position += Player.Location.Position;*/
            Flyweight tileFlyweight;
            for (int i = (int)(firstDrawingPoint.Y / Tile.TileSize / Location.Compression); i < (int)(lastDrawingPoint.Y / Tile.TileSize / Location.Compression); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TileSize; j < (int)lastDrawingPoint.X / Tile.TileSize; j++)
                {
                    if (i < 0 || j < 0 || i >= mapSize.Y || j >= mapSize.X) continue;
                    tileFlyweight = FlyweightFactory.GetFlyweight(new Tile(tiles[i, j].Type, tiles[i, j].Id));
                    tileFlyweight.Draw(new(j * Tile.TileSize, i * Tile.TileSize * Location.Compression), target, states);
                }
            }
            Player.Draw(target,states);
            target.Draw(darkness, states);
        }
    }
}
