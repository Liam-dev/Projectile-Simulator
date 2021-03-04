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

        public DetectionDirection Direction { get; set; }

        public float Separation { get; set; }

        [JsonIgnore]
        public Rectangle DetectionArea
        {
            get
            {
                if(texture != null)
                {
                    return new Rectangle((int)Position.X + 23, (int)Position.Y + 30, (int)Separation, 3);
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
                    return new Rectangle((int)Position.X, (int)Position.Y, texture.Width - 50 + (int)Separation, texture.Height);
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

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            if (Selected)
            {
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }

            Rectangle start = new Rectangle(0, 0, 23, 64);
            Rectangle middle = new Rectangle(24, 0, 1, 64);
            Rectangle end = new Rectangle(73, 0, 23, 64);

            spriteBatch.Draw(texture, new Rectangle(Position.ToPoint(), start.Size), start, Color.White);
            spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0)).ToPoint(), new Point((int)Separation, 64)), middle, Color.White);
            spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0) + new Vector2(Separation, 0)).ToPoint(), end.Size), end, Color.White);
        }
    }
}
