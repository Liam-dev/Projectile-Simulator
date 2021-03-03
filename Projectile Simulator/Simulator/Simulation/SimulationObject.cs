using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Forms.Services;
using System.ComponentModel;
using Simulator.Converters;

namespace Simulator.Simulation
{
    /// <summary>
    /// Base class for an object in a simulation.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SimulationObject : ISelectable, IMovable
    {
        // Texture of object
        protected Texture2D texture;

        // Texture of selection border
        protected Texture2D borderTexture;

        /// <summary>
        /// Gets or sets the length scale the simulation is using to determine how many pixels represents one metre.
        /// </summary>
        public static float Scale { get; set; }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        [Browsable(true)]
        [Category("Object")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the position of the object.
        /// </summary>
        [Browsable(false)]
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the displayed scaled position of the object. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [DisplayName("Position")]
        [Category("Object")]        
        public Vector2 DisplayPosition
        {
            get { return ScaleConverter.ScaleVector(Position, Scale, 1, true, 2); }
            set { Position = ScaleConverter.InverseScaleVector(value, Scale, 1); }
        }

        /// <summary>
        /// Gets or sets the name of the objects texture.
        /// </summary>
        [Browsable(false)]
        public string TextureName { get; set; }

        /// <summary>
        /// Gets the rectangular bounding box of the object.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual Rectangle BoundingBox
        {
            get
            {
                if (texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the centre of the object.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual Vector2 Centre
        {
            get
            {
                if (texture != null)
                {
                    return Position + (new Vector2(texture.Width, texture.Height) / 2);
                }
                else
                {
                    return Vector2.Zero;
                }
            }

            set
            {
                if (texture != null)
                {
                    Position = value - (new Vector2(texture.Width, texture.Height) / 2);
                }
            }
        }

        [Browsable(false)]
        public bool Selected { get; set; }

        [Browsable(true)]
        [Category("Object")]
        public bool Selectable { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public bool Moving { get; set; }

        [Browsable(true)]
        [Category("Object")]
        [DefaultValue(true)]
        public bool Movable { get; set; }

        public SimulationObject()
        {
        }

        public SimulationObject(string name, Vector2 position, string textureName)
        {
            Name = name;
            Position = position;
            TextureName = textureName;
            Movable = true;
        }

        /// <summary>
        /// Called when an object in loaded into a simulation.
        /// </summary>
        /// <param name="Editor">MonoGameServiceEditor</param>
        public virtual void OnLoad(MonoGameService Editor)
        {
            // Load and apply texture
            SetTexture(TextureName, Editor.Content);

            // Load border texture
            borderTexture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            borderTexture.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Updates the object over a certain timespan.
        /// </summary>
        /// <param name="delta">The time since the last update</param>
        public virtual void Update(TimeSpan delta)
        {
            
        }

        /// <summary>
        /// Draws the object to a spritebatch.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw object to</param>
        /// <param name="zoom">Simulation zoom level</param>
        public virtual void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, Position, Color.White);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom);
            }
        }

        // Sets the object's texture from a texture name
        protected void SetTexture(string textureName, ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/" + textureName);
        }

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        public void Move(Vector2 displacement)
        {
            Position += displacement;
        }     

        // Draws a border outline around the object's bounding box
        protected void DrawBorder(SpriteBatch spriteBatch, float zoom)
        {
            // Border width
            int width = (int)MathF.Max(1, MathF.Round(4 / MathF.Pow(zoom, 0.5f), 0f));

            Rectangle[] border = new Rectangle[]
            {
                // Left
                new Rectangle(BoundingBox.Left - width, BoundingBox.Top - width, width, BoundingBox.Height + (2 * width)),

                // Right
                new Rectangle(BoundingBox.Right, BoundingBox.Top, width, BoundingBox.Height + width),

                // Top
                new Rectangle(BoundingBox.Left - width, BoundingBox.Top - width, BoundingBox.Width + (2 * width), width),

                // Bottom
                new Rectangle(BoundingBox.Left, BoundingBox.Bottom, BoundingBox.Width + width, width)
            };

            // Draw all sides
            foreach (Rectangle side in border)
            {
                spriteBatch.Draw(borderTexture, destinationRectangle: side, color: Color.White);
            }
        }   
    }
}
