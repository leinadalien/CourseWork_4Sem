using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Objects
{
    public class Stone : Object
    {
        public Stone(int id)
        {
            size = new(Tile.TileSize * 2, Tile.TileSize * 2, Tile.TileSize * 2);
            shape.Size = new(Width, Height);
            Origin = new(0, size.Y);
            shape.Texture = Content.StoneTexture;
            shape.TextureRect = new IntRect(id * 48, 0, 48, 48);
        }

        public override FloatRect Bounds {get { return new(new Vector2f(TruePosition.X, TruePosition.Y - size.Z), new(size.X, size.Y)); } }
    }
}
