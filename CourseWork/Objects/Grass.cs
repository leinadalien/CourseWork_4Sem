using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Grass : Object
    {
        public Grass(int id)
        {
            IsTrigger = true;
            id %= 4;
            size = new(Tile.TileSize, Tile.TileSize, Tile.TileSize);
            Origin = new(0, size.Y);
            sprite.Texture = Content.GrassTexture;
            sprite.TextureRect = new IntRect(id * (int)size.X, 0, (int)size.X, (int)size.Y);
        }

        public override FloatRect Bounds { get { return new(TruePosition, new(size.X, size.Z)); } }
    }
}
