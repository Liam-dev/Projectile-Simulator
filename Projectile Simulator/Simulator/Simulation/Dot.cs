using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    class Dot : SimulationObject
    {
        public float Lifetime { get; protected set; }

        public Dot(string name, Vector2 position, float lifetime, string textureName = "dot") : base(name, position, textureName)
        {
            Lifetime = lifetime;
        }

        public override void Update(TimeSpan delta)
        {
            Lifetime -= (float)delta.TotalSeconds;
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
