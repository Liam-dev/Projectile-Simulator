﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectile_Simulator.Simulation
{
    class Dot : SimulationObject
    {
        public float Lifetime { get; protected set; }

        public Dot(Vector2 position, float lifetime, string textureName = "dot") : base(position, textureName)
        {
            Lifetime = lifetime;
        }

        public override void Update(GameTime gameTime)
        {
            Lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Lifetime > 0)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
