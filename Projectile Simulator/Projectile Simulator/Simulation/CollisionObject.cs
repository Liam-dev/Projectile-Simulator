using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectile_Simulator.Simulation
{
    public class CollisionObject : SimulationObject
    {
        public CollisionObject(Vector2 position, Texture2D texture) : base(position, texture)
        {

        }
    }
}
