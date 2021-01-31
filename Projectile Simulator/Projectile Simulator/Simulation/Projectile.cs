using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectile_Simulator.Simulation;

namespace Projectile_Simulator.Simulation
{
    public class Projectile : PhysicsObject
    {
        public float DragCoefficient { get; set; }
        public float Radius { get { return texture.Width / 2; } }

        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture, float mass) : base(position, texture, mass)
        {
            DragCoefficient = 0f;           
            RestitutionCoefficient = 1f;
            this.velocity = velocity;
        }

        public override void Update(GameTime gameTime)
        {
            resultantForce = Vector2.Zero;
            //resultantForce += Mass * 980 * Vector2.UnitY;
            resultantForce += CalulateDrag();

            base.Update(gameTime);
        }

        protected Vector2 CalulateDrag()
        {
            //float area = texture.Height * texture.Width * MathF.PI * 0.25f;
            return DragCoefficient * velocity.Length() * -velocity;
        }
    }

}
