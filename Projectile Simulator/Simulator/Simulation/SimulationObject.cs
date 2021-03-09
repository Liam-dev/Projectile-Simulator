using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
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
    [JsonObject]
    [TypeConverter(typeof(SerializableExpandableObjectConverter))]
    public class SimulationObject : ISelectable, IMovable
    {
        /// <summary>
        /// The texture of the object.
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// The texture of the selection border of the object.
        /// </summary>
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
        /// Gets or sets the position vector of the object.
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

        [Browsable(false)]
        [Category("Object")]
        public bool Selectable { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public bool Moving { get; set; }

        [Browsable(true)]
        [Category("Object")]
        [DefaultValue(true)]
        public bool Movable { get; set; }

        /// <summary>
        /// Parameterless constructor for SimulationObject.
        /// </summary>
        public SimulationObject()
        {

        }

        /// <summary>
        /// Constructor for SimulationObject.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        public SimulationObject(string name, Vector2 position, string textureName)
        {
            Name = name;
            Position = position;
            TextureName = textureName;
            Selectable = true;
            Movable = true;
        }

        /// <summary>
        /// Gets a string that represents the object.
        /// </summary>
        /// <returns>The Name of the object.</returns>
        public override string ToString()
        {
            return Name;
        }      

        /// <summary>
        /// Called when an object in loaded into a simulation.
        /// </summary>
        /// <param name="Editor">MonoGameServiceEditor.</param>
        public virtual void OnLoad(MonoGameService Editor)
        {
            // Load and apply texture
            texture = LoadTexture(TextureName, Editor.Content);

            // Load border texture
            borderTexture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            borderTexture.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Updates the object over a certain timespan.
        /// </summary>
        /// <param name="delta">The time since the last update.</param>
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
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }
        }

        /// <summary>
        /// Loads a texture from a name using a ContentManager.
        /// </summary>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="content">Content manager to load from.</param>
        /// <returns>Loaded Texture2D.</returns>
        protected Texture2D LoadTexture(string textureName, ContentManager content)
        {
            return content.Load<Texture2D>("Textures/" + textureName);
        }

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        public void Move(Vector2 displacement)
        {
            Position += displacement;
        }

        /// <summary>
        /// Draws a border outline around the object's bounding box.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="zoom">Zoom level of simulation.</param>
        /// <param name="rectangle">Rectangle to draw border around.</param>
        /// <param name="width">Width of the border to draw.</param>
        protected void DrawBorder(SpriteBatch spriteBatch, float zoom, Rectangle rectangle, int width)
        {
            // Border width
            int _width = (int)MathF.Max(1, MathF.Round(width / MathF.Pow(zoom, 0.5f), 0f));

            Rectangle[] border = new Rectangle[]
            {
                // Left
                new Rectangle(rectangle.Left - _width, rectangle.Top - _width, _width, rectangle.Height + (2 * _width)),

                // Right
                new Rectangle(rectangle.Right, rectangle.Top, _width, rectangle.Height + _width),

                // Top
                new Rectangle(rectangle.Left - _width, rectangle.Top - _width, rectangle.Width + (2 * _width), _width),

                // Bottom
                new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width + _width, _width)
            };

            // Draw all sides
            foreach (Rectangle side in border)
            {
                spriteBatch.Draw(borderTexture, side, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            }
        }   
    }
}
