using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectile_Simulator.Simulation
{
    public class Box : StaticObject
    {
        public Vector2 Dimensions { get; set; }

        public Box()
        {

        }

        public Box(Vector2 position, string textureName, Vector2 dimensions) : base(position, textureName)
        {
            Dimensions = dimensions;
            RestitutionCoefficient = 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
