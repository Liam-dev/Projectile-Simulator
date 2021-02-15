using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectile_Simulator.Simulation
{
    public class StaticObject : CollisionObject
    {
        public StaticObject()
        {

        }

        public StaticObject(Vector2 position, string textureName) : base(position, textureName)
        {

        }
    }
}
