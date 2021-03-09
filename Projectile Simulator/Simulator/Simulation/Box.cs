using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Simulator.Converters;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A StaticObject which has rectangular axis-aligned dimensions.
    /// </summary>
    public class Box : StaticObject
    {
        /// <summary>
        /// Gets or sets the dimensions of the box.
        /// </summary>
        [Browsable(false)]
        public Vector2 Dimensions { get; set; }

        /// <summary>
        /// Gets or sets the displayed scaled linear dimensions of the box. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Box")]
        [DisplayName("Size")]
        public virtual float DiplaySize
        {
            get { return ScaleConverter.Scale(Dimensions.X, Scale, 1, true, 2); }
            set { Dimensions = new Vector2(ScaleConverter.InverseScale(value, Scale, 1)); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Vector2 Centre
        {
            get { return Position + (Dimensions / 2); }
        }

        /// <summary>
        /// Parameterless constructor for Box.
        /// </summary>
        public Box()
        {

        }

        /// <summary>
        /// Constructor for Box.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="textureName"></param>
        /// <param name="restitutionCoefficient"></param>
        /// <param name="dimensions"></param>
        public Box(string name, Vector2 position, string textureName, float restitutionCoefficient, Vector2 dimensions) : base(name, position, textureName, restitutionCoefficient)
        {
            Dimensions = dimensions;
            RestitutionCoefficient = 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, BoundingBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.01f);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }  
        }
    }
}
