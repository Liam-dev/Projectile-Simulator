using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Simulator.Simulation;

namespace Simulator.Simulation
{
    public class Projectile : PhysicsObject
    {
        protected Trajectory trajectory;

        public float DragCoefficient { get; set; }
        public float Radius
        {
            get
            {
                if (texture != null)
                {
                    return texture.Width / 2;
                }
                else
                {
                    return 0;
                }              
            }
        }

        public Projectile()
        {

        }

        public Projectile(string name, Vector2 position, string textureName, float mass, float restitutionCoefficient, float dragCoefficient) : base(name, position, textureName, mass, restitutionCoefficient)
        {
            DragCoefficient = dragCoefficient;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            trajectory = new Trajectory(Position);

            base.OnLoad(Editor);
        }

        public override void Update(TimeSpan delta)
        {
            resultantForce = Vector2.Zero;
            
            ApplyForce(CalculateWeight());
            ApplyForce(CalulateDrag());

            ClampHorizontalSpeed();

            trajectory.AddPoint(Position);

            base.Update(delta);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        protected Vector2 CalulateDrag()
        {
            //float area = texture.Height * texture.Width * MathF.PI * 0.25f;
            return DragCoefficient * velocity.Length() * -velocity;
        }

        protected Vector2 CalculateWeight()
        {
            return Mass * 980 * Vector2.UnitY;
        }

        protected void ClampHorizontalSpeed()
        {
            if (MathF.Abs(velocity.X) < 1)
            {
                velocity.X = 0;
            }
        }
    }

}
