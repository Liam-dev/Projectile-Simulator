using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Newtonsoft.Json;
using Simulator.Converters;
using System;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject used to measure distance in a simulation.
    /// </summary>
    class TapeMeasure : SimulationObject, IPersistent
    {
        /// <summary>
        /// Font used for length display
        /// </summary>
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

        /// <summary>
        /// Gets or sets the position of the tape measure's start handle.
        /// </summary>
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

        /// <summary>
        /// Parameterless constructor for TapeMeasure.
        /// </summary>
        public TapeMeasure()
        {
        }

        /// <summary>
        /// Constructor for TapeMeasure
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place start of tape measure.</param>
        /// <param name="end">Position to place end of tape measure.</param>
        /// <param name="thickness">Thickness of tape measure.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="fontName">Name of font to load.</param>
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
            // Find angle (note negative angle for clockwise rotation)
            float angle = -MathF.Atan2(-(End.Centre.Y - Start.Centre.Y), End.Centre.X - Start.Centre.X);

            // Drawn rectangle width is scaled using the zoom to -0.5 power to maintain size at differnt zoom levels
            Rectangle rectangle = new Rectangle((int)Start.Position.X, (int)Start.Position.Y, (int)Length, (int)MathF.Round(Thickness * MathF.Pow(zoom, -0.5f), 0));

            spriteBatch.Draw(texture, rectangle, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0.2f);

            // Draw length as string the side of the centre of the tape measure
            spriteBatch.DrawString(font, DisplayLength.ToString() + " m", Start.Position + (End.Position - Start.Position) / 2 + new Vector2(40, 40), Color.Black, 0, Vector2.Zero, 1 / zoom, SpriteEffects.None, 0);
        }
    }
}