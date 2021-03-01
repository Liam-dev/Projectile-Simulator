using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Simulator.Simulation
{
    public class PhysicsObject : CollisionObject
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Vector2 resultantForce;
        protected Vector2 impulse;

        [Browsable(true)]
        public float Mass { get; set; }
        
        public PhysicsObject()
        {

        }

        public PhysicsObject(string name, Vector2 position, string textureName, float mass, float restitutionCoefficient) : base(name, position, textureName, restitutionCoefficient)
        {
            Mass = mass;
        }

        public override void Update(TimeSpan delta)
        {
            float time = (float)delta.TotalSeconds;

            velocity += impulse / Mass;
            impulse = Vector2.Zero;

            acceleration = resultantForce / Mass;

            velocity += acceleration * time;
            Position += velocity * time;

            base.Update(delta);
        }

        public void ApplyForce(Vector2 force)
        {
            resultantForce += force;
        }

        public void ApplyImpulse(Vector2 impulse)
        {
            this.impulse += impulse;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }
    }
}
