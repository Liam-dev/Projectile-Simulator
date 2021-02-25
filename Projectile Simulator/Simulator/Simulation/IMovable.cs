using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    interface IMovable
    {
        bool Moving { get; set; }
        void Move(Vector2 displacement);
    }
}
