using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class FatTree : Object
    {
        public FatTree(int id)
        {
            id %= 2;
            size = new(Tile.TileSize * 3, Tile.TileSize * 3, Tile.TileSize);
            Origin = new(Tile.TileSize, size.Y);
            sprite.Texture = Content.FatTreesTexture;
            sprite.TextureRect = new IntRect(id * (int)size.X, 0, (int)size.X, (int)size.Y);
        }
        public override FloatRect Bounds { get { return new(new Vector2f(TruePosition.X, TruePosition.Y - size.Z), new(Tile.TileSize, size.Z)); } }
    }
}
