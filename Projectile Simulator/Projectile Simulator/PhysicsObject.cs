using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator
{
    class PhysicsObject : SimulationObject
    {
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected Vector2 resultantForce;
        protected float mass;

        public PhysicsObject(Vector2 position, Texture2D texture, float mass) : base(position, texture)
        {
            this.mass = mass;
            resultantForce = mass * 500f * Vector2.UnitY;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration = resultantForce / mass;
            velocity += acceleration * delta;
            Position += velocity * delta;
        }
    }
}
