using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Simulator.Simulation
{
    /// <summary>
    /// A CollisionObject which has physics influence in a simulation.
    /// </summary>
    public class PhysicsObject : CollisionObject
    {
        // Kinematic variables
        protected Vector2 velocity;
        protected Vector2 acceleration;

        // The current resultant force of object
        protected Vector2 resultantForce;

        // The instantaneous impulse to apply when object is next updated
        protected Vector2 impulse;

        /// <summary>
        /// Gets or sets the mass of the object.
        /// </summary>
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

            // Apply impulse
            velocity += impulse / Mass;
            impulse = Vector2.Zero;

            // Newton's second law
            acceleration = resultantForce / Mass;

            // Update kinematics
            velocity += acceleration * time;
            Position += velocity * time;

            base.Update(delta);
        }

        /// <summary>
        /// Applies a constant force to the object, through its centre of mass.
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector2 force)
        {
            resultantForce += force;
        }

        /// <summary>
        /// Applies an instantaneous impulse to the object, through its centre of mass.
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyImpulse(Vector2 impulse)
        {
            this.impulse += impulse;
        }

        /// <summary>
        /// Gets the velocity of the object.
        /// </summary>
        /// <returns>The object's velocity</returns>
        public Vector2 GetVelocity()
        {
            return velocity;
        }
    }
}
