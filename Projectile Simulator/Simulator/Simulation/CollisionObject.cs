using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Simulator.Simulation
{
    public class CollisionObject : SimulationObject
    {
        [Browsable(true)]
        [Category("Physics")]
        [DisplayName("Coefficient of restitution")]
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
