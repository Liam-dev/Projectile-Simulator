using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    public class SimulationObject
    {
        public Vector2 Position { get; protected set; }
        protected Texture2D texture;

        public SimulationObject(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Position.X, spriteBatch.GraphicsDevice.Viewport.Height - Position.Y);
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
