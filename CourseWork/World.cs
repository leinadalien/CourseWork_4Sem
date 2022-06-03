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
        public static float Compression = 0.5f;
        private List<Location> locations;
        private List<Location> transitions;
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
        private RectangleShape darkness;
        public Player Player;
        private ObjectFlyweightFactory ObjectFlyweightFactory = new();
        private FloatRect drawingBounds;
        private Tile[,] tiles;
        private Vector2f topLeftPoint = new(0, 0);
        private Vector2i mapSize = new(192, 192);//192
        private Vector2f size = new(192 * Tile.TileSize, 192 * Tile.TileSize);
        public List<Location> Locations { get { return locations; } }
        public List<Location> Transitions { get { return transitions; } }
        public List<Object> DrawableObjects { get; }
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
            DrawableObjects = new();
            transitions = new();
            locations = new();
            tiles = new Tile[mapSize.Y, mapSize.X];
            Random random = new(1);
            GenerateRooms(random);
            GenerateTransitions(random);
            GenerateTiles(random);
            GenerateObjects(random);
            Player = new(locations.First());
            Player.TruePosition = locations.First().StartPosition + locations.First().TruePosition;
            darkness = new((Vector2f)Program.Window.Size);
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
                var location = new Glade(locationBounds);
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
                    transitions.Add(new Transition(transitionBound));
                }
            }
            List<Location> temp = new();
            temp.AddRange(locations);
            temp.AddRange(transitions);
            foreach (Location firstLocation in temp)
            {
                IntRect tempBounds = new(firstLocation.IntBounds.Left - 1, firstLocation.IntBounds.Top - 1, firstLocation.IntBounds.Width + 2, firstLocation.IntBounds.Height + 2);
                foreach (Location secondLocation in temp.Where(x => x.IntBounds != firstLocation.IntBounds))
                {
                    if (tempBounds.Intersects(secondLocation.IntBounds))
                    {
                        firstLocation.ConnectLocation(secondLocation);
                    }
                }
            }
            
        }
        
        private void GenerateTiles(Random random)
        {
            foreach (Location location in locations)
            {
                IntRect bounds = location.IntBounds;
                Tile[,] locationTiles = location.GenerateTiles(random);
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
                Tile[,] locationTiles = location.GenerateTiles(random);
                
                for (int i = 0; i < bounds.Height; i++)
                {
                    for (int j = 0; j < bounds.Width; j++)
                    {
                        if (tiles[i + bounds.Top, j + bounds.Left] != null)
                        {
                            if (locationTiles[i, j].Type == TileType.TRAIL)
                            {
                                tiles[i + bounds.Top, j + bounds.Left] = locationTiles[i, j];
                            }
                            else if (tiles[i + bounds.Top, j + bounds.Left].Type == locationTiles[i, j].Type && tiles[i + bounds.Top, j + bounds.Left].Rotation != locationTiles[i, j].Rotation)
                            {
                                tiles[i + bounds.Top, j + bounds.Left] = new(new() { Type = TileType.TRAIL });
                            }
                        }
                        else
                        {
                            tiles[i + bounds.Top, j + bounds.Left] = locationTiles[i, j];
                        }

                    }
                }
            }
            for (int i = 0; i < mapSize.X; i++)
            {
                for (int j = 0; j < mapSize.Y; j++)
                {
                    if (tiles[i, j] == null)
                    {
                        tiles[i, j] = new( new() { Type = TileType.GROUND });
                    }
                }
            }
        }
        private void GenerateObjects(Random random)
        {
            foreach (Location location in locations)
            {
                AddObjects(location.GenerateObjects(random));
            }
        }
        public void Update(int deltatime)
        {
            Player.Update(deltatime);
            UpdatePosition();
            darkness.Position = -Position;
            UpdateDrawableObjects();
            Compression = Player.TruePosition.Y / size.Y * 0.4f + 0.2f;
            RemoveObject(Player);
            AddObject(Player);
        }
        public virtual void UpdateDrawableObjects()
        {
            firstDrawingPoint = -Position;
            lastDrawingPoint = -Position + (Vector2f)Program.Window.Size;
            drawingBounds = new(firstDrawingPoint.X, firstDrawingPoint.Y / Compression, lastDrawingPoint.X - firstDrawingPoint.X, (lastDrawingPoint.Y - firstDrawingPoint.Y) / Compression);
        }
        public void UpdatePosition()
        {
            Vector2f curPosition = new(Program.Window.Size.X / 2 - Player.Position.X, Program.Window.Size.Y / 2 - Player.Position.Y);
            if (curPosition.X > topLeftPoint.X)
            {
                curPosition.X = topLeftPoint.X;
            }
            if (curPosition.Y > topLeftPoint.Y + Program.Window.Size.Y / 2)
            {
                curPosition.Y = topLeftPoint.Y + Program.Window.Size.Y / 2;
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
            for (int i = (int)(firstDrawingPoint.Y / Tile.TileSize / Compression); i <= (int)(lastDrawingPoint.Y / Tile.TileSize / Compression); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TileSize; j <= (int)lastDrawingPoint.X / Tile.TileSize; j++)
                {
                    if (i < 0 || j < 0 || i >= mapSize.Y || j >= mapSize.X) continue;
                    tiles[i, j].Position = new(j * Tile.TileSize, (i * Tile.TileSize * Compression));
                    tiles[i, j].Scale = new(1, Compression);
                    tiles[i, j].Draw(target, states);
                }
            }
            foreach (Object drawableObject in DrawableObjects)
            {
                if (drawableObject.Bounds.Intersects(drawingBounds))
                {
                    drawableObject.Draw(target, states);
                }
            }
            target.Draw(darkness, states);
        }
        public void AddObject(Object obj)
        {
            int index = 0;
            while (index < DrawableObjects.Count && obj.CompareTo(DrawableObjects[index]) >= 1)
            {
                index++;
            }
            DrawableObjects.Insert(index, obj);
        }
        public void AddObjects(List<Object> objects)
        {
            foreach (Object obj in objects)
            {
                AddObject(obj);
            }
        }
        public void RemoveObject(Object obj)
        {
            DrawableObjects.Remove(obj);
        }
    }
}
