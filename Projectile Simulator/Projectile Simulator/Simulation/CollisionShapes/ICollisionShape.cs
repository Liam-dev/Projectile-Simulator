using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    public interface ICollisionShape
    {
        bool Colliding(ICollisionShape shape);
    }
}
