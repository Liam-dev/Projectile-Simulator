using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Simulator.Converters;

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
        [JsonIgnore]
        public Dictionary<ITrigger, StopwatchInput> TriggerDictionary
        {
            get
            {
                Dictionary<ITrigger, StopwatchInput> result = new Dictionary<ITrigger, StopwatchInput>();
                foreach (var trigger in Triggers)
                {
                    result.Add(trigger.Item1, trigger.Item2);
                }

                return result;
            }
        }

        [Browsable(false)]
        public List<(ITrigger, StopwatchInput)> Triggers { get; set; }

        [Browsable(false)]
        public List<ITrigger> TestTriggers { get; set; } = new List<ITrigger>();

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

            foreach (var trigger in TriggerDictionary)
            {
                switch (trigger.Value)
                {
                    case StopwatchInput.Start:
                        trigger.Key.Triggered += StartTrigger_Triggered;
                        break;

                    case StopwatchInput.Stop:
                        trigger.Key.Triggered += StopTrigger_Triggered;
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
            RemoveTrigger(trigger);

            Triggers.Add((trigger, input));

            switch (input)
            {
                case StopwatchInput.Start:
                    trigger.Triggered += StartTrigger_Triggered;
                    break;

                case StopwatchInput.Stop:
                    trigger.Triggered += StopTrigger_Triggered;
                    break;
            }
            
        }

        /// <summary>
        /// Removes a trigger to the stopwatch on a certain input.
        /// </summary>
        /// <param name="trigger">The trigger to remove</param>
        /// <param name="input">The input to remove the trigger from</param>
        public void RemoveTrigger(ITrigger trigger)
        {
            Triggers.RemoveAll(x => x.Item1 == trigger);

            trigger.Triggered -= StartTrigger_Triggered;
            trigger.Triggered -= StopTrigger_Triggered;  
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
