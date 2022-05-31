using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Locations
{
    public class Transition : Location
    {
        public Transition(IntRect bounds)
        {
            connectedLocations = new();
            IntBounds = bounds;
            shape.FillColor = new(0, 255, 255, 120);
            TileCount = new(bounds.Width, bounds.Height);
            Position = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize * Compression);
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize * Compression);
            Random random = new();
            tiles = new TileState[TileCount.Y, TileCount.X];
            for (int i = 0; i < TileCount.Y; i++)
            {
                for (int j = 0; j < TileCount.X; j++)
                {
                    tiles[i, j] = new() { Type = TileType.GROUND, Id = (byte)random.Next(8) };
                }
            }
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize * Compression);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            foreach (var drawableObject in Objects)
            {
                if (drawingBounds.Intersects(drawableObject.Bounds))
                {
                    target.Draw(drawableObject, states);
                }
            }
            
            //shape.Draw(target, states);
            
        }
    }
}
