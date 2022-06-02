﻿using CourseWork.Objects;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Locations
{
    public class Glade : Location
    {
        public Glade(IntRect bounds)
        {
            IntBounds = bounds;
            TileCount = new(bounds.Width, bounds.Height);
            TruePosition = new(bounds.Left * Tile.TileSize, bounds.Top * Tile.TileSize);

            var obj = new Stone
            {
                TruePosition = new(3 * Tile.TileSize, 3 * Tile.TileSize)
            };
            obj.TruePosition += TruePosition;
            Objects.Add(obj);
            
            size = new(TileCount.X * Tile.TileSize, 0, TileCount.Y * Tile.TileSize);
            StartPosition = new Vector2f(15 * Tile.TileSize, 15 * Tile.TileSize);
        }

        
    }
}
