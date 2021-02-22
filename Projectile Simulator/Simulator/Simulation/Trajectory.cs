using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    public class Trajectory
    {
        protected List<Vector2> points = new List<Vector2>();

        public Trajectory(Vector2 position)
        {
            AddPoint(position);
        }

        public void AddPoint(Vector2 position)
        {
            points.Add(position);
        }
    }
}
