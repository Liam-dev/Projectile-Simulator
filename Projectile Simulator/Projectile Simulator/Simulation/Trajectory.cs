using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Projectile_Simulator.Simulation
{
    class Trajectory
    {
        protected List<Vector2> points;

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
