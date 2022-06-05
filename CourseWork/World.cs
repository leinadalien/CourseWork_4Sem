using CourseWork.Entities;
using CourseWork.Locations;
using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CourseWork
{
    public class World : Transformable, Drawable
    {
        public bool PlayerAtHome = false;
        public static float Compression = 0.5f;
        private Location firstLocation;
        private Location keyLocation;
        private House playerHouse;
        private List<Location> locations;
        private List<Location> transitions;
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
        private RectangleShape darkness;
        public List<Entity> Entities { get; } = new();
        public List<House> Houses { get; } = new();
        public Player Player;
        private Key key;
        private FloatRect drawingBounds;
        private Tile[,] tiles;
        private Vector2f topLeftPoint = new(0, 0);
        private Vector2i extraSpace = new(5, 30);
        private Vector2i mapSize = new(96, 96);//192
        private Vector2f size;
        public List<Location> Locations { get { return locations; } }
        public List<Location> Transitions { get { return transitions; } }
        private List<Object> drawableObjects { get; }
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
        public World(int seed)
        {
            size = new(mapSize.X * Tile.TileSize, mapSize.Y * Tile.TileSize);
            drawableObjects = new();
            transitions = new();
            locations = new();
            tiles = new Tile[mapSize.Y, mapSize.X];
            Random random = new(seed);
            GenerateRooms(random);
            GenerateTransitions(random);
            GenerateTiles(random);
            GenerateObjects(random);
            firstLocation = locations[random.Next(locations.Count)];
            Player = new(firstLocation) { TruePosition = firstLocation.StartPosition + firstLocation.TruePosition };
            Entities.Add(Player);
            GenerateEntities();
            darkness = new((Vector2f)Program.Window.Size);
            darkness.Texture = Content.DarknessTexture;
        }
        
        private void GenerateRooms(Random random)
        {
            Map root = new(new(new(extraSpace.X,extraSpace.Y), mapSize - extraSpace * 2));
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
                        Tile worldTile = tiles[i + bounds.Top, j + bounds.Left];
                        Tile locationTile = locationTiles[i, j];
                        if (worldTile != null)
                        {
                            if (locationTile.Type == TileType.TRAIL || worldTile.Type == TileType.TRAIL)
                            {
                                worldTile = new(new() { Type = TileType.TRAIL });
                            }
                            else if (worldTile.Type == TileType.TRAIL_SIDE && locationTile.Type == TileType.TRAIL_SIDE && locationTile.SpriteRotation != worldTile.SpriteRotation)
                            {
                                if (Math.Abs(locationTile.SpriteRotation - worldTile.SpriteRotation) != 180)
                                {
                                    float rotation;
                                    rotation = Math.Max(locationTile.SpriteRotation, worldTile.SpriteRotation);
                                    if (Math.Abs(locationTile.SpriteRotation - worldTile.SpriteRotation) >= 270)
                                    {
                                        rotation += 90;
                                    }
                                    worldTile = new(new() { Type = TileType.TRAIL_EXTERNAL_CORNER, Rotation = rotation });
                                } else
                                {
                                    worldTile = new(new() { Type = TileType.TRAIL });
                                }
                            }
                            else if (locationTile.Type == TileType.TRAIL_SIDE && worldTile.Type == TileType.TRAIL_INTERNAL_CORNER)
                            {
                                worldTile = locationTile;
                            }
                        }
                        else
                        {
                            worldTile = locationTile;
                        }
                        tiles[i + bounds.Top, j + bounds.Left] = worldTile;
                    }
                }
            }
            for (int i = 0; i < mapSize.Y; i++)
            {
                for (int j = 0; j < mapSize.X; j++)
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
            for (int i = 0; i < mapSize.Y; i++)
            {
                for (int j = 0; j < mapSize.X; j++)
                {
                    
                    if (tiles[i, j].Type == TileType.GROUND)
                    {
                        double randomObject = random.NextDouble();
                        bool isBound = false;
                        for (int a = i - 1; a < i + 2; a++)
                        {
                            for (int b = j - 1; b < j + 2; b++)
                            {
                                if (a >= 0 && b >= 0 && a < mapSize.Y && b < mapSize.X)
                                {
                                    if (tiles[a, b].Type == TileType.TRAIL_SIDE)
                                    {
                                        isBound = true;
                                    }
                                }
                            }
                        }
                        if (isBound)
                        {
                            if (randomObject < 0.5)
                            {
                                AddObject(new Stone(random.Next(4)) { TruePosition = new(j * Tile.TileSize, (i + 1) * Tile.TileSize) });
                            }
                            else
                            {
                                AddObject(new HighTree(random.Next(4)) { TruePosition = new(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2), Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                        }
                        else
                        {
                            if (randomObject > 0.9)
                            {
                                AddObject(new Stone(random.Next(4)) { TruePosition = new(j * Tile.TileSize, (i + 1) * Tile.TileSize) });
                            }
                            else if (randomObject > 0.8)
                            {
                                AddObject(new HighTree(random.Next(4)) { TruePosition = new(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2), Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                            else if (randomObject > 0.78)
                            {
                                AddObject(new FatTree(random.Next(2)) { TruePosition = new(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2), Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                            else if (randomObject > 0.7)
                            {
                                AddObject(new Grass(random.Next(4)) { TruePosition = new(j * Tile.TileSize, i * Tile.TileSize + Tile.TileSize / 2), Brightness = (float)random.NextDouble() * 0.5f + 0.5f });
                            }
                        }
                    }
                    
                }
            }
            foreach (Location location in locations)
            {
                AddObjects(location.GenerateObjects(random));
                Houses.AddRange(location.Houses);
            }
            keyLocation = locations[random.Next(locations.Count)];
            key = new() { TruePosition = new Vector2f(random.Next(keyLocation.TileCount.X) * Tile.TileSize, random.Next(keyLocation.TileCount.Y) * Tile.TileSize) + keyLocation.TruePosition };
            foreach (Object obj in keyLocation.Objects.ToList())
            {
                if (key.Intersects(obj))
                {
                    keyLocation.Objects.Remove(obj);
                }
            }
            keyLocation.Objects.Add(key);
            foreach (Transition transition in transitions)
            {
                List<Object> transitionObjects = transition.GenerateObjects(random);
                foreach (Object obj in transitionObjects.ToList())
                {
                    bool intersected = false;
                    foreach (Location location in locations)
                    {
                        if (obj.Intersects(location))
                        {
                            intersected = true;
                            break;
                        }
                    }
                    if (!intersected)
                    {
                        AddObject(obj);
                    }
                    else
                    {
                        transition.Objects.Remove(obj);
                    }
                }
            }
            playerHouse = Houses[random.Next(Houses.Count)];
        }
        private void GenerateEntities()
        {
            foreach (Transition transiton in transitions)
            {
                foreach (Wolf wolf in transiton.Wolves)
                {
                    wolf.Target = Player;
                    Entities.Add(wolf);
                }
            }
        }
        public void Update(int deltatime)
        {
            foreach (Entity entity in Entities)
            {
                entity.Update(deltatime);
            }
            if (Player.Intersects(key))
            {
                Player.WithKey = true;
                RemoveObject(key);
                keyLocation.Objects.Remove(key);
                playerHouse.IsTrigger = true;
            }
            if (Player.Intersects(playerHouse))
            {
                PlayerAtHome = true;
            }
            size.Y = mapSize.Y * Tile.TileSize;
            UpdatePosition();
            darkness.Position = -Position;
            UpdateDrawableObjects();
            Compression = (Player.TruePosition.Y / (mapSize.Y * Tile.TileSize) - 0.5f) * 0.3f + 0.3f;
            
        }
        public virtual void UpdateDrawableObjects()
        {
            firstDrawingPoint = -Position;
            lastDrawingPoint = -Position + (Vector2f)Program.Window.Size;
            drawingBounds = new(firstDrawingPoint, lastDrawingPoint - firstDrawingPoint);
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
            foreach (Entity entity in Entities)
            {
                AddObject(entity);
            }
            states.Transform *= Transform;
            for (int i = (int)(firstDrawingPoint.Y / Tile.TileSize / Compression); i <= (int)(lastDrawingPoint.Y / Tile.TileSize / Compression); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TileSize; j <= (int)lastDrawingPoint.X / Tile.TileSize; j++)
                {
                    if (i < 0 || j < 0 || i >= mapSize.Y || j >= mapSize.X) continue;
                    tiles[i, j].Position = new(j * Tile.TileSize, (int)(i * Tile.TileSize * Compression));
                    tiles[i, j].Scale = new(1, Compression);
                    tiles[i, j].Draw(target, states);
                }
            }
            foreach (Object drawableObject in drawableObjects)
            {
                if (drawableObject.DrawableBounds.Intersects(drawingBounds))
                {
                    drawableObject.Draw(target, states);
                }
            }
            foreach (Entity entity in Entities)
            {
                RemoveObject(entity);
            }
            target.Draw(darkness, states);
        }
        public void AddObject(Object obj)
        {
            int index = 0;
            while (index < drawableObjects.Count && obj.CompareTo(drawableObjects[index]) >= 1)
            {
                index++;
            }
            drawableObjects.Insert(index, obj);
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
            drawableObjects.Remove(obj);
        }
        public void MouseClick(MouseButtonEventArgs args)
        {
            FloatRect touchableBounds = new(new(Player.TruePosition.X - Player.VisibilityRadius, Player.TruePosition.Y - Player.VisibilityRadius), new Vector2f(2f, 2f) * Player.VisibilityRadius);
            foreach (Entity entity in Entities.Where(x => x.TruePosition != Player.TruePosition))
            {
                FloatRect entityBounds = entity.DrawableBounds;
                entityBounds.Left += Position.X;
                entityBounds.Top += Position.Y;
                if (entityBounds.Contains(args.X, args.Y) && touchableBounds.Intersects(entity.Bounds))
                {
                    entity.Health--;
                    break;
                }
            }
        }
    }
}
