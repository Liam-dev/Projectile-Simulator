using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using MonoGame.Forms.Services;
using Simulator.Converters;

namespace Simulator.Simulation
{
    class TapeMeasure : SimulationObject, IPersistent
    {
        protected SpriteFont font;

        [Browsable(false)]
        public string FontName { get; set; }

        public Handle Start { get; set; }
        public Handle End { get; set; }

        public new Vector2 Position
        {
            get { return Start.Position; }
            set { Start.Position = value; }
        }

        [JsonIgnore]
        [Browsable(false)]
        public float Length
        {
            get { return (Start.Centre - End.Centre).Length(); }

            set { End.Centre = Start.Centre + Vector2.Normalize(Start.Centre - End.Centre) * value; }
        }

        [JsonIgnore]
        [Browsable(true)]
        [Category("Measure")]
        [DisplayName("Length")]
        public float DisplayLength
        {
            get { return ScaleConverter.Scale(Length, Scale, 1, true, 2); }
            set { Length = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        [Browsable(true)]
        public int Thickness { get; set; }

        public TapeMeasure()
        {

        }

        public TapeMeasure(string name, Vector2 position, Vector2 end, int thickness, string textureName, string fontName) : base(name, position, textureName)
        {
            Start = new Handle(name + "Start", position, "handle");
            End = new Handle(name + "End", end, "handle");

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
            // find angle (note negative y and negative for clockwise)
            float angle = -MathF.Atan2(-(End.Centre.Y - Start.Centre.Y), End.Centre.X - Start.Centre.X);

            // note power to scale
            Rectangle rectangle = new Rectangle((int)Start.Position.X, (int)Start.Position.Y, (int)Length, (int)MathF.Round(Thickness / MathF.Pow(zoom, 0.5f), 0));

            spriteBatch.Draw(texture, rectangle, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0.2f);

            spriteBatch.DrawString(font, DisplayLength.ToString() + " m", Start.Position + (End.Position - Start.Position) / 2 + new Vector2(40, 40), Color.Black, 0, Vector2.Zero, 1 / zoom, SpriteEffects.None, 0);
        }
    }
}
