using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    interface ISelectable
    {
        bool Selected { get; set; }

        bool Selectable { get; set; }

        bool Intersects(Vector2 point);
    }
}
