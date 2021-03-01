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
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SimulationObject : ISelectable, IMovable
    {
        public static float Scale;

        protected Texture2D texture;
        protected Texture2D borderTexture;

        [Browsable(true)]
        [Category("Object")]
        public string Name { get; set; }

        [Browsable(false)]
        public Vector2 Position { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [DisplayName("Position")]
        [Category("Object")]        
        public Vector2 DisplayPosition
        {
            get { return ScaleConverter.ScaleVector(Position, Scale, 1, true, 2); }
            set { Position = ScaleConverter.InverseScaleVector(value, Scale, 1); }
        }

        [Browsable(false)]
        public string TextureName { get; set; }

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

        public virtual void OnLoad(MonoGameService Editor)
        {
            SetTexture(TextureName, Editor.Content);

            borderTexture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            borderTexture.SetData(new[] { Color.White });
        }

        public virtual void Update(TimeSpan delta)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, Position, Color.White);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom);
            }
        }

        public void SetTexture(string textureName, ContentManager content)
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


            foreach (Rectangle side in border)
            {
                spriteBatch.Draw(borderTexture, destinationRectangle: side, color: Color.White);
            }
        }   
    }
}
