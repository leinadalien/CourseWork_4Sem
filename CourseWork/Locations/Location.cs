using CourseWork.Flyweights;
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
        public override FloatRect Bounds { get { return new(Position, new(TileCount.X * Tile.TileSize, TileCount.Y * Tile.TileSize * Compression)); } }
        public Vector2i TileCount { get; protected set; } = new(32, 32);
        public static float Compression { get; set; } = 0.5f;
        public List<Door> Doors { get;}
        protected Tuple<TileType, int>[,] tiles;
        public List<Object> Objects { get;}
        public Vector2f StartPosition { get; set; }

        public Location()
        {
            TileFlyweightFactory tileFactory = new(
                new Tile(TileType.GROUND, 0),
                new Tile(TileType.GROUND, 1),
                new Tile(TileType.GROUND, 2),
                new Tile(TileType.GROUND, 3),
                new Tile(TileType.GROUND, 4),
                new Tile(TileType.GROUND, 5),
                new Tile(TileType.GROUND, 6),
                new Tile(TileType.GROUND, 7)
                );
            Objects = new();
            Doors = new();
            var obj = new Stone
            {
                Position = new(3 * Tile.TileSize, 3 * Tile.TileSize * Compression)
            };
            AddObject(obj);
        }
        public void AddObject(Object obj)
        {
            int index = 0;
            while (index < Objects.Count && obj.CompareTo(Objects[index]) >= 1)
            {
                index++;
            }
            Objects.Insert(index, obj);
        }
        public void RemoveObject(Object obj)
        {
            Objects.Remove(obj);
        }
        public virtual void UpdateDrawableObjects(Entity entity)
        {
            firstDrawingPoint = (entity.Position - new Vector2f(entity.VisibilityRadius * Tile.TileSize, entity.VisibilityRadius * Tile.TileSize * Compression)) - Position;
            lastDrawingPoint = (entity.Position + new Vector2f((entity.VisibilityRadius + 1) * Tile.TileSize, (entity.VisibilityRadius + 1) * Tile.TileSize * Compression)) - Position;
            drawingBounds = new(firstDrawingPoint, new(lastDrawingPoint.X - firstDrawingPoint.X, lastDrawingPoint.Y - firstDrawingPoint.Y));
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            for (int i = (int)(firstDrawingPoint.Y / Tile.TileSize / Compression); i < (int)(lastDrawingPoint.Y / Tile.TileSize / Compression); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TileSize; j < (int)lastDrawingPoint.X / Tile.TileSize; j++)
                {
                    if (i < 0 || j < 0 || i >= TileCount.Y || j >= TileCount.X || tiles[i, j] == null) continue;
                    target.Draw(tiles[i, j], states);//LAST THINK ABOUT THIS
                }
            }
            //DOORS
            foreach (var door in Doors)
            {
                door.Position -= Position;
                target.Draw(door, states);
                door.Position += Position;
            }
            //
            foreach (var drawableObject in Objects)
            {
                if (drawingBounds.Intersects(drawableObject.Bounds))
                {
                    target.Draw(drawableObject, states);
                }
            }
            
        }
    }
}
