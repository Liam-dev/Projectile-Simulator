using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using MonoGame.Forms.Services;
using Simulator.Converters;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject used to measure distance in a simulation.
    /// </summary>
    class TapeMeasure : SimulationObject, IPersistent
    {
        // Font used for length display
        protected SpriteFont font;

        /// <summary>
        /// Gets or sets name of the tape measure's font.
        /// </summary>
        [Browsable(false)]
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the start handle of the tape measure.
        /// </summary>
        [Browsable(false)]
        public Handle Start { get; set; }

        /// <summary>
        /// Gets or sets the end handle of the tape measure.
        /// </summary>
        [Browsable(false)]
        public Handle End { get; set; }

        // Position becomes position of start handle
        public new Vector2 Position
        {
            get { return Start.Position; }
            set { Start.Position = value; }
        }

        /// <summary>
        /// Gets or sets the length of the tape measure.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public float Length
        {
            get { return (Start.Centre - End.Centre).Length(); }

            set { End.Centre = Start.Centre + Vector2.Normalize(Start.Centre - End.Centre) * value; }
        }

        /// <summary>
        /// Gets or sets the displayed scaled length of the tape measure. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Measure")]
        [DisplayName("Length")]
        public float DisplayLength
        {
            get { return ScaleConverter.Scale(Length, Scale, 1, true, 2); }
            set { Length = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        /// <summary>
        /// Gets or sets the thickness of the tape measure.
        /// </summary>
        [Browsable(true)]
        public int Thickness { get; set; }

        public TapeMeasure()
        {

        }

        public TapeMeasure(string name, Vector2 position, Vector2 end, int thickness, string textureName, string fontName) : base(name, position, textureName)
        {
            Start = new Handle(name + "Start", position, "handle", this);
            End = new Handle(name + "End", end, "handle", this);

            Thickness = thickness;
            FontName = fontName;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            font = Editor.Content.Load<SpriteFont>("Fonts/" + FontName);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            // Find angle (note negative y and negative angle for clockwise)
            float angle = -MathF.Atan2(-(End.Centre.Y - Start.Centre.Y), End.Centre.X - Start.Centre.X);

            // note power to scale
            Rectangle rectangle = new Rectangle((int)Start.Position.X, (int)Start.Position.Y, (int)Length, (int)MathF.Round(Thickness / MathF.Pow(zoom, 0.5f), 0));

            spriteBatch.Draw(texture, rectangle, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0.2f);

            spriteBatch.DrawString(font, DisplayLength.ToString() + " m", Start.Position + (End.Position - Start.Position) / 2 + new Vector2(40, 40), Color.Black, 0, Vector2.Zero, 1 / zoom, SpriteEffects.None, 0);
        }
    }
}
