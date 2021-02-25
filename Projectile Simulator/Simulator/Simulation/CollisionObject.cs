using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    public class CollisionObject : SimulationObject
    {
        public float RestitutionCoefficient { get; set; }

        public CollisionObject()
        {

        }

        public CollisionObject(string name, Vector2 position, string textureName, float restitutionCoefficient) : base(name, position, textureName)
        {
            RestitutionCoefficient = restitutionCoefficient;
        }
    }
}
