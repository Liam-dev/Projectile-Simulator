using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class Handle : SimulationObject
    {
        public event EventHandler PositionChanged;

        protected float zoom;

        public override Rectangle BoundingBox
        {
            get
            {
                if (texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width / zoom), (int)(texture.Height / zoom));
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        public Handle(string name, Vector2 position, string textureName) : base(name, position, textureName)
        {
            Selectable = true;
            Movable = true;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            this.zoom = zoom;
            spriteBatch.Draw(texture, Position, null, null, null, 0, new Vector2(1 / zoom), Color.White, SpriteEffects.None, 0);
        }
    }
}
