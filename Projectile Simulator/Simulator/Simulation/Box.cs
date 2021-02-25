using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class Box : StaticObject, ISelectable
    {
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

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }
    }
}
