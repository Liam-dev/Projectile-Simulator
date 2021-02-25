using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class Box : StaticObject, ISelectable, IMovable
    {
        protected Texture2D borderTexture;

        public Vector2 Dimensions { get; set; }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }

        public override Vector2 Centre
        {
            get { return Position + (Dimensions / 2); }
        }

        public bool Selected { get ; set; }
        public bool Selectable { get ; set; }

        public Box()
        {

        }

        public Box(string name, Vector2 position, string textureName, float restitutionCoefficient, Vector2 dimensions) : base(name, position, textureName, restitutionCoefficient)
        {
            Dimensions = dimensions;
            RestitutionCoefficient = 0.95f;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            borderTexture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            borderTexture.SetData(new[] { Color.White });

            base.OnLoad(Editor);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, BoundingBox, Color.White);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom);
            }  
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

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }
    }
}
