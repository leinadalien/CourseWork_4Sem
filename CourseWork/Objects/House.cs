using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class House : Object
    {
        public House()
        {
            size = new(Tile.TileSize * 3, Tile.TileSize * 4, Tile.TileSize * 3);
            Origin = new(0, size.Y);
            sprite.Texture = Content.HouseTexture;
            sprite.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);
        }
        public override FloatRect Bounds { get { return new(new(TruePosition.X + Tile.TileSize / 4, TruePosition.Y - size.Z), new(size.X - Tile.TileSize / 2, size.Z)); } }
    }
}
