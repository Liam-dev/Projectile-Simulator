using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Simulator.Simulation;

namespace Simulator
{
    public class SimulationState
    {
        public List<object> Objects { get; set; }

        public bool Paused { get; set; }

        public Color BackgroundColour { get; set; } = Color.SkyBlue;

        public Vector2 Gravity { get; set; }

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
