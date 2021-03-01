using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Simulator.Converters;

namespace Simulator.Simulation
{
    class Wall : Box, IScalable
    {
        [Browsable(false)]
        [Category("Object")]
        public Color Colour { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [Category("Box")]
        [DisplayName("Colour")]
        public System.Drawing.Color DisplayColour
        {
            get { return System.Drawing.Color.FromArgb(Colour.A, Colour.R, Colour.G, Colour.B); }
            set { Colour = new Color(value.R, value.G, value.B, value.A); texture.SetData(new[] { Colour }); }
        }

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
        public override float DiplaySize { get => base.DiplaySize; set => base.DiplaySize = value; }

        [Browsable(false)]
        public bool MaintainAspectRatio { get; set; }

        public Wall()
        {
        }

        public Wall(string name, Vector2 position, Color colour, float restitutionCoefficient, Vector2 dimensions) : base(name, position, "wall", restitutionCoefficient, dimensions)
        {
            Colour = colour;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            texture = new Texture2D(Editor.graphics, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Colour });
        }

    }
}
