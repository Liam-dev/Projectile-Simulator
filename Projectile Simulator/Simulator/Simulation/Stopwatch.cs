using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which can measure time in a simulation.
    /// </summary>
    public class Stopwatch : SimulationObject, IPersistent
    {
        /// <summary>
        /// Font used for stopwatch display.
        /// </summary>
        protected SpriteFont font;

        /// <summary>
        /// Records if stopwatch is advancing in time.
        /// </summary>
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
        /// Gets the dictionary of trigger and stopwatch input pairs.
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

        /// <summary>
        /// Gets or sets the list of triggers for the stopwatch.
        /// </summary>
        [Browsable(false)]
        public List<(ITrigger, StopwatchInput)> Triggers { get; set; }

        /// <summary>
        /// Parameterless constructor for Stopwatch.
        /// </summary>
        public Stopwatch()
        {
        }

        /// <summary>
        /// Constructor for Stopwatch.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="fontName">Name of font to load.</param>
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

        /// <summary>
        /// Enumeration of the input that triggers can control a stopwatch on.
        /// </summary>
        public enum StopwatchInput
        {
            Start,
            Stop
        }

        /// <summary>
        /// Adds a trigger to the stopwatch on a certain input.
        /// </summary>
        /// <param name="trigger">The trigger to add.</param>
        /// <param name="input">The input to add the trigger to.</param>
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
        /// Removes a certain trigger to the stopwatch.
        /// </summary>
        /// <param name="trigger">The trigger to remove.</param>
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