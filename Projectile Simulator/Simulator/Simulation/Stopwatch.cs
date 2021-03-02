using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Simulator.Simulation
{
    public class Stopwatch : SimulationObject
    {
        protected SpriteFont font;

        [Browsable(false)]
        public string FontName { get; set; }

        public TimeSpan Time { get; set; }

        public Stopwatch(string name, Vector2 position, string textureName, string fontName) : base(name, position, textureName)
        {
            FontName = fontName;
            Selectable = true;
            Movable = true;
            Time = TimeSpan.FromMilliseconds(3141);
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            font = Editor.Content.Load<SpriteFont>("Fonts/" + FontName);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, Position, Color.White);
            spriteBatch.DrawString(font, Time.ToString(@"ss\.fff"), Position + new Vector2(24, 8), Color.Black);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom);
            }
        }
    }
}
