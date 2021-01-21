using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator
{
    class Projectile : PhysicsObject
    {
        public Projectile(Vector2 position, Texture2D texture, float mass) : base(position, texture, mass)
        {
            resultantForce = mass * 500f * Vector2.UnitY;
            velocity = new Vector2(200, 0);
        }
    }
}
