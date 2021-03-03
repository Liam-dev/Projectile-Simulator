using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    class Detector : SimulationObject, IPersistent, ITrigger
    {
        public event EventHandler Triggered;

        public Detector(string name, Vector2 position, string textureName) : base(name, position, textureName)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            base.Draw(spriteBatch, zoom);
        }
    }
}
