using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Locations
{
    public class Transition : Location
    {
        private Location startLocation;
        private Location endLocation;
        public Transition(Location startLocation, Location endLocation)
        {
            shape.FillColor = new(0, 255, 255);
            this.startLocation = startLocation;
            this.endLocation = endLocation;
        }
    }
}
