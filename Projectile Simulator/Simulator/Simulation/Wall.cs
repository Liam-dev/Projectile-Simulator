using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    class Wall : Box, IScalable
    {
        public bool MaintainAspectRatio { get; set; }

        public Wall()
        {
        }

        public Wall(string name, Vector2 position, string textureName, float restitutionCoefficient, Vector2 dimensions) : base(name, position, textureName, restitutionCoefficient, dimensions)
        {
        }

        
    }
}
