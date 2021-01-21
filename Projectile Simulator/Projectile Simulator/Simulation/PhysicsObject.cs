using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation
{
    public class PhysicsObject : SimulationObject
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Vector2 resultantForce;

        public float Mass { get; set; }

        public PhysicsObject(Vector2 position, Texture2D texture, float mass) : base(position, texture)
        {
            Mass = mass;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration = resultantForce / Mass;
            velocity += acceleration * delta;
            Position += velocity * delta;
        }

        public void ApplyForce(Vector2 force, Vector2 offset = default)
        {
            resultantForce += force;
        }
    }
}
