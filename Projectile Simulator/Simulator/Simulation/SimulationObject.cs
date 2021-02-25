using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Forms.Services;
using System.ComponentModel;

namespace Simulator.Simulation
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SimulationObject : IMovable
    {
        public string Name { get; protected set; }

        public Vector2 Position { get; set; }

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

        public string TextureName { get; set; }

        public bool Moving { get; set; }

        protected Texture2D texture;

        protected Texture2D borderTexture;

        public SimulationObject()
        {

        }

        public SimulationObject(string name, Vector2 position, string textureName)
        {
            Name = name;
            Position = position;
            TextureName = textureName; 
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
        }

        public void SetTexture(string textureName, ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
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
