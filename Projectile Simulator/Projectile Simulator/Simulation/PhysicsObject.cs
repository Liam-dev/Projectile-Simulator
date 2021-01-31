using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    public class PhysicsObject : CollisionObject
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Vector2 resultantForce;
        protected Vector2 impulse;

        public float Mass { get; set; }
        public float RestitutionCoefficient { get; set; }

        public PhysicsObject(Vector2 position, Texture2D texture, float mass) : base(position, texture)
        {
            Mass = mass;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity += impulse / Mass;
            impulse = Vector2.Zero;

            acceleration = resultantForce / Mass;

            velocity += acceleration * delta;
            Position += velocity * delta;
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
