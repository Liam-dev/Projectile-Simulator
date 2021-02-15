using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    public class Cannon : SimulationObject
    {
        public int ATestProperty { get; set; }

        public Cannon()
        {

        }

        public Cannon(Vector2 position, string textureName) : base(position, textureName)
        {
            ATestProperty = 42;
        }
    }
}
