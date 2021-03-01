using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel;

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
