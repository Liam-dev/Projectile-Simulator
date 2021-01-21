using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    class SimulationObject
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }

        public SimulationObject(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
