using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame;

namespace Simulator.Simulation
{
    public class SimulationObject
    {
        public Vector2 Position { get; set; }

        public string TextureName { get; set; }

        protected Texture2D texture;

        public SimulationObject()
        {

        }

        public SimulationObject(Vector2 position, string textureName)
        {
            Position = position;
            TextureName = textureName; 
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Scaled position
            //Vector2 position = new Vector2(scale * Position.X, spriteBatch.GraphicsDevice.Viewport.Height - (scale * Position.Y));
            //spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.Draw(texture, Position, Color.White);
        }

        public void SetTexture(ContentManager content)
        {
            texture = content.Load<Texture2D>(TextureName);
        }
    }
}
