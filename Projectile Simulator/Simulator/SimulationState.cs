using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Simulator.Converters;
using Simulator.Simulation;

namespace Simulator
{
    /// <summary>
    /// An encapsulation of a simulation's objects and properties.
    /// </summary>
    public class SimulationState
    {
        /// <summary>
        /// Gets or sets the objects stored in the state.
        /// </summary>
        [Browsable(false)]
        public List<object> Objects { get; set; } = new List<object>();

        /// <summary>
        /// Gets or sets if the simulation is paused.
        /// </summary>
        [Browsable(false)]
        public bool Paused { get; set; }

        /// <summary>
        /// Gets or sets the background colour of the simulation.
        /// </summary>
        [Browsable(false)]
        public Color BackgroundColour { get; set; } = Color.SkyBlue;

        /// <summary>
        /// Gets or sets the displayed background colour as a System.Drawing.Color. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Category("Appearance")]
        [DisplayName("Background colour")]
        public System.Drawing.Color DisplayColour
        {
            get { return System.Drawing.Color.FromArgb(BackgroundColour.A, BackgroundColour.R, BackgroundColour.G, BackgroundColour.B); }
            set { BackgroundColour = new Color(value.R, value.G, value.B, value.A); }
        }

        /// <summary>
        /// Gets or sets the gravitational field strength in the simulation.
        /// </summary>
        [Browsable(false)]
        public Vector2 Gravity { get; set; }

        /// <summary>
        /// Gets or sets the displayed gravitational field strength as a float. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Category("Physics")]
        [DisplayName("Gravitational acceleration")]
        public float DisplayGravity
        {
            get { return ScaleConverter.Scale(Gravity.Y, SimulationObject.Scale, 1, true, 2); }
            set { Gravity = ScaleConverter.InverseScaleVector(new Vector2(0, value), SimulationObject.Scale, 1); }
        }

        /// <summary>
        /// Parameterless constructor for SimulationState.
        /// </summary>
        public SimulationState()
        {
            
        }

        /// <summary>
        /// Constructor for SimulationState from a list of objects.
        /// </summary>
        /// <param name="objects">Objects to save.</param>
        public SimulationState(List<object> objects)
        {
            foreach (object @object in objects)
            {
                if (@object is IPersistent)
                {
                    Objects.Add(@object);
                }
            }
        }
    }
}
