using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Simulator.Converters;
using System;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which detects projectiles.
    /// </summary>
    public class Detector : SimulationObject, IPersistent, ITrigger
    {
        /// <summary>
        /// Enumeration of the horizontal and vertical directions.
        /// </summary>
        public enum DetectionDirection
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// Gets or sets whether the detector can detect objects.
        /// </summary>
        [Browsable(false)]
        public bool Detecting { get; set; }

        /// <summary>
        /// Gets or sets the direction the detector is detecting in.
        /// </summary>
        [Browsable(true)]
        [Category("Detector")]
        public DetectionDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the length of the detector.
        /// </summary>
        [Browsable(false)]
        public float Separation { get; set; }

        /// <summary>
        /// Gets or sets the displayed scaled separation of the detector. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [DisplayName("Separation")]
        [Category("Detector")]
        public float DisplaySeparation
        {
            get { return ScaleConverter.Scale(Separation, Scale, 1, false, 0); }
            set { Separation = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        /// <summary>
        /// Gets the area rectangle in which projectiles can be detected.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
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

        /// <summary>
        /// Parameterless constructor for Detector.
        /// </summary>
        public Detector()
        {
        }

        /// <summary>
        /// Constructor for Detector.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="separation">Detection length of the detector.</param>
        public Detector(string name, Vector2 position, string textureName, float separation) : base(name, position, textureName)
        {
            Separation = separation;
        }

        /// <summary>
        /// Triggers the detector with an object.
        /// </summary>
        /// <param name="object">Object detected.</param>
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

            // Determine texture section rectangles to draw.
            Rectangle start = new Rectangle(0, 0, 23, texture.Height);
            Rectangle middle = new Rectangle(24, 0, 1, texture.Height);
            Rectangle end = new Rectangle(73, 0, 23, texture.Height);

            if (Direction == DetectionDirection.Horizontal)
            {
                // Draw start
                spriteBatch.Draw(texture, new Rectangle(Position.ToPoint(), start.Size), start, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                // Draw middle
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0)).ToPoint(), new Point((int)Separation, texture.Height)), middle, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                // Draw end
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(start.Width, 0) + new Vector2(Separation, 0)).ToPoint(), end.Size), end, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
            else
            {
                // Draw start
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0)).ToPoint(), start.Size), start, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                // Draw middle
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0) + new Vector2(0, start.Width)).ToPoint(), new Point((int)Separation, texture.Height)), middle, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
                // Draw end
                spriteBatch.Draw(texture, new Rectangle((Position + new Vector2(texture.Height, 0) + new Vector2(0, start.Width) + new Vector2(0, Separation)).ToPoint(), end.Size), end, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}