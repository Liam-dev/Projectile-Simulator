using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which can measure time in a simulation.
    /// </summary>
    public class Stopwatch : SimulationObject, IPersistent
    {
        // Font used for stopwatch display
        protected SpriteFont font;

        // Records if stopwatch is advancing
        protected bool running;

        /// <summary>
        /// Gets or sets name of the stopwatch's font.
        /// </summary>
        [Browsable(false)]
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the time stored in the stopwatch.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Time recorded")]
        public TimeSpan Timer { get; set; }

        /// <summary>
        /// Gets or sets triggers for the stopwatch.
        /// </summary>
        [Browsable(false)]
        public List<(ITrigger, StopwatchInput)> Triggers { get; set; }

        public Stopwatch()
        {

        }

        public Stopwatch(string name, Vector2 position, string textureName, string fontName) : base(name, position, textureName)
        {
            FontName = fontName;

            Triggers = new List<(ITrigger, StopwatchInput)>();

            Selectable = true;
            Movable = true;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            font = Editor.Content.Load<SpriteFont>("Fonts/" + FontName);

            // Add triggers
            foreach (var trigger in Triggers)
            {
                switch (trigger.Item2)
                {
                    case StopwatchInput.Start:
                        trigger.Item1.Triggered += StartTrigger_Triggered;
                        break;

                    case StopwatchInput.Stop:
                        trigger.Item1.Triggered += StopTrigger_Triggered;
                        break;
                };
            }
        }

        public override void Update(TimeSpan delta)
        {
            if (running)
            {
                // Add the update frame time to timer
                Timer += delta;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.09f);
            spriteBatch.DrawString(font, Timer.ToString(@"ss\.ff"), Position + new Vector2(24, 8), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f);

            if (Selected)
            {
                DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }
        }

        /// <summary>
        /// Starts the stopwatches timer.
        /// </summary>
        public void TimerStart()
        {
            running = true;
            Timer = TimeSpan.Zero;
        }

        /// <summary>
        /// Stops the stopwatches timer.
        /// </summary>
        public void TimerStop()
        {
            running = false;
        }

        public enum StopwatchInput
        {
            Start,
            Stop
        }

        /// <summary>
        /// Adds a trigger to the stopwatch on a certain input.
        /// </summary>
        /// <param name="trigger">The trigger to add</param>
        /// <param name="input">The input to add the trigger to</param>
        public void AddTrigger(ITrigger trigger, StopwatchInput input)
        {
            RemoveTrigger(trigger, input);

            Triggers.Add((trigger, input));

            switch (input)
            {
                case StopwatchInput.Start:
                    trigger.Triggered -= StartTrigger_Triggered;
                    trigger.Triggered += StartTrigger_Triggered;
                    break;

                case StopwatchInput.Stop:
                    trigger.Triggered -= StopTrigger_Triggered;
                    trigger.Triggered += StopTrigger_Triggered;
                    break;
            }
            
        }

        /// <summary>
        /// Removes a trigger to the stopwatch on a certain input.
        /// </summary>
        /// <param name="trigger">The trigger to remove</param>
        /// <param name="input">The input to remove the trigger from</param>
        public void RemoveTrigger(ITrigger trigger, StopwatchInput input)
        {
            Triggers.Remove((trigger,StopwatchInput.Start));
            Triggers.Remove((trigger, StopwatchInput.Stop));

            switch (input)
            {
                case StopwatchInput.Start:
                    trigger.Triggered -= StartTrigger_Triggered;
                    break;

                case StopwatchInput.Stop:
                    trigger.Triggered -= StopTrigger_Triggered;
                    break;
            }    
        }

        private void StartTrigger_Triggered(object sender, EventArgs e)
        {
            TimerStart();
        }

        private void StopTrigger_Triggered(object sender, EventArgs e)
        {
            TimerStop();
        }
    }
}
