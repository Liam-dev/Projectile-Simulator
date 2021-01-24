using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectile_Simulator.Simulation;

namespace Projectile_Simulator.Simulation
{
    public class Projectile : PhysicsObject
    {
        public Projectile(Vector2 position, Texture2D texture, float mass) : base(position, texture, mass)
        {
            resultantForce = mass * -9.8f * Vector2.UnitY;
            velocity = new Vector2(2f, 0);
        }
    }
}
