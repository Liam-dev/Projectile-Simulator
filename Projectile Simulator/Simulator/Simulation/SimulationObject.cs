using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;

namespace Simulator.Simulation
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SimulationObject
    {
        public string Name { get; protected set; }

        public Vector2 Position { get; set; }

        public string TextureName { get; set; }

        protected Texture2D texture;

        public SimulationObject()
        {

        }

        public SimulationObject(string name, Vector2 position, string textureName)
        {
            Name = name;
            Position = position;
            TextureName = textureName; 
        }

        public virtual void OnLoad(ContentManager content)
        {
            SetTexture(TextureName, content);
        }

        public virtual void Update(TimeSpan delta)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Scaled position
            //Vector2 position = new Vector2(scale * Position.X, spriteBatch.GraphicsDevice.Viewport.Height - (scale * Position.Y));
            //spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.Draw(texture, Position, Color.White);
        }

        public void SetTexture(string textureName, ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
        }
    }
}
