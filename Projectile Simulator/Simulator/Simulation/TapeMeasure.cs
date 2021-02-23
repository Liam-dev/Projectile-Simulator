using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Simulator.Simulation
{
    class TapeMeasure : SimulationObject
    {
        protected SpriteFont font;

        public Vector2 End { get; set; }

        public float Length
        {
            get { return (Position - End).Length(); }

            set { End = Position + Vector2.Normalize(Position - End) * value; }
        }

        public int Thickness { get; set; }

        public TapeMeasure(string name, Vector2 position, Vector2 end, int thickness, string textureName/*, SpriteFont font*/) : base(name, position, textureName)
        {
            End = end;
            Thickness = thickness;
            //this.font = font;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            // find angle (note negative y and negative for clockwise)
            float angle = -MathF.Atan2(-(End.Y - Position.Y), End.X - Position.X);

            // note power to scale
            Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Length, (int)(Thickness / MathF.Pow(zoom, 0.5f)));

            spriteBatch.Draw(texture, rectangle, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);

            //spriteBatch.DrawString(font, Length.ToString(), (End - Position) / 2 + new Vector2(10, 0), Color.Black);
        }
    }
}
