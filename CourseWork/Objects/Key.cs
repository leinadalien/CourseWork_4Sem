using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Key : Object
    {
        public Key()
        {
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            Origin = new(Tile.TileSize, size.Y);
            sprite.Texture = Content.KeyTexture;
            sprite.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);
        }
        public override FloatRect Bounds { get { return new(0, 0, 0, 0); } }
    }
}
