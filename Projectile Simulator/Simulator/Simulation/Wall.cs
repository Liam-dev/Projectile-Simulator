using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    class Wall : Box, IScalable
    {
        public Color Colour { get; set; }

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
