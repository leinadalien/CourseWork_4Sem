using SFML.Graphics;
using SFML.System;

namespace CourseWork
{
    public class Location : Transformable, Drawable
    {
        protected List <Enemy> enemies;
        protected Vector2f firstDrawingPoint;
        protected Vector2f lastDrawingPoint;
        public List<Touchable> touchableObjects;
        public const int WORLD_HEIGHT = 32;
        public const int WORLD_WIDTH = 32;
        public const float WORlD_COMPRESSION_Y = 0.5f;
        private RectangleShape darkness;
        public Player Player { get; set; }
        protected Random random;
        protected Tile[][] tiles;
        protected byte[][] map;
        protected PriorityQueue<Drawable, float> drawableObjects;
        public Vector2f StartPosition { get; set; }


        public Location()
        {
            Player = new();
            drawableObjects = new();
            touchableObjects = new();
            random = new();
            Generate();
            tiles = new Tile[WORLD_HEIGHT][];
            for (int i = 0; i < WORLD_HEIGHT; i++)
            {
                tiles[i] = new Tile[WORLD_WIDTH];
                for (int j = 0; j < WORLD_WIDTH; j++)
                {
                    /*if (map[i][j] > 128 && map[i][j] < 144)
                    {
                        tiles[i][j] = new Tile(TileType.TRAIL, 0);
                    }
                    else
                    {
                        tiles[i][j] = new(TileType.GROUND, random.Next(8));
                    }*/
                    tiles[i][j] = new(TileType.GROUND, random.Next(8));
                    tiles[i][j].Position = new Vector2f(j * Tile.TILE_SIZE, i * Tile.TILE_SIZE * WORlD_COMPRESSION_Y);
                }
            }
            StartPosition = new Vector2f(15 * Tile.TILE_SIZE, 15 * Tile.TILE_SIZE * WORlD_COMPRESSION_Y);
            darkness = new(new Vector2f(Player.VisibilityRadius * 2 * Tile.TILE_SIZE * 1.1f, Player.VisibilityRadius * 2 * WORlD_COMPRESSION_Y * Tile.TILE_SIZE * 1.1f));

            darkness.Origin = darkness.Size / 2;
            darkness.Texture = Content.DarknessTexture;
        }
        protected void Generate()
        {
            map = new byte[WORLD_HEIGHT][];
            for (int i = 0; i < WORLD_HEIGHT; i++)
            {
                map[i] = new byte[WORLD_WIDTH];
                random.NextBytes(map[i]);
            }
            for (int LayerSize = 2; LayerSize < WORLD_HEIGHT / 2; LayerSize *= 2)
            {
                byte[][] newLayer = new byte[WORLD_HEIGHT / LayerSize + 1][];
                
                for (int i = 0; i < newLayer.Length; i++)
                {
                    newLayer[i] = new byte[WORLD_WIDTH / LayerSize + 1];
                    random.NextBytes(newLayer[i]);
                }
                for (int i = 0; i < map.Length; i++)
                {
                    for (int j = 0; j < map[i].Length; j++)
                    {
                        int y21 = newLayer[i / LayerSize][j / LayerSize] + (newLayer[i / LayerSize + 1][j / LayerSize] - newLayer[i / LayerSize][j / LayerSize] * (i % LayerSize) / LayerSize);
                        int y43 = newLayer[i / LayerSize][j / LayerSize + 1] + (newLayer[i / LayerSize + 1][j / LayerSize + 1] - newLayer[i / LayerSize][j / LayerSize + 1] * (i % LayerSize) / LayerSize);
                        int x4321 = y21 + (y43 - y21) * (j % LayerSize) / LayerSize;
                        map[i][j] = (byte)((map[i][j] +x4321) / 2);
                        //linear interpolation
                    }
                }
            }
        }
        public void PrintMap()
        {
            for (int i = 0; i < WORLD_HEIGHT; i++)
            {
                for (int j = 0; j < WORLD_WIDTH; j++)
                {
                    Console.Write(map[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void Update()
        {
            darkness.Position = Player.Position;
            UpdatePosition();
            touchableObjects.Clear();
            UpdateDrawableObjects();
        }


        public void UpdatePosition()
        {
            Vector2f curPosition = Position;
            if (Player.Position.X - curPosition.X > Program.Window.Size.X / 2 && WORLD_WIDTH * Tile.TILE_SIZE - Player.Position.X > Program.Window.Size.X / 2)
            {
                curPosition.X = -Player.Position.X + Program.Window.Size.X / 2;
            }
            if (Player.Position.Y - curPosition.Y > Program.Window.Size.Y / 2 && WORLD_HEIGHT * Tile.TILE_SIZE * WORlD_COMPRESSION_Y - Player.Position.Y > Program.Window.Size.Y / 2)
            {
                curPosition.Y = -Player.Position.Y + Program.Window.Size.Y / 2;
            }
            if (curPosition.X + WORLD_WIDTH * Tile.TILE_SIZE < Program.Window.Size.X)
            {
                curPosition.X = -WORLD_WIDTH * Tile.TILE_SIZE + Program.Window.Size.X;
            }
            if (curPosition.Y + WORLD_HEIGHT * Tile.TILE_SIZE * WORlD_COMPRESSION_Y < Program.Window.Size.Y)
            {
                curPosition.Y = -WORLD_HEIGHT * Tile.TILE_SIZE * WORlD_COMPRESSION_Y + Program.Window.Size.Y;
            }
            Position = curPosition;
        }
        public virtual void UpdateDrawableObjects()
        {
            firstDrawingPoint = Player.Position - new Vector2f(Player.VisibilityRadius * Tile.TILE_SIZE, Player.VisibilityRadius * Tile.TILE_SIZE * WORlD_COMPRESSION_Y);
            lastDrawingPoint = Player.Position + new Vector2f(Player.VisibilityRadius * Tile.TILE_SIZE, Player.VisibilityRadius * Tile.TILE_SIZE * WORlD_COMPRESSION_Y);
            drawableObjects.Enqueue(Player, Player.Position.Y);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            for (int i = (int)(firstDrawingPoint.Y / Tile.TILE_SIZE / WORlD_COMPRESSION_Y); i < (int)(lastDrawingPoint.Y / Tile.TILE_SIZE / WORlD_COMPRESSION_Y); i++)
            {
                for (int j = (int)firstDrawingPoint.X / Tile.TILE_SIZE; j < (int)lastDrawingPoint.X / Tile.TILE_SIZE; j++)
                {
                    if (i < 0 || j < 0 || i >= WORLD_HEIGHT || j >= WORLD_WIDTH || tiles[i][j] == null) continue;
                    target.Draw(tiles[i][j], states);
                }
            }
            target.Draw(darkness, states);
            while (drawableObjects.Count > 0)
            {
                target.Draw(drawableObjects.Dequeue(), states);
            }
            
        }
    }
}
