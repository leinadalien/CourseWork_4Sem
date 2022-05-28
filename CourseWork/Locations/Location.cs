using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public abstract class Location : Transformable, Drawable
    {
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
        protected FloatRect drawingBounds;
        public FloatRect Bounds { get { return new(Position, new(Width * Tile.TILE_SIZE, Height * Tile.TILE_SIZE * Compression)); } }
        public int Height { get; private set; } = 32;//TOFIX
        public int Width { get; private set; } = 32;
        public static float Compression { get; private set; } = 0.5f;
        public List<Door> Doors { get;}
        protected Random random;
        protected Tile[][] tiles;
        public List<Object> Objects { get;}
        public Vector2f StartPosition { get; set; }

        public Location()
        {
            random = new();
            tiles = new Tile[Height][];
            for (int i = 0; i < Height; i++)
            {
                tiles[i] = new Tile[Width];
                for (int j = 0; j < Width; j++)
                {
                    tiles[i][j] = new(TileType.GROUND, random.Next(8));
                    tiles[i][j].Position = new Vector2f(j * Tile.TILE_SIZE, i * Tile.TILE_SIZE * Compression);
                }
            }
            StartPosition = new Vector2f(15 * Tile.TILE_SIZE, 15 * Tile.TILE_SIZE * Compression);
            Objects = new();
            Doors = new();
            
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
            firstDrawingPoint = (entity.Position - new Vector2f(entity.VisibilityRadius * Tile.TILE_SIZE, entity.VisibilityRadius * Tile.TILE_SIZE * Compression)) - Position;
            lastDrawingPoint = (entity.Position + new Vector2f((entity.VisibilityRadius + 1) * Tile.TILE_SIZE, (entity.VisibilityRadius + 1) * Tile.TILE_SIZE * Compression)) - Position;
            drawingBounds = new(firstDrawingPoint, new(lastDrawingPoint.X - firstDrawingPoint.X, lastDrawingPoint.Y - firstDrawingPoint.Y));
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            for (int i = (int)(firstDrawingPoint.Y / Tile.TILE_SIZE / Compression); i < (int)(lastDrawingPoint.Y / Tile.TILE_SIZE / Compression); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TILE_SIZE; j < (int)lastDrawingPoint.X / Tile.TILE_SIZE; j++)
                {
                    if (i < 0 || j < 0 || i >= Height || j >= Width || tiles[i][j] == null) continue;
                    target.Draw(tiles[i][j], states);
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
