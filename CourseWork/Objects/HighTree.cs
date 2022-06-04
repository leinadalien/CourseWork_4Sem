using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class HighTree : Object
    {
        public HighTree(int id)
        {
            id %= 4;
            size = new(Tile.TileSize, Tile.TileSize * 4, Tile.TileSize);
            Origin = new(0, size.Y);
            sprite.Texture = Content.HighTreesTexture;
            sprite.TextureRect = new IntRect(id * (int)size.X, 0, (int)size.X, (int)size.Y);
        }
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X + Tile.TileSize / 4, TruePosition.Y - size.Z), new(size.X / 2, size.Z / 2)); } }
    }
}
