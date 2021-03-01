﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Simulator.Converters;
using System.ComponentModel;

namespace Simulator.Simulation
{
    public class Box : StaticObject
    {
        [Browsable(false)]
        public Vector2 Dimensions { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [Category("Box")]
        [DisplayName("Dimensions")]
        [TypeConverter(typeof(System.Drawing.SizeFConverter))]
        public System.Drawing.SizeF DiplayDimensions
        {
            get { return VectorSizeConverter.VectorToSize(ScaleConverter.ScaleVector(Dimensions, Scale, 1, true, 2)); }
            set { Dimensions = ScaleConverter.InverseScaleVector(VectorSizeConverter.SizeToVector(value), Scale, 1); }
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

        public Box()
        {

        }

        public Box(string name, Vector2 position, string textureName, float restitutionCoefficient, Vector2 dimensions) : base(name, position, textureName, restitutionCoefficient)
        {
            Dimensions = dimensions;
            RestitutionCoefficient = 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, BoundingBox, Color.White);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom);
            }  
        }
    }
}
