using CourseWork.Flyweights;
using CourseWork.Locations;
using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public abstract class Location : Object
    {
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
        protected FloatRect drawingBounds;
        public override FloatRect Bounds { get { return new(TruePosition, new(TileCount.X * Tile.TileSize, TileCount.Y * Tile.TileSize)); } }
        public IntRect IntBounds { get; protected set; }
        public Vector2i TileCount { get; protected set; } = new(32, 32);
        protected List<Location> connectedLocations;
        
        public List<Location> ConnectedLocations { get { return connectedLocations; } }
        public List<Object> Objects { get; }
        public Vector2f StartPosition { get; set; }

        public Location()
        {
            Objects = new();
            connectedLocations = new();
            
        }
        public virtual Tile[,] GenerateTiles(Random random)
        {
            Tile[,] tiles = new Tile[TileCount.Y, TileCount.X];
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_INTERNAL_CORNER});
                        }
                        else if (j == TileCount.X - 1)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_INTERNAL_CORNER, Rotation = 90 });
                        }
                        else
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_SIDE});
                        }
                    } else if (i == TileCount.Y - 1)
                    {
                        if (j == 0)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_INTERNAL_CORNER, Rotation = 270 });
                        }
                        else if (j == TileCount.X - 1)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_INTERNAL_CORNER, Rotation = 180 });
                        }
                        else
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_SIDE, Rotation = 180});
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_SIDE, Rotation = 270 });
                        }
                        else if (j == TileCount.X - 1)
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL_SIDE, Rotation = 90 });
                        }
                        else
                        {
                            tiles[i, j] = new(new() { Type = TileType.TRAIL});
                        }
                    }
                }
            }
            return tiles;
        }
        public abstract List<Object> GenerateObjects(Random random);
        public void ConnectLocation(Location location)
        {
            connectedLocations.Add(location);
        }
        
    }
}
