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
        public virtual TileState[,] GenerateTiles(Random random)
        {
            TileState[,] tiles = new TileState[TileCount.Y, TileCount.X];
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    tiles[i, j] = new() { Type = TileType.GROUND, Id = (byte)random.Next(8) };
                }
            }
            return tiles;
        }
        public void ConnectLocation(Location location)
        {
            connectedLocations.Add(location);
        }
        
    }
}
