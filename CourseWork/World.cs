using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class World : Transformable, Drawable
    {
        private List<Location> locations;
        public List<Location> Locations { get { return locations; } }
        public Player Player { get; set; }
        public World(int locationsCount)
        {
            Player = new();
            locations = new();
            for(int i = 0; i < locationsCount; i++)
            {
                locations.Add(new Village());
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            throw new NotImplementedException();
        }
    }
}
