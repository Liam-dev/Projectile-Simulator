using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class Detector : SimulationObject, IPersistent, ITrigger
    {
        public enum DetectionDirection
        {
            Horizontal,
            Vertical
        }

        public bool Detecting{ get; set; }

        public DetectionDirection Direction { get; set; }

        public float Separation { get; set; }

        [JsonIgnore]
        public Rectangle DetectionArea
        {
            get
            {
                if (texture != null)
                {
                    if (Direction == DetectionDirection.Horizontal)
                    {
                        return new Rectangle((int)Position.X + 23, (int)Position.Y + 30, (int)Separation, 3);
                    }
                    else
                    {
                        return new Rectangle((int)Position.X + 30, (int)Position.Y + 23, 3, (int)Separation);
                    }        
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        [JsonIgnore]
        public override Rectangle BoundingBox
        {
            get
            {
                if (texture != null)
                {
                    if (Direction == DetectionDirection.Horizontal)
                    {
                        return new Rectangle((int)Position.X, (int)Position.Y, texture.Width - 50 + (int)Separation, texture.Height);
                    }
                    else
                    {
                        return new Rectangle((int)Position.X, (int)Position.Y, texture.Height, texture.Width - 50 + (int)Separation);
                    }
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }
        

        public event EventHandler Triggered;

        public Detector()
        {

        }

        public Detector(string name, Vector2 position, string textureName) : base(name, position, textureName)
        {

        }

        public void OnObjectEntered(SimulationObject @object)
        {
            Triggered?.Invoke(this, new EventArgs());
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            if (Selected)
            {
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }

            float rotation = (int)Direction * 0.5f * MathF.PI;

            Rectangle start = new Rectangle(0, 0, 23, texture.Height);
            Rectangle middle = new Rectangle(24, 0, 1, texture.Height);
            Rectangle end = new Rectangle(73, 0, 23, texture.Height);
            
            if (Direction == DetectionDirection.Horizontal)
            {
                spriteBatch.Draw(texture, new Rectangle(Position.ToPoint(), start.Size), start, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0)).ToPoint(), new Point((int)Separation, texture.Height)), middle, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0) + new Vector2(Separation, 0)).ToPoint(), end.Size), end, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0)).ToPoint(), start.Size), start, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0) + new Vector2(0, start.Width)).ToPoint(), new Point((int)Separation, texture.Height)), middle, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0) + new Vector2(0, start.Width) + new Vector2(0, Separation)).ToPoint(), end.Size), end, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}
