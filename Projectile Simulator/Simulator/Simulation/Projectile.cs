using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Simulator.Simulation;
using Simulator.Converters;

namespace Simulator.Simulation
{
    public class Projectile : PhysicsObject
    {
        protected Trajectory trajectory;

        [Browsable(true)]
        [DisplayName("Drag coefficient")]
        // Dimensionless constant
        public float DragCoefficient { get; set; }

        [Browsable(false)]
        public float Radius { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [DisplayName("Radius")]
        public float DisplayRadius
        {
            get { return ScaleConverter.Scale(Radius, Scale, 1, true, 2); }
            set { Radius = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(2 * Radius), (int)(2 * Radius)); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Vector2 Centre
        {
            get { return Position + new Vector2(Radius); }
        }

        public Projectile()
        {

        }

        public Projectile(string name, Vector2 position, string textureName, float mass, float restitutionCoefficient, float radius, float dragCoefficient) : base(name, position, textureName, mass, restitutionCoefficient)
        {
            Radius = radius;
            DragCoefficient = dragCoefficient;
            Movable = false;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            //Radius = texture.Width / 2;

            trajectory = new Trajectory(Position);

            
        }

        public override void Update(TimeSpan delta)
        {
            resultantForce = Vector2.Zero;
            
            ApplyForce(CalculateWeight());
            ApplyForce(CalulateDrag());

            ClampHorizontalSpeed();

            //trajectory.AddPoint(Position);

            base.Update(delta);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 2 * (int)Radius, 2 * (int)Radius), Color.White);
        }

        protected Vector2 CalulateDrag()
        {
            return 0.5f * DragCoefficient * velocity.Length() * -velocity / Radius;
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
