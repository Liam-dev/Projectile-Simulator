﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class StaticObject : CollisionObject, IPersistent
    {
        public StaticObject()
        {

        }

        public StaticObject(string name, Vector2 position, string textureName, float restitutionCoefficient) : base(name, position, textureName, restitutionCoefficient)
        {

        }
    }
}
