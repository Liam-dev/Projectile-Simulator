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
    public class SimulationState
    {
        [Browsable(false)]
        public List<object> Objects { get; set; }

        [Browsable(false)]
        public bool Paused { get; set; }

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

        [Browsable(false)]
        public Vector2 Gravity { get; set; }

        [JsonIgnore]
        [Category("Physics")]
        [DisplayName("Gravitational acceleration")]
        public float DisplayGravity
        {
            get { return ScaleConverter.Scale(Gravity.Y, SimulationObject.Scale, 1, true, 2); }
            set { Gravity = ScaleConverter.InverseScaleVector(new Vector2(0, value), SimulationObject.Scale, 1); }
        }

        public SimulationState()
        {
            Objects = new List<object>();
        }

        public SimulationState(List<object> objects)
        {
            Objects = new List<object>();

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
